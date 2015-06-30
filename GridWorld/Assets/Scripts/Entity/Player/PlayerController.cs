using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : EntityController {

	//[SyncVar]
	public new PlayerMovement movement;

	//[SyncVar]
	public new PlayerCombat combat;

	// Use this for initialization
	protected override void Start () {

	}

	public void Initialize(MapGenerator map){
		movement = this.gameObject.GetComponent<PlayerMovement> ();
		movement.map = map;
		movement.Initialize ();
		combat = this.gameObject.GetComponent<PlayerCombat> ();
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

	protected override EntityMovement MovementHolder(){
		return movement;
	}
	
	protected override EntityCombat CombatHolder(){
		return combat;
	}
}
