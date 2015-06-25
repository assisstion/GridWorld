using UnityEngine;
using System.Collections;

public class Slash : Skill{

	public Slash(PlayerController control) : base(control, "slash", 1.0f){

	} 

	public override SkillEvent GetSkillEvent(){
		return new SlashSkillEvent (controller);
	}

	public class SlashSkillEvent : AbstractSkillEvent{

		GameObject animObj;
		PlayerController controller;

		public SlashSkillEvent(PlayerController cont){
			controller = cont;
		}

		public override bool Initialize(){
			base.Initialize ();

			int x = controller.movement.playerX + Direction.ValueX (controller.movement.direction);
			int y = controller.movement.playerY + Direction.ValueY (controller.movement.direction);

			//Cancel checking
			if(!(controller.movement.IsGameSpace(x, y) && controller.movement.CanPass (controller.movement.map.tiles [x, y]))){
				return false;
			}

			//Animation
			animObj = GameObject.CreatePrimitive (PrimitiveType.Plane);
			animObj.transform.position = controller.movement.ConvertPosition (x, y, -2.0f);
			animObj.transform.rotation = Quaternion.Euler (new Vector3 (Direction.Rotation(controller.movement.direction), 270, 90));
			animObj.transform.localScale = new Vector3 (0.1f, 1, 0.02f);

			//On hit effects
			GameObject target = controller.movement.map.objects[x,y];
			if (target != null) {
				EnemyBaseManager manager = target.GetComponent<EnemyBaseManager>();
				if(manager != null){
					EnemyBaseController enemyControl = manager.controller;
					enemyControl.combat.TakeDamage(100);
				}
			}

			return true;
		}

		public override bool Update(){
			if (TimePassed() > 1) {
				return false;
			}
			return true;
		}

		public override void CleanUp(){
			GameObject.Destroy (animObj);
		}
	}
}
