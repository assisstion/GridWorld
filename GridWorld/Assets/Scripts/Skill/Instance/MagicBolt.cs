using UnityEngine;
using System.Collections.Generic;

public class MagicBolt : Skill{
	
	float cd;
	
	public MagicBolt(EntityController control, float cd, float manaCost) 
	: base(control, cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(){
		return new MagicBoltEvent(controller, cd);
	}

	public static MagicBolt Default(EntityController control){
		return new MagicBolt(control, 0.5f, 20);
	}

	public override SkillInfo GetID(){
		return SkillInfo.MagicBolt;
	}

	public override string GetCustomStat(){
		return "Range: " + 3;
	}

	public override string GetBody(){
		return "Cast a magical bolt that deals damage to an enemy";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		//hs.Add ("Flare");
		return hs;
	}

	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(1);
	}

	public class MagicBoltEvent : ProjectileSkillEvent{

		public MagicBoltEvent(EntityController control, float cd){
			controller = control;
			cooldown = cd;
		}

		protected override HashSet<float> GetAngles(){
			HashSet<float> set = new HashSet<float>();
			set.Add(0);
			return set;
		}

		protected override Projectile GetNewProjectile(){
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			obj.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 1);
			obj.transform.position = controller.movement.ConvertPosition(
				controller.movement.playerX, controller.movement.playerY, -2.0f);
			obj.transform.rotation = Quaternion.Euler(new Vector3(Direction.Rotation(direction), 270, 90));
			obj.transform.localScale = new Vector3(0.05f, 1, 0.05f);
			return new MagicBoltProjectile(controller, obj, 0.5f, 0.5f);
		}
	}

	public class MagicBoltProjectile : LineProjectile{

		float damage = 10;
		static float speedDefault = 7.5f;
		static float range = 3;

		public MagicBoltProjectile(EntityController owner, GameObject obj, float w, float h)
		: base(owner, range * owner.movement.map.gridSize, speedDefault, obj, w, h){
			clipping = true;
		}

		public override bool Hit(EntityController controller){
			controller.combat.TakeDamage(controller.combat, damage);

			return true;
		}


	}
}
