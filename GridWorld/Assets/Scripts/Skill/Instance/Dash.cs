﻿using UnityEngine;
using System.Collections.Generic;

public class Dash : Skill{

	float cd;
	
	public Dash(float cd, float manaCost) 
	: base(cd, manaCost){
		this.cd = cd;
	}
	
	public override SkillEvent GetSkillEvent(EntityController controller){
		return new DashSkillEvent(controller, cd);
	}

	public static Dash Default(){
		return new Dash(0.3f, 10);
	}

	public override string GetCustomStat(){
		return "Range: " + 2;
	}

	public override SkillInfo GetID(){
		return SkillInfo.Dash;
	}

	public override string GetBody(){
		return "Quickly dash for a short distance.";
	}

	public override HashSet<string> GetPrerequisites(){
		HashSet<string> hs = new HashSet<string>();
		return hs;
	}
	
	public override int GetMinimumWave(){
		return Skills.MinimumWaveFromTier(1);
	}

	public override SkillAnimation GetAnimation(int x, int y, int direction, float length){
		//TODO Add dash animation
		return new EmptySkillAnimation();
	}

	public class DashSkillEvent : NoncombatAbstractSkillEvent{
		
		public DashSkillEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		int dashDistance = 2;
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
				//}
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

		public override SkillInfo GetInfo(){
			return SkillInfo.Dash;
		}
	}
}
