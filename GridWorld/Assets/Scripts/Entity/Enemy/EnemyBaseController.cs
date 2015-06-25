using UnityEngine;
using System.Collections;

public class EnemyBaseController : EntityController {

	public new EnemyBaseMovement movement{
		get{
			return __movement;
		}
	}
	protected EnemyBaseMovement __movement;
	
	public new EnemyBaseCombat combat{
		get{
			return __combat;
		}
	}
	protected EnemyBaseCombat __combat;

	public EnemyBaseController(){

	}

	public EnemyBaseController(EnemyBaseMovement movement,
	                           EnemyBaseCombat combat){
		__movement = movement;
		__combat = combat;
	}
	
	// Use this for initialization
	protected override void Start () {

	}
	
	// Update is called once per frame
	protected override void Update () {
		
	}
	
	protected override EntityMovement MovementHolder(){
		return __movement;
	}
	
	protected override EntityCombat CombatHolder(){
		return __combat;
	}
}
