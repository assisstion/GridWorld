using UnityEngine;
using System.Collections.Generic;

public class Meditate : Skill{
	
	float cd;
	
	public Meditate(float cd, float manaCost) 
	: base(cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(EntityController controller){
		return new MeditateEvent(controller, cd);
	}

	public static Meditate Default(){
		return new Meditate(3f, 0);
	}

	public override SkillInfo GetID(){
		return SkillInfo.Meditate;
	}

	public override string GetCustomStat(){
		return "Duration: " + 3 + " s";
	}

	public override string GetBody(){
		return "Greatly increases mana regeneration";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Heal");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(4);
	}

	public override SkillAnimation GetAnimation(int x, int y, int direction, float length){
		return new EmptySkillAnimation();
	}
	
	public class MeditateEvent : NoncombatAbstractSkillEvent{
		
		public MeditateEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		protected override bool PostCast(){
			controller.combat.AddEffect("meditate", 3.0f);
			return true;
		}

		public override SkillInfo GetInfo(){
			return SkillInfo.Meditate;
		}
	}
}
