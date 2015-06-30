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

	public float defaultMoveCooldown = 0.5f;
	public float defaultTurnCooldown = 0.15f;
	
	public float moveCooldown;
	public float turnCooldown;

	protected float speed;

	public void Setup(int x, int y, int dir){
		TryMove (x, y, dir, MoveMode.NoEvent);
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
		if (this.direction == direction) {
			return false;
		} else {
			_direction = direction;
			TurnSuccess ();
			return true;
		}
	}

	public bool CanMoveTo(int x, int y){
		return IsGameSpace (x, y) && map.objects [x, y] == null 
			&& CanPass (map.tiles [x, y]);
	}

	public bool TryMove(int x, int y, int direction, MoveMode mode) {
		
		if (mode.Equals(MoveMode.NoEvent) || CanMoveTo(x,y)) {
			map.objects[playerX,playerY] = null;
			_playerX = x;
			_playerY = y;
			_direction = direction;
			map.objects[x,y] = this.gameObject;
			UpdatePosition ();
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

	
	public void UpdatePosition(){
		transform.position = ConvertPosition (playerX, playerY, transform.position.z);
	}
	
	public bool IsGameSpace(int x, int y){
		return !(x < 0 || x >= map.width || y < 0 || y >= map.width);
	}

	// Use this for initialization
	protected virtual void Start () {
		moveCooldown = defaultMoveCooldown;
		turnCooldown = defaultTurnCooldown;
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

	public enum MoveMode{
		Cooldown,NoCooldown,NoEvent
	}
}
