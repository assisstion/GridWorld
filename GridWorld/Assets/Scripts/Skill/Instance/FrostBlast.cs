using UnityEngine;
using System.Collections.Generic;

public class FrostBlast : Skill{

	float cd;

	public FrostBlast(EntityController control, float cd, float manaCost) 
			: base(control, cd, manaCost){
		this.cd = cd;
	}

	public override SkillEvent GetSkillEvent(){
		return new FrostBlastEvent(controller, cd);
	}

	public static FrostBlast Default(EntityController control){
		return new FrostBlast(control, 1f, 50);
	}

	public override SkillInfo GetID(){
		return SkillInfo.FrostBlast;
	}

	public override string GetCustomStat(){
		return "Range: " + 6;
	}

	public override string GetBody(){
		return "Destroy all slowed or frozen enemies";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Blizzard");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(4);
	}

	public class FrostBlastEvent : AbstractSkillEvent{
		
		public Dictionary<KeyValuePair<int, int>, GameObject> anim;
		
		public FrostBlastEvent(EntityController cont, float cd){
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
		
		int extent = 6;
		
		protected override HashSet<KeyValuePair<int, int>> GetCoordinates(){
			HashSet<KeyValuePair<int,int>> set = new HashSet<KeyValuePair<int, int>>();
			for(int x = -extent; x <= extent; x++){
				for(int y = -extent; y <= extent; y++){
					if(x == 0 && y == 0){
						continue;
					}
					set.Add(new KeyValuePair<int, int>(x, y));
				}
			}
			return set;
		}
		
		protected override void RunAttack(KeyValuePair<int, int> coords){
			KeyValuePair<int, int> pair = LocalToGame(coords);
			GameObject animObj;
			animObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			animObj.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 1f);
			animObj.transform.position = controller.movement.ConvertPosition(pair.Key, pair.Value, -2.0f);
			animObj.transform.rotation = Quaternion.Euler(new Vector3(Direction.Rotation(direction), 270, 90));
			animObj.transform.localScale = new Vector3(0.05f, 1, 0.05f);
			anim.Add(coords, animObj);
		}
		
		protected override void Hit(EntityController control){
			if(control.combat.effects.ContainsKey("slow") || control.combat.effects.ContainsKey("freeze")){
				control.combat.TakeDamage(controller.combat, 10);
			}
			//control.combat.AddEffect("slow", 3.0f);
			//control.combat.TakeDamage(controller.combat, 10);
		}
	}
}
