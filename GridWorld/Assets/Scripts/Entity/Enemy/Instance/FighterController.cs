using UnityEngine;
using System.Collections;

namespace FighterEnemy{
	public class FighterController : EnemyBaseController {

		
		public new FighterMovement movement{
			get{
				return ___movement;
			}
		}
		
		FighterMovement ___movement;
		
		public new FighterCombat combat{
			get{
				return ___combat;
			}
		}
		FighterCombat ___combat;
		
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
		
		public override void Initialize(){
			if (started) {
				return;
			}
			started = true;
			___movement = this.gameObject.AddComponent <FighterMovement>() 
				as FighterMovement;
			movement.Initialize(this, 4, 8, Direction.right, map);
			___combat = this.gameObject.AddComponent<FighterCombat>()
				as FighterCombat;
			combat.Initialize (this);
			combat.holder = this.gameObject.GetComponent<EnemyBaseManager> ().holder;
		}
		
		
		public class FighterMovement : EnemyBaseMovement{
			
			new FighterController controller;
			bool started = false;
			int lastDirection;
			
			public void Initialize(FighterController control, int x, int y, int dir, MapGenerator map){
				if (started) {
					return;
				}
				started = true;
				controller = control;
				this.map = map;
				Setup (x, y, dir);
				lastDirection = GetDirection();
			}
			
			public void MoveRandom(){
				int targetDir = Direction.RandomDirection ();
				if (Random.Range (0, 2) == 0) {
					targetDir = lastDirection;
				}
				lastDirection = targetDir;
				GoTowards (targetDir);
			}

			protected override EnemyBaseController GetController(){
				return controller;
			}
		}
		
		public class FighterCombat : EnemyBaseCombat{
			
			new FighterController controller;
			bool started = false;
			
			//float strikeChance = 0.25f;
			
			public FighterCombat(){
			}
			
			public void Initialize(FighterController control){
				if (started) {
					return;
				}
				started = true;
				controller = control;
				skills = new Skill[1];
				skills [0] = Slash.Default (controller);
				maxHealth = 10;
			}
			
			protected override void PerformAction(){
				int xDist = controller.target.movement.playerX - controller.movement.playerX;
				int yDist = controller.target.movement.playerY - controller.movement.playerY;
				if (Abs (xDist) + Abs (yDist) == 1) {
					Attack ();
				} else {
					float f = Random.value;
					int primaryD;
					int secondaryD;
					if(Abs(xDist) > Abs(yDist)){
						if(xDist > 0){
							primaryD = Direction.right;
						}
						else{
							primaryD = Direction.left;
						}
						if(yDist > 0){
							secondaryD = Direction.up;
						}
						else{
							secondaryD = Direction.down;
						}
					}
					else{
						if(yDist > 0){
							primaryD = Direction.up;
						}
						else{
							primaryD = Direction.down;
						}
						if(xDist > 0){
							secondaryD = Direction.right;
						}
						else{
							secondaryD = Direction.left;
						}
						if(!(Abs(yDist) > Abs(xDist))){
							if(Random.value > 0.5){
								int tempD = primaryD;
								primaryD = secondaryD;
								secondaryD = tempD;
							}
						}
					}
					if(f > 0.2){
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
			
			void Attack(){
				int xDist = controller.target.movement.playerX - controller.movement.playerX;
				int yDist = controller.target.movement.playerY - controller.movement.playerY;
				if (xDist == 1) {
					if (!controller.movement.TryTurn (Direction.right)) {
						action = skills [0].Activate ();
					}
				} else if (xDist == -1) {
					if (!controller.movement.TryTurn (Direction.left)) {
						action = skills [0].Activate ();
					}
				} else if (yDist == 1) {
					if (!controller.movement.TryTurn (Direction.up)) {
						action = skills [0].Activate ();
					}
				} else if (yDist == -1) {
					if (!controller.movement.TryTurn (Direction.down)) {
						action = skills [0].Activate ();
					}
				} else {
					Debug.Log ("Wut");
				}
			}
			
			static int Abs(int x){
				if (x < 0) {
					return -x;
				} else {
					return x;
				}

			}

			static int Signum(int x){
				if (x < 0) {
					return -1;
				} else if (x > 0) {
					return 1;
				} else {
					return 0;
				}
			}

			protected override EnemyBaseController GetController(){
				return controller;
			}
			
		}
	}
}