using UnityEngine;
using System.Collections;

public class EntityMovement : MonoBehaviour {

	public MapGenerator map;

	public int direction{
		get{
			return _direction;
		}
	}
	protected int _direction{
		get{
			return __direction;
		}
		set{
			__direction = value;
			transform.rotation = Quaternion.Euler(
				new Vector3(Direction.Rotation(__direction), 270, 90));
		}
	}
	int __direction;

	public int playerX {
		get{
			return _playerX;
		}
	}
	protected int _playerX;
	public int playerY {
		get{
			return _playerY;
		}
	}
	protected int _playerY;
	
	protected float moveCooldown = 0.5f;
	protected float turnCooldown = 0.15f;

	protected float speed;

	protected void Setup(int x, int y, int dir){
		_playerX = x;
		_playerY = y;
		_direction = dir;
	}

	//0 = move success
	//1 = move fail, no turn
	//2 = turn success
	protected int GoTowards(int dir){
		if(direction == dir){
			if(TryMove(playerX + Direction.ValueX(dir), 
			        playerY + Direction.ValueY(dir), dir)){
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
	
	protected void TryTurn(int direction){
		_direction = direction;
		TurnSuccess ();
	}

	public bool CanMoveTo(int x, int y){
		return IsGameSpace (x, y) && map.objects [x, y] == null 
			&& CanPass (map.tiles [x, y]);
	}

	protected bool TryMove(int x, int y, int direction) {
		
		if (CanMoveTo(x,y)) {
			map.objects[playerX,playerY] = null;
			_playerX = x;
			_playerY = y;
			_direction = direction;
			map.objects[x,y] = this.gameObject;
			UpdatePosition ();
			MoveSuccess();
			return true;
		}
		return false;
	}

	protected virtual void MoveSuccess(){
		//to be overriden
	}

	protected virtual void TurnSuccess(){
		//to be overriden
	}

	
	void UpdatePosition(){
		transform.position = ConvertPosition (playerX, playerY, transform.position.z);
	}
	
	public bool IsGameSpace(int x, int y){
		return !(x < 0 || x >= map.width || y < 0 || y >= map.width);
	}

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	public bool CanPass(GameObject obj){
		GridController gc = obj.GetComponent<GridController> ();
		if (gc.terrainType.Equals ("rock")) {
			return false;
		}
		return true;
	}
	
	public Vector3 ConvertPosition(int x, int y, float z){
		return new Vector3 (x * map.gridSize, y * map.gridSize, z);
	}
}
