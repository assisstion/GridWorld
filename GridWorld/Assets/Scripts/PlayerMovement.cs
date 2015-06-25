using UnityEngine;
using System.Collections;

public class PlayerMovement : EntityMovement {
	
	PlayerController controller;

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

	protected override void MoveSuccess ()
	{
		//Overriden method currently empty
		base.MoveSuccess ();
		GridController gc = map.tiles[playerX,playerY].GetComponent<GridController> ();
		if (gc.terrainType.Equals ("swamp")) {
			controller.combat.action = moveCooldown * 4;
			controller.combat.TakeDamage(10);
		}
		else{
			controller.combat.action = moveCooldown;
		}
	}
	
}
