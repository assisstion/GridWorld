using UnityEngine;
using System.Collections.Generic;

public class Hyper : Skill {
	
	float cd;
	
	public Hyper(EntityController control, float cd, float manaCost) 
	: base(control, "Hyper", cd, manaCost){
		this.cd = cd;
	} 
	
	public override SkillEvent GetSkillEvent(){
		return new HyperEvent (controller, cd);
	}

	public static Hyper Default(EntityController control){
		return new Hyper (control, 0.5f, 75);
	}

	public override int GetID (){
		return 4;
	}

	public override string GetCustomStat(){
		return "Duration: " + 5 + " s";
	}

	public override string GetBody(){
		return "Reduces movement cooldown by half";
	}
	
	public class HyperEvent : NoncombatAbstractSkillEvent{
		
		public HyperEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}
		
		public override bool Update(){
			if (TimePassed() > 5) {
				controller.movement.moveCooldown = controller.movement.defaultMoveCooldown;
				controller.movement.turnCooldown = controller.movement.defaultTurnCooldown;
				return false;
			}
			return true;
		}
		
		protected override bool ShouldCancel(HashSet<KeyValuePair<int, int>> casts){
			return false;
		}

		protected override bool PostCast(){
			controller.movement.moveCooldown = controller.movement.defaultMoveCooldown / 2;
			controller.movement.turnCooldown = controller.movement.defaultTurnCooldown / 3;
			return true;
		}

	}
}
