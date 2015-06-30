using UnityEngine;
using System.Collections.Generic;

public class Heal : Skill {
	
	float cd;
	
	public Heal(EntityController control, float cd, float manaCost) 
	: base(control, "Heal", cd, manaCost){
		this.cd = cd;
	} 
	
	public override SkillEvent GetSkillEvent(){
		return new HealEvent (controller, cd);
	}

	public static Heal Default(EntityController control){
		return new Heal (control, 0.1f, 30);
	}

	public override int GetID (){
		return 3;
	}

	public override string GetCustomStat(){
		return "Heal: " + 10;
	}

	public override string GetBody(){
		return "Restores health to the caster";
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
