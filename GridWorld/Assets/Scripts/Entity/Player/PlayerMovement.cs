using UnityEngine;
using System.Collections;

public class PlayerMovement : EntityMovement{

	bool started;
	
	PlayerController controller;

	CameraController cam;

	public PlayerMovement(){

	}

	// Use this for initialization
	protected override void Start(){
		//
	}

	public override void Init(){
		base.Init();

		//cam = server.client.GetComponentInChildren<CameraController>();
		
		defaultMoveCooldown = 0.3f;
		if(PlayerController.DEBUG){
			defaultMoveCooldown = 0.1f;
			defaultTurnCooldown = 0.05f;
		}
		moveCooldown = defaultMoveCooldown;
		turnCooldown = defaultTurnCooldown;
		controller = this.gameObject.GetComponent<PlayerController>();

		//NetworkPlayerController netPlayer = server.client.GetComponent<NetworkPlayerController>();
		//netPlayer.movement.RpcSetGridsize(map.gridSize);

		

		//GameObject.FindGameObjectWithTag("CGameController")
		//	.GetComponent<ClientMapController>().Generate(netPlayer);
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
		if(init){
			if(!started){
				Initialize();
			}
		}
	}

	protected override void MoveSuccess(bool ping){
		//Overriden method currently empty
		base.MoveSuccess(ping);
		//NetworkPlayerController netPlayer = server.client.GetComponent<NetworkPlayerController>();
		//netPlayer.RpcUpdateCamera();
		//GridController gc = map.tiles[playerX, playerY].GetComponent<GridController>();
		if(map.TerrainType(playerX, playerY).Equals("swamp")){
			controller.combat.TakeDamage(map, 10);
			if(ping){
				controller.combat.SetAction(moveCooldown * 4 * MoveMultiplier());
			}
		}
		else{
			if(ping){
				controller.combat.SetAction(moveCooldown * MoveMultiplier());
			}
		}
	}

	protected override void TurnSuccess(){
		//Overriden method currently empty
		base.TurnSuccess();
		//GridController gc = map.tiles[playerX, playerY].GetComponent<GridController>();
		if(map.TerrainType(playerX, playerY).Equals("swamp")){
			controller.combat.SetAction(turnCooldown * 4 * TurnMultiplier());
		}
		else{
			controller.combat.SetAction(turnCooldown * TurnMultiplier());
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

	public override void UpdatePosition(){
		//no base
		NetworkPlayerController netPlayer = server.client.GetComponent<NetworkPlayerController>();
		if(netPlayer.movement.gridSize != map.gridSize){
			netPlayer.movement.gridSize = map.gridSize;
		}
		netPlayer.movement.x = playerX;
		netPlayer.movement.y = playerY;
		netPlayer.movement.direction = GetDirection();
		//Update loop catches syncvars
		//netPlayer.RpcUpdatePosition();
	}
}
