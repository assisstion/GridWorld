using UnityEngine;
using System.Collections;

public class Slash : Skill{

	float cd;

	public Slash(EntityController control, float cd) 
			: base(control, "slash", cd){
		this.cd = cd;
	} 

	public override SkillEvent GetSkillEvent(){
		return new SlashSkillEvent (controller, cd);
	}

	public class SlashSkillEvent : AbstractSkillEvent{

		int direction;
		int x;
		int y;
		float cooldown;
		GameObject animObj;
		EntityController controller;

		public SlashSkillEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
		}

		public override bool Initialize(){
			base.Initialize ();

			direction = controller.movement.direction;
			x = controller.movement.playerX + Direction.ValueX (direction);
			y = controller.movement.playerY + Direction.ValueY (direction);

			//Cancel checking
			if(!(controller.movement.IsGameSpace(x, y) && controller.movement.CanPass (controller.movement.map.tiles [x, y]))){
				return false;
			}

			//Animation
			animObj = GameObject.CreatePrimitive (PrimitiveType.Plane);
			animObj.transform.position = controller.movement.ConvertPosition (x, y, -2.0f);
			animObj.transform.rotation = Quaternion.Euler (new Vector3 (Direction.Rotation(direction), 270, 90));
			animObj.transform.localScale = new Vector3 (0.1f, 1, 0.02f);

			//On hit effects
			GameObject target = controller.movement.map.objects[x,y];
			if (target != null) {
				if(target.tag.Equals("Enemy")){
					if(!controller.tag.Equals("Enemy")){
						EnemyBaseManager manager = target.GetComponent<EnemyBaseManager>();
						if(manager != null){
							EnemyBaseController enemyControl = manager.controller;
							enemyControl.combat.TakeDamage(10);
						}
					}
				}
				else if(target.tag.Equals("Player")){
					if(!controller.tag.Equals("Player")){
						target.GetComponent<PlayerCombat>().TakeDamage(10);
					}
				}
			}

			return true;
		}

		public override bool Update(){
			animObj.transform.position = controller.movement.ConvertPosition (x, y, -2.0f) 
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
	}
}
