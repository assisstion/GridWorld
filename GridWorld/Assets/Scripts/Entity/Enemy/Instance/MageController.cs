using UnityEngine;
using System.Collections;

namespace MageEnemy{
	public class MageController : EnemyBaseController{

		
		public new MageMovement movement{
			get{
				return ___movement;
			}
		}
		
		MageMovement ___movement;
		
		public new MageCombat combat{
			get{
				return ___combat;
			}
		}

		MageCombat ___combat;
		
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
			___movement = this.gameObject.AddComponent <MageMovement>() 
				as MageMovement;
			movement.Initialize(this, 4, 8, Direction.right, map);
			___combat = this.gameObject.AddComponent<MageCombat>()
				as MageCombat;
			combat.Initialize(this);
			combat.holder = this.gameObject.GetComponent<EnemyBaseManager>().holder;
		}
		
		
		public class MageMovement : EnemyBaseMovement{

			// Use this for initialization
			protected override void Start(){
				base.Start();
			}
			
			// Update is called once per frame
			protected override void Update(){
				base.Update();
			}
			
			new MageController controller;
			bool started = false;
			int lastDirection;
			
			public void Initialize(MageController control, int x, int y, int dir, MapGenerator map){
				if(started){
					return;
				}
				started = true;
				controller = control;
				this.map = map;
				Setup(x, y, dir);
				lastDirection = GetDirection();
			}
			
			public void MoveRandom(){
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
		
		public class MageCombat : EnemyBaseCombat{

			// Use this for initialization
			protected override void Start(){
				base.Start();
			}
			
			// Update is called once per frame
			protected override void Update(){
				base.Update();
			}
			
			new MageController controller;
			bool started = false;
			
			//float strikeChance = 0.25f;
			
			public MageCombat(){
			}
			
			public void Initialize(MageController control){
				if(started){
					return;
				}
				started = true;
				controller = control;
				skills = new Skill[1];
				skills[0] = Fireball.Default(controller);
				_maxHealth = 10;
			}

			protected bool CanHit(int x, int y){
				return (Abs(x) <= 1 && Abs(y) <= 1);
			}

			int Zero(int xDist, int yDist){
				if(Abs(xDist) < Abs(yDist)){
					if(xDist < 0){
						return Direction.left;
					}
					else if(xDist > 0){
						return Direction.right;
					}
					else{
						return -1;
					}
				}
				else{
					if(yDist < 0){
						return Direction.up;
					}
					else if(yDist > 0){
						return Direction.down;
					}
					else{
						return -1;
					}
				}
			}

			int Four(int xDist, int yDist){
				if(Abs(Abs(xDist) - 4) < Abs(Abs(yDist) - 4)){
					if((xDist > 0 && xDist < 4) || xDist < -4){
						return Direction.right;
					}
					else if((xDist < 0 && xDist > -4) || xDist > 4){
						return Direction.left;
					}
					else{
						return -1;
					}
				}
				else{
					if((yDist > 0 && yDist < 4) || yDist < -4){
						return Direction.up;
					}
					else if((yDist < 0 && yDist > -4) || yDist > 4){
						return Direction.down;
					}
					else{
						return -1;
					}
				}
			}
			
			protected override void PerformAction(){
				int xDist = -controller.target.movement.playerX + controller.movement.playerX;
				int yDist = -controller.target.movement.playerY + controller.movement.playerY;

				bool can = true;
				int dir = Direction.down;
				if(CanHit(xDist, yDist - 4)){
					dir = Direction.down;
				}
				else if(CanHit(xDist, yDist + 4)){
					dir = Direction.up;
				}
				else if(CanHit(xDist - 4, yDist)){
					dir = Direction.left;
				}
				else if(CanHit(xDist + 4, yDist)){
					dir = Direction.right;
				}
				else{
					can = false;
				}
				if(can){
					//Debug.Log(dir);
					Attack(dir);
				}
				else{

					int primaryD = -1;
					int secondaryD = -1;
					int z = Zero(xDist, yDist);
					if(z >= 0){
						primaryD = z;
						int t = Four(xDist, yDist);
						if(t >= 0){
							secondaryD = t;
						}
						else{
							secondaryD = primaryD;
						}
					}
					else{
						int t = Four(xDist, yDist);
						if(t >= 0){
							primaryD = t;
							secondaryD = t;
						}
						else{
							primaryD = Direction.RandomDirection();
							secondaryD = Direction.RandomDirection();
						}
					}
					float f = Random.value;
					if(f > 0.8){
						if(controller.movement.GetDirection() == primaryD){
							controller.movement.GoTowards(primaryD);
						}
						else if(controller.movement.GetDirection() == secondaryD){
							controller.movement.GoTowards(secondaryD);
						}
						else{
							controller.movement.MoveRandom();
						}
					}
					else if(f > 0.2){
						controller.movement.GoTowards(primaryD);
					}
					else if(f > 0.1){
						controller.movement.GoTowards(secondaryD);
					}
					else{
						controller.movement.MoveRandom();
					}
				}
			}
			
			void Attack(int direction){
				if(!controller.movement.TryTurn(direction)){
					SetAction(skills[0].Activate());
				}
			}
			
			static int Abs(int x){
				if(x < 0){
					return -x;
				}
				else{
					return x;
				}

			}

			static int Signum(int x){
				if(x < 0){
					return -1;
				}
				else if(x > 0){
					return 1;
				}
				else{
					return 0;
				}
			}

			protected override EnemyBaseController GetController(){
				return controller;
			}
			
		}
	}
}