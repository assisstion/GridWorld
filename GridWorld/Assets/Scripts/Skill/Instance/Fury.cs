using UnityEngine;
using System.Collections.Generic;

public class Fury : Skill{
	
	float cd;
	
	public Fury(EntityController control, float cd, float manaCost) 
	: base(control, cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(){
		return new FuryEvent(controller, cd);
	}

	public static Fury Default(EntityController control){
		return new Fury(control, 0.5f, 50);
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
		return Skills.MinimumWaveFromTier(3);
	}
	
	public class FuryEvent : NoncombatAbstractSkillEvent{
		
		public FuryEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}
		
		public override bool Update(){
			return false;
		}
		
		protected override bool ShouldCancel(HashSet<KeyValuePair<int, int>> casts){
			return false;
		}

		protected override bool PostCast(){
			controller.combat.AddEffect("fury", 3.0f);
			return true;
		}

	}
}
