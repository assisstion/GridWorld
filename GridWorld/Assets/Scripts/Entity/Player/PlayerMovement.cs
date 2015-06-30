using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovement : EntityMovement {

	[SyncVar]
	bool started;

	//[SyncVar]
	PlayerController controller;

	//[SyncVar]
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
			//Initialize();
			return;
		}
		//if (!isLocalPlayer) {
		//	return;
		//}
		InputCheck ();
	}

	[ClientCallback]
	public void InputCheck(){
		if (!isLocalPlayer) {
			return;
		}
		if(controller.combat.TryLockAction()){
			if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
				CmdMoveAction(Direction.up);
			}
			else if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				CmdMoveAction(Direction.left);
			}
			else if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
				CmdMoveAction(Direction.down);
			}
			else if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				CmdMoveAction(Direction.right);
			}
			controller.combat.UnlockAction();
		}
	}

	[Command]
	public void CmdMoveAction(int direction){
		GoTowards(direction);
	}

	public override void Setup(int x, int y, int dir){
		base.Setup (x, y, dir);
		UpdateCam ();
	}

	void UpdateCam(){
		cam.UpdateLocation (transform.position.x, transform.position.y, GetDirection());
	}

	protected override void MoveSuccess (bool ping)
	{
		//Overriden method currently empty
		base.MoveSuccess (ping);
		UpdateCam ();
		GridController gc = map.tiles[playerX,playerY].GetComponent<GridController> ();
		if (gc.terrainType.Equals ("swamp")) {
			controller.combat.TakeDamage(10);
			if(ping){
				controller.combat.action = moveCooldown * 4;
			}
		}
		else{
			if(ping){
				controller.combat.action = moveCooldown;
			}
		}
	}

	protected override void TurnSuccess(){
		//Overriden method currently empty
		base.TurnSuccess ();
		UpdateCam ();
		GridController gc = map.tiles[playerX,playerY].GetComponent<GridController> ();
		if (gc.terrainType.Equals ("swamp")) {
			controller.combat.action = turnCooldown * 4;
		}
		else{
			controller.combat.action = turnCooldown;
		}
	}
	
}
