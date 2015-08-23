using UnityEngine;
using System.Collections.Generic;

public class Lunge : Skill{

	float cd;
	float dashTime;
	
	public Lunge(float cd, float dashTime, float manaCost) 
	: base(cd, manaCost){
		this.cd = cd;
		this.dashTime = dashTime;
	}
	
	public override SkillEvent GetSkillEvent(EntityController controller){
		List<SkillEvent> evs = new List<SkillEvent>();
		evs.Add(new LungeDashEvent(controller, dashTime));
		evs.Add(new LungeStrikeEvent(controller, cd - dashTime));
		return new CompositeSkillEvent(controller, evs);
	}

	public static Lunge Default(){
		return new Lunge(1f, 0.3f, 10);
	}

	public override string GetCustomStat(){
		return "Range: " + 3;
	}

	public override SkillInfo GetID(){
		return SkillInfo.Lunge;
	}

	public override string GetBody(){
		return "Dash for a short distance, then performs a strike";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Dash");
		hs.Add("Slash");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(2);
	}

	public class LungeDashEvent : NoncombatAbstractSkillEvent{
		
		public LungeDashEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		int dashDistance = 3;
		int stop = 0;
		bool moveDone;
		
		public override bool Update(){
			if(!moveDone){
				stop = dashDistance;
				int start = (int)(TimePassed() / cooldown * dashDistance);
				for(int i = start; i <= dashDistance; i++){
					KeyValuePair<int, int> pair = LocalToGame(new KeyValuePair<int, int>(0, i));
					if(!controller.movement.CanMoveTo(pair.Key, pair.Value)){
						stop = i - 1;
						break;
					}
				}
				//if(stop == 0){
				//	moveDone = true;
				//}
				//else{
				KeyValuePair<int, int> limPair = LocalToGame(new KeyValuePair<int, int>(0, dashDistance));
				KeyValuePair<int, int> stopPair = LocalToGame(new KeyValuePair<int, int>(0, stop));
				Vector3 limDist = (
					controller.movement.ConvertPosition(limPair.Key, limPair.Value, -1.0f) - 
					controller.movement.ConvertPosition(x, y, -1.0f));
				Vector3 travelDist = (
					controller.movement.ConvertPosition(stopPair.Key, stopPair.Value, -1.0f) - 
					controller.movement.ConvertPosition(x, y, -1.0f));

				float clampPassed = Mathf.Clamp(TimePassed() / cooldown, 0, travelDist.magnitude / limDist.magnitude);

				controller.movement.transform.position = 
					controller.movement.ConvertPosition(x, y, -1.0f) + ((limDist) * (clampPassed));
				if(clampPassed > travelDist.magnitude / limDist.magnitude){
					moveDone = true;
				}
				//	}
			}
			if(TimePassed() > cooldown){
				return false;
			}
			return true;
		}

		public override void CleanUp(){
			KeyValuePair<int, int> stopPair = LocalToGame(new KeyValuePair<int, int>(0, stop));
			controller.movement.TryMove(stopPair.Key, stopPair.Value, direction, EntityMovement.MoveMode.NoCooldown);
			//controller.movement.UpdatePosition();
		}

		protected override bool CanRun(){
			KeyValuePair<int, int> pair = LocalToGame(new KeyValuePair<int, int>(0, 1));
			if(!controller.movement.CanMoveTo(pair.Key, pair.Value)){
				return false;
			}
			return true;
		}
	}

	public class LungeStrikeEvent : AbstractSkillEvent{

		
		protected GameObject animObj;

		public LungeStrikeEvent(EntityController cont, float cd){
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
		}
	}
}
