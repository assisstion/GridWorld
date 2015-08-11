using UnityEngine;
using System.Collections;

public class PlayerController : EntityController{
	
	public const bool DEBUG = true;

	public new PlayerMovement movement{
		get{
			return __movement;
		}
	}

	PlayerMovement __movement;

	public new PlayerCombat combat{
		get{
			return __combat;
		}
	}

	PlayerCombat __combat;

	// Use this for initialization
	protected override void Start(){
		__movement = this.gameObject.GetComponent<PlayerMovement>();
		__combat = this.gameObject.GetComponent<PlayerCombat>();
	}
	
	// Update is called once per frame
	protected override void Update(){
	
	}

	protected override EntityMovement MovementHolder(){
		return __movement;
	}
	
	protected override EntityCombat CombatHolder(){
		return __combat;
	}
}
