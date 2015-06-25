using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public int direction{
		get{
			return _direction;
		}
	}
	int _direction;

	public int playerX {
		get{
			return _playerX;
		}
	}
	int _playerX;
	public int playerY {
		get{
			return _playerY;
		}
	}
	int _playerY;

	float moveCooldown = 0.5f;

	float speed;

	public MapGenerator map;

	PlayerController controller;

	// Use this for initialization
	void Start () {
		speed = map.gridSize;
		_playerX = 0;
		_playerY = 0;
		_direction = Direction.right;
		controller = this.gameObject.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(controller.combat.TryLockAction()){
			if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
				TryMove(playerX, playerY + 1, Direction.up);
			}
			else if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				TryMove(playerX - 1, playerY, Direction.left);
			}
			else if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
				TryMove(playerX, playerY - 1, Direction.down);
			}
			else if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				TryMove(playerX + 1, playerY, Direction.right);
			}
			controller.combat.UnlockAction();
		}
	}
	
	public bool IsGameSpace(int x, int y){
		return !(x < 0 || x >= map.width || y < 0 || y >= map.width);
	}

	void TryMove(int x, int y, int direction) {

		if (IsGameSpace(x, y) && map.objects[x,y] == null 
		    		&& CanPass (map.tiles [x, y])) {
			map.objects[playerX,playerY] = null;
			_playerX = x;
			_playerY = y;
			_direction = direction;
			map.objects[x,y] = this.gameObject;
			UpdatePosition ();
			GridController gc = map.tiles[x,y].GetComponent<GridController> ();
			if (gc.terrainType.Equals ("swamp")) {
				controller.combat.action = moveCooldown * 4;
				controller.combat.TakeDamage(10);
			}
			else{
				controller.combat.action = moveCooldown;
			}
		}
	}

	void UpdatePosition(){
		transform.position = ConvertPosition (playerX, playerY, transform.position.z);
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
