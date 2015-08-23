using UnityEngine;
using System.Collections;

namespace TargetDummyEnemy{
	public class TargetDummyController : EnemyBaseController{

		int mode;

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
			base.Start();
			if(!started){
				Initialize();
			}
		}

		protected override void Update(){
			base.Update();
		}

		public override void Initialize(){
			if(started){
				return;
			}
			started = true;
			___movement = this.gameObject.AddComponent <TargetDummyMovement>() 
				as TargetDummyMovement;
			movement.Initialize(this, 4, 8, Direction.right, map);
			___combat = this.gameObject.AddComponent<TargetDummyCombat>()
				as TargetDummyCombat;
			combat.Initialize(this);
			combat.holder = this.gameObject.GetComponent<EnemyBaseManager>().holder;
		}

		//0 = no attack
		//1 = slash
		//2 = slash/dash
		//3 = slash/dash/fireball/heal
		public void SetMode(int mode){
			this.mode = mode;
		}

		
		public class TargetDummyMovement : EnemyBaseMovement{

			// Use this for initialization
			protected override void Start(){
				base.Start();
			}
			
			// Update is called once per frame
			protected override void Update(){
				base.Update();
			}

			new TargetDummyController controller;
			bool started = false;
			int lastDirection;

			public void Initialize(TargetDummyController control, int x, int y, int dir, MapGenerator map){
				if(started){
					return;
				}
				started = true;
				controller = control;
				this.map = map;
				Setup(x, y, dir);
				lastDirection = GetDirection();
			}

			public void Move(){
				int targetDir = Direction.RandomDirection();
				if(Random.Range(0, 2) == 0){
					targetDir = lastDirection;
				}
				lastDirection = targetDir;
				GoTowards(targetDir);
			}

			protected override EnemyBaseController GetController(){
				return controller;
			}
		}
		
		public class TargetDummyCombat : EnemyBaseCombat{

			// Use this for initialization
			protected override void Start(){
				base.Start();
			}
			
			// Update is called once per frame
			protected override void Update(){
				base.Update();
			}

			new TargetDummyController controller;
			bool started = false;
			float strikeChance = 0.25f;

			public TargetDummyCombat(){
			}

			public void Initialize(TargetDummyController control){
				if(started){
					return;
				}
				started = true;
				controller = control;
				skills = new Skill[4];
				skills[0] = Slash.Default(controller);
				skills[1] = Lunge.Default(controller);
				skills[2] = Fireball.Default(controller);
				skills[3] = Heal.Default(controller);
				_maxHealth = 10;
			}

			protected override void PerformAction(){
				if(Random.value < strikeChance){
					Attack();
				}
				else{
					controller.movement.Move();
				}
			}

			void Attack(){
				int[] aSkill = AllowedSkills();
				if(aSkill.Length == 0){
					SetAction(0);
				}
				else{
					SetAction(skills[aSkill[
					  Random.Range(0, aSkill.Length)]].Activate());
				}
			}

			protected override EnemyBaseController GetController(){
				return controller;
			}

			public int[] AllowedSkills(){
				switch(controller.mode){
					case 0:
						return new int[]{};
					case 1:
						return new int[]{0};
					case 2:
						return new int[]{0,1};
					case 3:
						return new int[]{0,1,2};
					default:
						return new int[]{};
				}
			}
		}
	}
}