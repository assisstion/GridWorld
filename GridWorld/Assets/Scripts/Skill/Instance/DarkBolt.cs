using UnityEngine;
using System.Collections.Generic;

public class DarkBolt : Skill{
	
	float cd;
	
	public DarkBolt(EntityController control, float cd, float manaCost) 
	: base(control, cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(){
		return new DarkBoltEvent(controller, cd);
	}

	public static DarkBolt Default(EntityController control){
		return new DarkBolt(control, 0.5f, 10);
	}

	public override SkillInfo GetID(){
		return SkillInfo.DarkBolt;
	}

	public override string GetCustomStat(){
		return "Range: " + 5;
	}

	public override string GetBody(){
		return "Using health, cast a dark bolt that deals damage to an enemy";
	}

	public override CostType GetCostType(){
		return CostType.Health;
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Magic Bolt");
		return hs;
	}

	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(2);
	}

	public class DarkBoltEvent : ProjectileSkillEvent{

		public DarkBoltEvent(EntityController control, float cd){
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
			obj.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
			obj.transform.position = controller.movement.ConvertPosition(
				controller.movement.playerX, controller.movement.playerY, -2.0f);
			obj.transform.rotation = Quaternion.Euler(new Vector3(Direction.Rotation(direction), 270, 90));
			obj.transform.localScale = new Vector3(0.05f, 1, 0.05f);
			return new DarkBoltProjectile(controller, obj, 0.5f, 0.5f);
		}
	}

	public class DarkBoltProjectile : LineProjectile{

		float damage = 10;
		static float speedDefault = 10f;
		static float range = 5;
		
		public DarkBoltProjectile(EntityController owner, GameObject obj, float w, float h)
		: base(owner, range * owner.movement.map.gridSize, speedDefault, obj, w, h){
			clipping = true;
		}

		public override bool Hit(EntityController controller){
			controller.combat.TakeDamage(controller.combat, damage);

			return true;
		}


	}
}
