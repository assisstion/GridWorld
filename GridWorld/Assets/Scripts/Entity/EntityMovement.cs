using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EntityMovement : NetworkBehaviour, Initializable{

	public ServerOnlyScript server;
	public GameObject client;

	public MapGenerator map;

	protected bool init;

	private int direction{
		get{
			return _direction;
		}
	}

	protected int _direction;

	public int playerX{
		get{
			return _playerX;
		}
	}

	protected int _playerX;

	public int playerY{
		get{
			return _playerY;
		}
	}

	protected int _playerY;
	public float defaultMoveCooldown = 0.5f;
	public float defaultTurnCooldown = 0.15f;
	protected float moveCooldown;
	protected float turnCooldown;
	//protected float speed;

	public void Setup(int x, int y, int dir){
		TryMove(x, y, dir, MoveMode.NoEvent);
	}

	//0 = move success
	//1 = move fail, no turn
	//2 = turn success
	public int GoTowards(int dir){
		if(direction == dir){
			if(TryMove(playerX + Direction.ValueX(dir), 
			        playerY + Direction.ValueY(dir), dir, MoveMode.Cooldown)){
				return 0;
			}
			else{
				return 1;
			}
		}
		else{
			TryTurn(dir);
			return 2;
		}
	}
	
	public bool TryTurn(int direction){
		if(this.GetDirection() == direction){
			return false;
		}
		else{
			SetDirection(direction);
			TurnSuccess();
			return true;
		}
	}

	public virtual void SetDirection(int direction){
		_direction = direction;
		UpdatePosition();
	}

	public virtual int GetDirection(){
		return direction;
	}

	public Transform GetTransform(){
		return client.transform;
	}

	public bool CanMoveTo(int x, int y){
		return IsGameSpace(x, y) && map.objects[x, y] == null 
			&& CanPass(x, y);//map.tileData[x, y]);
	}

	public bool TryMove(int x, int y, int direction, MoveMode mode){
		
		if(mode.Equals(MoveMode.NoEvent) || CanMoveTo(x, y)){
			map.objects[playerX, playerY] = null;
			_playerX = x;
			_playerY = y;
			_direction = direction;
			map.objects[x, y] = this.gameObject;
			UpdatePosition();
			switch(mode){
				case MoveMode.Cooldown:
					MoveSuccess(true);
					break;
				case MoveMode.NoCooldown:
					MoveSuccess(false);
					break;
				case MoveMode.NoEvent:
					break;
			}
			return true;
		}
		return false;
	}

	protected virtual void MoveSuccess(bool ping){
		//to be overriden
	}

	protected virtual void TurnSuccess(){
		//to be overriden
	}
	
	public virtual void UpdatePosition(){
		GetTransform().rotation = Quaternion.Euler(
			new Vector3(Direction.Rotation(_direction), 270, 90));
		GetTransform().position = ConvertPosition(playerX, playerY, GetTransform().position.z);
	}
	
	public bool IsGameSpace(int x, int y){
		return !(x < 0 || x >= map.width || y < 0 || y >= map.width);
	}

	// Use this for initialization
	protected virtual void Start(){

	}

	public virtual void Init(){
		server = GetComponent<ServerOnlyScript>();

		//MP Set
		map = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapGenerator>();

		moveCooldown = defaultMoveCooldown;
		turnCooldown = defaultTurnCooldown;

		client = server.client.GetComponent<NetworkMovement>().mover;

		init = true;
		//targetTransform = client.transform;
	}
	
	// Update is called once per frame
	protected virtual void Update(){
	
	}

	public bool CanPass(int x, int y){//GameObject obj){
		//GridController gc = obj.GetComponent<GridController>();
		if(map.TerrainType(x, y).Equals("rock")){
			return false;
		}
		return true;
	}
	
	public Vector3 ConvertPosition(int x, int y, float z){
		return new Vector3(x * map.gridSize, y * map.gridSize, z);
	}

	public Vector3 UnconvertPosition(float x, float y, float z){
		return new Vector3(Mathf.FloorToInt(x / map.gridSize + 0.5f), Mathf.FloorToInt(y * map.gridSize + 0.5f), z);
	}

	public enum MoveMode{
		Cooldown,
		NoCooldown,
		NoEvent
	}
}
