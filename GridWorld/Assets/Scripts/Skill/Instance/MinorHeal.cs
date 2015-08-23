using UnityEngine;
using System.Collections.Generic;

public class MinorHeal : Skill{

	int healAmt = 10;
	float cd;
	
	public MinorHeal(float cd, float manaCost) 
	: base(cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(EntityController controller){
		return new Heal.HealEvent(controller, cd, healAmt);
	}

	public static MinorHeal Default(){
		return new MinorHeal(0.2f, 10);
	}

	public override SkillInfo GetID(){
		return SkillInfo.MinorHeal;
	}

	public override string GetCustomStat(){
		return "Heal: " + healAmt;
	}

	public override string GetBody(){
		return "Restores some health to the caster";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(2);
	}
}
