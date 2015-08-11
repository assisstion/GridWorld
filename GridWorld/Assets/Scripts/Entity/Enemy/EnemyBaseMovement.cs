using UnityEngine;
using System.Collections;

public class EnemyBaseMovement : EntityMovement{

	protected EnemyBaseController controller{
		get{
			return GetController();
		}
	}

	EnemyBaseController _controller;

	protected void SetupController(EnemyBaseController controller){
		_controller = controller;
	}

	protected virtual EnemyBaseController GetController(){
		return _controller;
	}

	// Use this for initialization
	protected override void Start(){
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update(){
		base.Update();
	}
	
	protected override void MoveSuccess(bool ping){
		base.MoveSuccess(ping);
		if(ping){
			controller.combat.action = moveCooldown * MoveMultiplier();
		}
	}
	
	protected override void TurnSuccess(){
		controller.combat.action = turnCooldown * TurnMultiplier();
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
