using UnityEngine;
using System.Collections.Generic;

public class MinorHeal : Skill {

	int healAmt = 10;
	float cd;
	
	public MinorHeal(EntityController control, float cd, float manaCost) 
	: base(control, "Minor Heal", cd, manaCost){
		this.cd = cd;
	} 
	
	public override SkillEvent GetSkillEvent(){
		return new Heal.HealEvent (controller, cd, healAmt);
	}

	public static MinorHeal Default(EntityController control){
		return new MinorHeal (control, 0.2f, 10);
	}

	public override SkillDB GetID (){
		return SkillDB.MinorHeal;
	}

	public override string GetCustomStat(){
		return "Heal: " + healAmt;
	}

	public override string GetBody(){
		return "Restores some health to the caster";
	}

	public override HashSet<string> GetPrerequisites ()
	{
		HashSet<string> hs = new HashSet<string> ();
		return hs;
	}
	
	public override int GetMinimumWave ()
	{
		return Skill.MinimumWaveFromTier (2);
	}
}
