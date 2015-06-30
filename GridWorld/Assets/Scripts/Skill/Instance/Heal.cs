using UnityEngine;
using System.Collections.Generic;

public class Heal : Skill {
	
	float cd;
	
	public Heal(EntityController control, float cd, float manaCost) 
	: base(control, "heal", cd, manaCost){
		this.cd = cd;
	} 
	
	public override SkillEvent GetSkillEvent(){
		return new HealEvent (controller, cd);
	}

	public static Heal Default(EntityController control){
		return new Heal (control, 1.0f, 20);
	}
	
	public class HealEvent : NoncombatAbstractSkillEvent{
		
		public HealEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}
		
		public override bool Update(){
			if (TimePassed() > cooldown) {
				return false;
			}
			return true;
		}
		
		protected override bool ShouldCancel(HashSet<KeyValuePair<int, int>> casts){
			return controller.combat.health == controller.combat.maxHealth;
		}

		protected override bool PostCast(){
			controller.combat.HealHealth (10);
			return true;
		}

	}
}
