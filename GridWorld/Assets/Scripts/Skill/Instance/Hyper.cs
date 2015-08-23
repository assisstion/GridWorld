using UnityEngine;
using System.Collections.Generic;

public class Hyper : Skill{
	
	float cd;
	
	public Hyper(float cd, float manaCost) 
	: base(cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(EntityController controller){
		return new HyperEvent(controller, cd);
	}

	public static Hyper Default(){
		return new Hyper(0.5f, 50);
	}

	public override SkillInfo GetID(){
		return SkillInfo.Hyper;
	}

	public override string GetCustomStat(){
		return "Duration: " + 3 + " s";
	}

	public override string GetBody(){
		return "Doubles movement speed";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Dash");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(3);
	}
	
	public class HyperEvent : NoncombatAbstractSkillEvent{
		
		public HyperEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		protected override bool PostCast(){
			controller.combat.AddEffect("hyper", 3.0f);
			return true;
		}

	}
}
