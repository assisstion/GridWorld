using UnityEngine;
using System.Collections.Generic;

public class Flare : Skill{
	
	float cd;
	
	public Flare(EntityController control, float cd, float manaCost) 
	: base(control, cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(){
		return new FlareEvent(controller, cd);
	}

	public static Flare Default(EntityController control){
		return new Flare(control, 0.35f, 10);
	}

	public override SkillInfo GetID(){
		return SkillInfo.Flare;
	}

	public override string GetCustomStat(){
		return "Range: " + 2;
	}

	public override string GetBody(){
		return "Cast a burst of fire that deals damage to an enemy";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		//hs.Add ("Flare");
		return hs;
	}

	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(1);
	}

	public class FlareEvent : AbstractSkillEvent{

		public Dictionary<KeyValuePair<int, int>, GameObject> anim;
		
		public FlareEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
			anim = new Dictionary<KeyValuePair<int, int>, GameObject>();
		}
		
		public override bool Update(){
			foreach(KeyValuePair<KeyValuePair<int, int>, GameObject> animPair in anim){
				KeyValuePair<int, int> pair = LocalToGame(animPair.Key);
				int vx = pair.Key;
				int vy = pair.Value;
				GameObject animX = animPair.Value;
				animX.transform.position = controller.movement.ConvertPosition(vx, vy, -2.0f);
				animX.transform.localScale = new Vector3 
					(0.05f * (1 - TimePassed() / cooldown), animX.transform.localScale.y, 0.05f * (1 - TimePassed() / cooldown));

			}
			if(TimePassed() > cooldown){
				return false;
			}
			return true;
		}
		
		public override void CleanUp(){
			foreach(KeyValuePair<KeyValuePair<int, int>, GameObject> animPair in anim){
				GameObject.Destroy(animPair.Value);
			}
		}
		
		protected override HashSet<KeyValuePair<int, int>> GetCoordinates(){
			HashSet<KeyValuePair<int,int>> set = new HashSet<KeyValuePair<int, int>>();
			set.Add(new KeyValuePair<int, int>(0, 2));
			return set;
		}
		
		protected override void RunAttack(KeyValuePair<int, int> coords){
			KeyValuePair<int, int> pair = LocalToGame(coords);
			GameObject animObj;
			animObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			animObj.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
			animObj.transform.position = controller.movement.ConvertPosition(pair.Key, pair.Value, -2.0f);
			animObj.transform.rotation = Quaternion.Euler(new Vector3(Direction.Rotation(direction), 270, 90));
			animObj.transform.localScale = new Vector3(0.05f, 1, 0.05f);
			anim.Add(coords, animObj);
		}
		
		protected override void Hit(EntityController control){
			control.combat.TakeDamage(controller.combat, 10);
		}

		/*protected override bool ShouldCancel(HashSet<KeyValuePair<int, int>> casts){
			if (base.ShouldCancel (casts)) {
				return true;
			}
			return !(casts.Contains(new KeyValuePair<int, int>(centerX, centerY)));
		}*/
	}
}
