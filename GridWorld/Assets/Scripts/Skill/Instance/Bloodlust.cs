using UnityEngine;
using System.Collections.Generic;

public class Bloodlust : Skill{
	
	float cd;
	
	public Bloodlust(EntityController control, float cd, float manaCost) 
	: base(control, cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(){
		return new BloodlustEvent(controller, cd);
	}

	public static Bloodlust Default(EntityController control){
		return new Bloodlust(control, 0.5f, 30);
	}

	public override CostType GetCostType(){
		return CostType.Health;
	}

	public override SkillInfo GetID(){
		return SkillInfo.Bloodlust;
	}

	public override string GetCustomStat(){
		return "Duration: " + 3 + " s";
	}

	public override string GetBody(){
		return "Kills cause caster to gain 30 health";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Vamp Strike");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(4);
	}
	
	public class BloodlustEvent : NoncombatAbstractSkillEvent{
		
		public BloodlustEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		protected override bool PostCast(){
			controller.combat.AddEffect("bloodlust", 3.0f);
			return true;
		}

	}
}
