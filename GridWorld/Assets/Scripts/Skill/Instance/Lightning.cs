using UnityEngine;
using System.Collections.Generic;

public class Lightning : Skill{

	static float actionRecovery = 0.6f;
	
	float cd;
	
	public Lightning(float cd, float manaCost) 
	: base(cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(EntityController controller){
		return new LightningEvent(controller, cd);
	}

	public static Lightning Default(){
		return new Lightning(0.75f, 10);
	}

	public override SkillInfo GetID(){
		return SkillInfo.Lightning;
	}

	public override string GetCustomStat(){
		return "Action recovery: " + actionRecovery;
	}

	public override string GetBody(){
		return "Shocks an enemy 3 tiles away, recovering action upon hitting";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Shock");
		return hs;
	}

	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(3);
	}

	public override SkillAnimation GetAnimation(int x, int y, int direction, float length){
		return new LightningAnimation(x, y, direction, length);
	}
	
	public class LightningAnimation : BoxSkillAnimation{
		
		public LightningAnimation(int x, int y, int direction, float length) : base(x, y, direction, length){
			
		}
		
		public override Color GetColor(){
			return new Color(1f, 1f, 0f);
		}

		public override HashSet<KeyValuePair<int, int>> GetCoords(){
			HashSet<KeyValuePair<int,int>> set = new HashSet<KeyValuePair<int, int>>();
			set.Add(new KeyValuePair<int, int>(0, 3));
			return set;
		}
	}

	public class LightningEvent : AbstractSkillEvent{

		//bool hit;

		//public Dictionary<KeyValuePair<int, int>, GameObject> anim;
		
		public LightningEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
			//anim = new Dictionary<KeyValuePair<int, int>, GameObject>();
		}
		
		public override bool Update(){
			/*foreach(KeyValuePair<KeyValuePair<int, int>, GameObject> animPair in anim){
				KeyValuePair<int, int> pair = LocalToGame(animPair.Key);
				int vx = pair.Key;
				int vy = pair.Value;
				GameObject animX = animPair.Value;
				animX.transform.position = controller.movement.ConvertPosition(vx, vy, -2.0f);
				animX.transform.localScale = new Vector3 
					(0.05f * (1 - TimePassed() / (cooldown - actionRecovery)), animX.transform.localScale.y, 0.05f * (1 - TimePassed() / (cooldown - actionRecovery)));

			}*/
			if(TimePassed() > cooldown - actionRecovery){
				return false;
			}
			return true;
		}
		
		public override void CleanUp(){
			/*foreach(KeyValuePair<KeyValuePair<int, int>, GameObject> animPair in anim){
				GameObject.Destroy(animPair.Value);
			}*/
		}
		
		protected override HashSet<KeyValuePair<int, int>> GetCoordinates(){
			HashSet<KeyValuePair<int,int>> set = new HashSet<KeyValuePair<int, int>>();
			set.Add(new KeyValuePair<int, int>(0, 3));
			return set;
		}
		
		protected override void RunAttack(KeyValuePair<int, int> coords){
			/*KeyValuePair<int, int> pair = LocalToGame(coords);
			GameObject animObj;
			animObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			animObj.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 0);
			animObj.transform.position = controller.movement.ConvertPosition(pair.Key, pair.Value, -2.0f);
			animObj.transform.rotation = Quaternion.Euler(new Vector3(Direction.Rotation(direction), 270, 90));
			animObj.transform.localScale = new Vector3(0.05f, 1, 0.05f);
			anim.Add(coords, animObj);*/
		}
		
		protected override void Hit(EntityController control){
			control.combat.TakeDamage(controller.combat, 10);
			controller.combat.delayedActionMod -= actionRecovery;
			//hit = true;
		}
		
		/*public override float GetActionModifier(){
			return base.GetActionModifier() + (hit ? -actionRecovery : 0);
		}*/

		/*protected override bool ShouldCancel(HashSet<KeyValuePair<int, int>> casts){
			if (base.ShouldCancel (casts)) {
				return true;
			}
			return !(casts.Contains(new KeyValuePair<int, int>(centerX, centerY)));
		}*/

		public override SkillInfo GetInfo(){
			return SkillInfo.Lightning;
		}
	}
}
