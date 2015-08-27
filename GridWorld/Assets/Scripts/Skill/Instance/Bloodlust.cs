using UnityEngine;
using System.Collections.Generic;

public class Bloodlust : Skill{
	
	float cd;
	
	public Bloodlust(float cd, float manaCost) 
	: base(cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(EntityController controller){
		return new BloodlustEvent(controller, cd);
	}

	public static Bloodlust Default(){
		return new Bloodlust(0.5f, 30);
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

	public override SkillAnimation GetAnimation(int x, int y, int direction, float length){
		return new EmptySkillAnimation();
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

		public override SkillInfo GetInfo(){
			return SkillInfo.Bloodlust;
		}
	}
}
