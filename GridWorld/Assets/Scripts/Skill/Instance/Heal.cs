using UnityEngine;
using System.Collections.Generic;

public class Heal : Skill{

	int healAmt = 20;
	float cd;
	
	public Heal(float cd, float manaCost) 
	: base(cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(EntityController controller){
		return new HealEvent(controller, cd, healAmt);
	}

	public static Heal Default(){
		return new Heal(0.2f, 30);
	}

	public override SkillInfo GetID(){
		return SkillInfo.Heal;
	}

	public override string GetCustomStat(){
		return "Heal: " + healAmt;
	}

	public override string GetBody(){
		return "Restores health to the caster";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Minor Heal");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(3);
	}
	
	public class HealEvent : NoncombatAbstractSkillEvent{

		int amt;
		
		public HealEvent(EntityController cont, float cd, int amt){
			controller = cont;
			cooldown = cd;
			this.amt = amt;
		}
		
		public override bool Update(){
			if(TimePassed() > cooldown){
				return false;
			}
			return true;
		}
		
		protected override bool ShouldCancel(HashSet<KeyValuePair<int, int>> casts){
			return controller.combat.GetHealth() == controller.combat.GetMaxHealth();
		}

		protected override bool PostCast(){
			controller.combat.HealHealth(amt);
			return true;
		}

	}
}
