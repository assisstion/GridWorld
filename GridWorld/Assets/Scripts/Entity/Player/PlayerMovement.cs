using UnityEngine;
using System.Collections;

public class PlayerMovement : EntityMovement {
	
	PlayerController controller;

	public CameraController cam;

	public PlayerMovement() : base(0, 0, Direction.right){

	}

	// Use this for initialization
	protected override void Start () {
		controller = this.gameObject.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(controller.combat.TryLockAction()){
			if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
				if(direction == Direction.up){
					TryMove(playerX, playerY + 1, Direction.up);
				}
				else{
					TryTurn(Direction.up);
				}
			}
			else if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				if(direction == Direction.left){
					TryMove(playerX - 1, playerY, Direction.left);
				}
				else{
					TryTurn(Direction.left);
				}
			}
			else if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
				if(direction == Direction.down){
					TryMove(playerX, playerY - 1, Direction.down);
				}
				else{
					TryTurn(Direction.down);
				}
			}
			else if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				if(direction == Direction.right){
					TryMove(playerX + 1, playerY, Direction.right);
				}
				else{
					TryTurn(Direction.right);
				}
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
