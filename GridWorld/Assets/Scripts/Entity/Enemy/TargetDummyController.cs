using UnityEngine;
using System.Collections;

namespace TargetDummyEnemy{
	public class TargetDummyController : EnemyBaseController {

		public MapGenerator map;

		public new TargetDummyMovement movement{
			get{
				return ___movement;
			}
		}

		TargetDummyMovement ___movement;
		
		public new TargetDummyCombat combat{
			get{
				return ___combat;
			}
		}
		TargetDummyCombat ___combat;

		protected override EntityMovement MovementHolder(){
			return ___movement;
		}
		
		protected override EntityCombat CombatHolder(){
			return ___combat;
		}

		bool started;

		protected override void Start(){
			base.Start ();
			if (!started) {
				Initialize();
			}
		}

		protected override void Update(){
			base.Update ();
		}

		public void Initialize(){
			if (started) {
				return;
			}
			started = true;
			___movement = this.gameObject.AddComponent <TargetDummyMovement>() 
				as TargetDummyMovement;
			movement.Initialize(this, 4, 8, Direction.right, map);
			___combat = this.gameObject.AddComponent<TargetDummyCombat>()
				as TargetDummyCombat;
			combat.Initialize (this);
			combat.holder = this.gameObject.GetComponent<EnemyBaseManager> ().holder;
		}

		
		public class TargetDummyMovement : EnemyBaseMovement{

			new TargetDummyController controller;
			bool started = false;
			int lastDirection;

			public void Initialize(TargetDummyController control, int x, int y, int dir, MapGenerator map){
				if (started) {
					return;
				}
				started = true;
				controller = control;
				Setup (x, y, dir);
				this.map = map;
				lastDirection = direction;
			}

			public void Move(){
				int targetDir = Direction.RandomDirection ();
				if (Random.Range (0, 2) == 0) {
					targetDir = lastDirection;
				}
				lastDirection = targetDir;
				GoTowards (targetDir);
			}

			public void SetLocation(int x, int y, int dir){
				Setup (x, y, dir);
			}

			protected override void MoveSuccess(){
				controller.combat.action = moveCooldown;
			}
		}
		
		public class TargetDummyCombat : EnemyBaseCombat{

			new TargetDummyController controller;
			bool started = false;

			public TargetDummyCombat(){
			}

			public void Initialize(TargetDummyController control){
				if (started) {
					return;
				}
				started = true;
				controller = control;
			}

			protected override void PerformAction(){
				controller.movement.Move ();
			}
			
		}
	}
}