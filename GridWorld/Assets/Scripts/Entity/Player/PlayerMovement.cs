using UnityEngine;
using System.Collections;

public class PlayerMovement : EntityMovement{

	bool started;
	
	PlayerController controller;

	public CameraController cam;

	public PlayerMovement(){

	}

	// Use this for initialization
	protected override void Start(){
		base.Start();
		defaultMoveCooldown = 0.3f;
		if(PlayerController.DEBUG){
			defaultMoveCooldown = 0.1f;
			defaultTurnCooldown = 0.05f;
		}
		moveCooldown = defaultMoveCooldown;
		turnCooldown = defaultTurnCooldown;
		controller = this.gameObject.GetComponent<PlayerController>();
	}

	public void Initialize(){
		if(started){
			return;
		}
		started = true;
		Setup(0, 0, Direction.right);
	}
	
	// Update is called once per frame
	protected override void Update(){
		if(!started){
			Initialize();
		}
		if(controller.combat.TryLockAction()){
			if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
				GoTowards(Direction.up);
			}
			else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
				GoTowards(Direction.left);
			}
			else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
				GoTowards(Direction.down);
			}
			else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
				GoTowards(Direction.right);
			}
			controller.combat.UnlockAction();
		}
	}

	protected override void MoveSuccess(bool ping){
		//Overriden method currently empty
		base.MoveSuccess(ping);
		cam.UpdateLocation(transform.position.x, transform.position.y);
		GridController gc = map.tiles[playerX, playerY].GetComponent<GridController>();
		if(gc.terrainType.Equals("swamp")){
			controller.combat.TakeDamage(map, 10);
			if(ping){
				controller.combat.action = moveCooldown * 4 * MoveMultiplier();
			}
		}
		else{
			if(ping){
				controller.combat.action = moveCooldown * MoveMultiplier();
			}
		}
	}

	protected override void TurnSuccess(){
		//Overriden method currently empty
		base.TurnSuccess();
		GridController gc = map.tiles[playerX, playerY].GetComponent<GridController>();
		if(gc.terrainType.Equals("swamp")){
			controller.combat.action = turnCooldown * 4 * TurnMultiplier();
		}
		else{
			controller.combat.action = turnCooldown * TurnMultiplier();
		}
	}

	public float MoveMultiplier(){
		float mult = 1.0f;
		if(controller.combat.effects.ContainsKey("hyper")){
			mult *= 0.5f;
		}
		return mult;
	}

	public float TurnMultiplier(){
		float mult = 1.0f;
		if(controller.combat.effects.ContainsKey("hyper")){
			mult *= 0.35f;
		}
		return mult;
	}
	
}
