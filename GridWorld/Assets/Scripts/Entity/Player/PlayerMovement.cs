using UnityEngine;
using System.Collections;

public class PlayerMovement : EntityMovement {

	bool started;
	
	PlayerController controller;

	public CameraController cam;

	public PlayerMovement(){

	}

	// Use this for initialization
	protected override void Start () {
		controller = this.gameObject.GetComponent<PlayerController> ();
	}

	public void Initialize(){
		if (started) {
			return;
		}
		started = true;
		Setup (0, 0, Direction.right);
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (!started) {
			Initialize();
		}
		if(controller.combat.TryLockAction()){
			if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
				GoTowards(Direction.up);
			}
			else if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				GoTowards(Direction.left);
			}
			else if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
				GoTowards(Direction.down);
			}
			else if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				GoTowards(Direction.right);
			}
			controller.combat.UnlockAction();
		}
	}

	protected override void MoveSuccess ()
	{
		//Overriden method currently empty
		base.MoveSuccess ();
		cam.UpdateLocation (transform.position.x, transform.position.y);
		GridController gc = map.tiles[playerX,playerY].GetComponent<GridController> ();
		if (gc.terrainType.Equals ("swamp")) {
			controller.combat.TakeDamage(10);
			controller.combat.action = moveCooldown * 4;
		}
		else{
			controller.combat.action = moveCooldown;
		}
	}

	protected override void TurnSuccess(){
		//Overriden method currently empty
		base.TurnSuccess ();
		GridController gc = map.tiles[playerX,playerY].GetComponent<GridController> ();
		if (gc.terrainType.Equals ("swamp")) {
			controller.combat.action = turnCooldown * 4;
		}
		else{
			controller.combat.action = turnCooldown;
		}
	}
	
}
