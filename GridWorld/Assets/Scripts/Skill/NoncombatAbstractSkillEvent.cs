using UnityEngine;
using System.Collections.Generic;

public abstract class NoncombatAbstractSkillEvent : AbstractSkillEvent {

	public override void CleanUp(){

	}
	
	protected override HashSet<KeyValuePair<int, int>> GetCoordinates (){
		HashSet<KeyValuePair<int,int>> set = new HashSet<KeyValuePair<int, int>> ();
		return set;
	}
	
	protected override void RunAttack(KeyValuePair<int, int> coords){
		//Do nothing
	}
	
	protected override void Hit(EntityController control){
		//Do nothing
	}

	protected override bool ShouldCancel(HashSet<KeyValuePair<int, int>> casts){
		return false;
	}
}
