using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EntityController : NetworkBehaviour{

	public EntityController(){

	}

	public EntityController(EntityMovement movement, EntityCombat combat){
		_movement = movement;
		_combat = combat;
	}

	public virtual EntityMovement movement{
		get{
			return MovementHolder();
		}
	}

	EntityMovement _movement;

	public virtual EntityCombat combat{
		get{
			return CombatHolder();
		}
	}

	EntityCombat _combat;

	protected virtual EntityMovement MovementHolder(){
		return _movement;
	}

	protected virtual EntityCombat CombatHolder(){
		return _combat;
	}

	protected virtual void Start(){

	}
	
	// Update is called once per frame
	protected virtual void Update(){
		
	}
}
