using UnityEngine;
using System.Collections.Generic;

public class VampStrike : Skill{
	
	static float healAmt = 30;

	float cd;

	public VampStrike(float cd, float manaCost) 
			: base(cd, manaCost){
		this.cd = cd;
	}

	public override SkillEvent GetSkillEvent(EntityController controller){
		return new VampStrikeSkillEvent(controller, cd);
	}

	public static VampStrike Default(){
		return new VampStrike(0.5f, 20);
	}

	public override SkillInfo GetID(){
		return SkillInfo.VampStrike;
	}

	public override string GetCustomStat(){
		return "Life steal: " + healAmt;
	}

	public override string GetBody(){
		return "Using health, attacks an enemy in front with life steal";
	}

	public override CostType GetCostType(){
		return CostType.Health;
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Dark Bolt");
		hs.Add("Minor Heal");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(3);
	}

	public class VampStrikeSkillEvent : AbstractSkillEvent{

		
		protected GameObject animObj;

		public VampStrikeSkillEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		public override bool Update(){
			KeyValuePair<int, int> pair = LocalToGame(new KeyValuePair<int, int>(0, 1));
			int vx = pair.Key;
			int vy = pair.Value;
			animObj.transform.position = controller.movement.ConvertPosition(vx, vy, -2.0f) 
				- Direction.ToVector(direction).normalized
				* controller.movement.map.gridSize * (TimePassed() / cooldown) / 2;
			animObj.transform.localScale = new Vector3 
				(0.1f * (1 - TimePassed() / cooldown), animObj.transform.localScale.y, animObj.transform.localScale.z);
			if(TimePassed() > cooldown){
				return false;
			}
			return true;
		}

		public override void CleanUp(){
			GameObject.Destroy(animObj);
		}

		protected override HashSet<KeyValuePair<int, int>> GetCoordinates(){
			HashSet<KeyValuePair<int,int>> set = new HashSet<KeyValuePair<int, int>>();
			set.Add(new KeyValuePair<int, int>(0, 1));
			return set;
		}

		protected override void RunAttack(KeyValuePair<int, int> coords){
			KeyValuePair<int, int> pair = LocalToGame(coords);
			animObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			animObj.transform.position = controller.movement.ConvertPosition(pair.Key, pair.Value, -2.0f);
			animObj.transform.rotation = Quaternion.Euler(new Vector3(Direction.Rotation(direction), 270, 90));
			animObj.transform.localScale = new Vector3(0.1f, 1, 0.02f);

		}

		protected override void Hit(EntityController control){
			control.combat.TakeDamage(controller.combat, 10);
			controller.combat.delayedHealthMod += healAmt;
			//controller.combat.HealHealth(healAmt);
		}
	}
}
