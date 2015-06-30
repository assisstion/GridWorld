using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EntityMovement : NetworkBehaviour {

	//[SyncVar]
	public MapGenerator map;

	[SyncVar]
	int __direction;

	public void SetDirection(int dir){
		__direction = dir;
		transform.rotation = Quaternion.Euler(
			new Vector3(Direction.Rotation(__direction), 270, 90));
	}

	public int GetDirection(){
		return __direction;
	}

	[SyncVar]
	public int playerX;
	[SyncVar]
	public int playerY;

	[SyncVar]
	protected float moveCooldown = 0.5f;
	[SyncVar]
	protected float turnCooldown = 0.15f;

	[SyncVar]
	protected float speed;

	public virtual void Setup(int x, int y, int dir){
		TryMove (x, y, dir, MoveMode.NoEvent);
	}

	//0 = move success
	//1 = move fail, no turn
	//2 = turn success
	public int GoTowards(int dir){
		if(GetDirection() == dir){
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
		if (GetDirection() == direction) {
			return false;
		} else {
			SetDirection(direction);
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
			playerX = x;
			playerY = y;
			SetDirection(direction);
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
