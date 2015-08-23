using UnityEngine;
using System.Collections.Generic;

public class Energize : Skill{
	
	float cd;
	
	public Energize(float cd, float manaCost) 
	: base( cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(EntityController controller){
		return new EnergizeEvent(controller, cd);
	}

	public static Energize Default(){
		return new Energize(1f, 20);
	}

	public override SkillInfo GetID(){
		return SkillInfo.Energize;
	}

	public override string GetCustomStat(){
		return "Duration: " + 3 + " s";
	}

	public override string GetBody(){
		return "The next skill activated costs no mana";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Lightning Bolt");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(4);
	}
	
	public class EnergizeEvent : NoncombatAbstractSkillEvent{
		
		public EnergizeEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		protected override bool PostCast(){
			controller.combat.AddEffect("energize", 3.0f);
			return true;
		}

	}
}
