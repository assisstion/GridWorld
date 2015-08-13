using UnityEngine;
using System.Collections.Generic;

public class Charge : Skill{

	float cd;
	float dashTime;
	
	public Charge(EntityController control, float cd, float dashTime, float manaCost) 
	: base(control, cd, manaCost){
		this.cd = cd;
		this.dashTime = dashTime;
	}
	
	public override SkillEvent GetSkillEvent(){
		return new ChargeDashEvent(controller, dashTime);
	}

	public static Charge Default(EntityController control){
		return new Charge(control, 0.5f, 0.5f, 30);
	}

	public override string GetCustomStat(){
		return "Distance: " + 5;
	}

	public override SkillInfo GetID(){
		return SkillInfo.Charge;
	}

	public override string GetBody(){
		return "Charge through enemies, dealing damage to all enemies struck";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		hs.Add("Lunge");
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(4);
	}

	public class ChargeDashEvent : NoncombatAbstractSkillEvent{
		
		public ChargeDashEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		int dashDistance = 5;
		int stop = 0;
		bool moveDone;

		bool TryHit(int x, int y){
			if(!controller.movement.IsGameSpace(x, y)){
				return false;
			}
			GameObject target = controller.movement.map.objects[x, y];
			if(target == null){
				return false;
			}
			if(target.tag.Equals("Enemy")){
				if(!controller.tag.Equals("Enemy")){
					EnemyBaseManager manager = target.GetComponent<EnemyBaseManager>();
					if(manager != null){
						Hit(manager.controller);
						return true;
					}
				}
			}
			else if(target.tag.Equals("Player")){
				if(!controller.tag.Equals("Player")){
					Hit(target.GetComponent<PlayerController>());
					return true;
				}
			}
			return false;
		}
		
		public override bool Update(){
			if(!moveDone){
				stop = dashDistance;
				int start = (int)(TimePassed() / cooldown * dashDistance);
				for(int i = start; i <= dashDistance; i++){
					KeyValuePair<int, int> pair = LocalToGame(new KeyValuePair<int, int>(0, i));
					bool go = true;
					if(i == start + 1){
						if(TryHit(pair.Key, pair.Value)){
							go = false;
						}
					}
					if(go && !controller.movement.CanMoveTo(pair.Key, pair.Value)){
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
			controller.movement.UpdatePosition();
		}

		protected override bool CanRun(){
			KeyValuePair<int, int> pair = LocalToGame(new KeyValuePair<int, int>(0, 1));
			if(!controller.movement.CanMoveTo(pair.Key, pair.Value)){
				return false;
			}
			return true;
		}

		protected override void Hit(EntityController control){
			control.combat.TakeDamage(controller.combat, 10);
		}
	}
}
