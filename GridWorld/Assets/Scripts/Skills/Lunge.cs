using UnityEngine;
using System.Collections.Generic;

public class Lunge : Skill {

	float cd;
	float dashTime;
	
	public Lunge(EntityController control, float cd, float dashTime) 
	: base(control, "slash", cd){
		this.cd = cd;
		this.dashTime = dashTime;
	} 
	
	public override SkillEvent GetSkillEvent(){
		List<SkillEvent> evs = new List<SkillEvent> ();
		evs.Add (new LungeDashEvent (controller, dashTime));
		evs.Add (new LungeStrikeEvent (controller, dashTime - cd));
		return new CompositeSkillEvent(controller, evs);
	}

	public class LungeDashEvent : NoncombatAbstractSkillEvent{
		
		public LungeDashEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		int dashDistance = 2;
		int stop = 0;
		
		public override bool Update(){
			stop = dashDistance;
			for (int i = 1; i <= dashDistance; i++) {
				KeyValuePair<int, int> pair = LocalToGame (new KeyValuePair<int, int> (0, dashDistance));
				if(!controller.movement.CanMoveTo(pair.Key, pair.Value)){
					stop = i - 1;
				}
			}
			KeyValuePair<int, int> limPair = LocalToGame (new KeyValuePair<int, int> (0, dashDistance));
			KeyValuePair<int, int> stopPair = LocalToGame (new KeyValuePair<int, int> (0, stop));
			Vector3 limDist = (
				controller.movement.ConvertPosition (limPair.Key, limPair.Value, -1.0f) - 
				controller.movement.ConvertPosition (x, y, -1.0f));
			Vector3 travelDist = (
				controller.movement.ConvertPosition (stopPair.Key, stopPair.Value, -1.0f) - 
				controller.movement.ConvertPosition (x, y, -1.0f));

			float clampPassed = Mathf.Clamp (TimePassed (), 0, travelDist.magnitude / limDist.magnitude);



			controller.movement.transform.position = 
				controller.movement.ConvertPosition (x, y, -1.0f) + ((limDist) * (clampPassed / cooldown));
	

			if (TimePassed() > cooldown) {
				return false;
			}
			return true;
		}

		public override void CleanUp(){
			KeyValuePair<int, int> stopPair = LocalToGame (new KeyValuePair<int, int> (0, stop));
			controller.movement.TryMove (stopPair.Key, stopPair.Value, direction, EntityMovement.MoveMode.NoCooldown);
		}
	}

	public class LungeStrikeEvent : AbstractSkillEvent{
		
		public LungeStrikeEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}
		
		public override bool Update(){
			KeyValuePair<int, int> pair = LocalToGame (new KeyValuePair<int, int>(0,1));
			int vx = pair.Key;
			int vy = pair.Value;
			animObj.transform.position = controller.movement.ConvertPosition (vx, vy, -2.0f) 
				- Direction.ToVector(direction).normalized
					*controller.movement.map.gridSize*(TimePassed()/cooldown)/2;
			animObj.transform.localScale = new Vector3 
				(0.1f*(1-TimePassed()/cooldown) ,animObj.transform.localScale.y,animObj.transform.localScale.z);
			if (TimePassed() > cooldown) {
				return false;
			}
			return true;
		}
		
		public override void CleanUp(){
			GameObject.Destroy (animObj);
		}
		
		protected override HashSet<KeyValuePair<int, int>> GetCoordinates (){
			HashSet<KeyValuePair<int,int>> set = new HashSet<KeyValuePair<int, int>> ();
			set.Add (new KeyValuePair<int, int> (0, 1));
			return set;
		}
		
		protected override void RunAttack(KeyValuePair<int, int> coords){
			KeyValuePair<int, int> pair = LocalToGame (coords);
			animObj = GameObject.CreatePrimitive (PrimitiveType.Plane);
			animObj.transform.position = controller.movement.ConvertPosition (pair.Key, pair.Value, -2.0f);
			animObj.transform.rotation = Quaternion.Euler (new Vector3 (Direction.Rotation(direction), 270, 90));
			animObj.transform.localScale = new Vector3 (0.1f, 1, 0.02f);
			
		}
		
		protected override void Hit(EntityController control){
			control.combat.TakeDamage (10);
		}
	}
}
