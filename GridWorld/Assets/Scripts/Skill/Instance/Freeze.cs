using UnityEngine;
using System.Collections.Generic;

public class Freeze : Skill{

	float cd;

	public Freeze(float cd, float manaCost) 
			: base(cd, manaCost){
		this.cd = cd;
	}

	public override SkillEvent GetSkillEvent(EntityController controller){
		return new FreezeEvent(controller, cd);
	}

	public static Freeze Default(){
		return new Freeze(0.5f, 20);
	}

	public override SkillInfo GetID(){
		return SkillInfo.Freeze;
	}

	public override string GetCustomStat(){
		return "Range: " + 1;
	}

	public override string GetBody(){
		return "Freeze all enemies around the caster";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		//hs.Add("Cleave");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(2);
	}

	public override SkillAnimation GetAnimation(int x, int y, int direction, float length){
		return new FreezeAnimation(x, y, direction, length);
	}
	
	public class FreezeAnimation : BoxSkillAnimation{
		
		public FreezeAnimation(int x, int y, int direction, float length) : base(x, y, direction, length){
			
		}
		
		public override Color GetColor(){
			return new Color(1, 0, 0);
		}
		
		int extent = 1;
		
		public override HashSet<KeyValuePair<int, int>> GetCoords(){
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
	}

	public class FreezeEvent : AbstractSkillEvent{

		float duration = 3;
		
		//public Dictionary<KeyValuePair<int, int>, GameObject> anim;
		
		public FreezeEvent(EntityController cont, float cd){
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
					(0.05f * (1 - TimePassed() / duration), animX.transform.localScale.y, 0.05f * (1 - TimePassed() / duration));
				
			}*/
			if(TimePassed() > duration){
				return false;
			}
			return true;
		}
		
		public override void CleanUp(){
			/*foreach(KeyValuePair<KeyValuePair<int, int>, GameObject> animPair in anim){
				GameObject.Destroy(animPair.Value);
			}*/
		}
		
		int extent = 1;
		
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
			/*KeyValuePair<int, int> pair = LocalToGame(coords);
			GameObject animObj;
			animObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			animObj.GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 1f);
			animObj.transform.position = controller.movement.ConvertPosition(pair.Key, pair.Value, -2.0f);
			animObj.transform.rotation = Quaternion.Euler(new Vector3(Direction.Rotation(direction), 270, 90));
			animObj.transform.localScale = new Vector3(0.05f, 1, 0.05f);
			anim.Add(coords, animObj);*/
		}
		
		protected override void Hit(EntityController control){
			control.combat.AddEffect("freeze", 3.0f);
			//control.combat.TakeDamage(controller.combat, 10);
		}

		public override SkillInfo GetInfo(){
			return SkillInfo.Freeze;
		}
	}
}
