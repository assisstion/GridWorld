using UnityEngine;
using System.Collections.Generic;

public class Fury : Skill{
	
	float cd;
	
	public Fury(float cd, float manaCost) 
	: base(cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(EntityController controller){
		return new FuryEvent(controller, cd);
	}

	public static Fury Default(){
		return new Fury(0.5f, 50);
	}

	public override SkillInfo GetID(){
		return SkillInfo.Fury;
	}

	public override string GetCustomStat(){
		return "Duration: " + 3 + " s";
	}

	public override string GetBody(){
		return "Kills reduce current action to 0.5";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Quake");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(4);
	}

	public override SkillAnimation GetAnimation(int x, int y, int direction, float length){
		return new EmptySkillAnimation();
	}
	
	public class FuryEvent : NoncombatAbstractSkillEvent{
		
		public FuryEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		protected override bool PostCast(){
			controller.combat.AddEffect("fury", 3.0f);
			return true;
		}

		public override SkillInfo GetInfo(){
			return SkillInfo.Fury;
		}
	}
}
