﻿using UnityEngine;
using System.Collections.Generic;

public class Fireball : Skill {
	
	float cd;
	
	public Fireball(EntityController control, float cd, float manaCost) 
	: base(control, "fireball", cd, manaCost){
		this.cd = cd;
	} 
	
	public override SkillEvent GetSkillEvent(){
		return new FireballEvent (controller, cd);
	}

	public static Fireball Default(EntityController control){
		return new Fireball (control, 1.0f, 50);
	}
	
	public class FireballEvent : AbstractSkillEvent{

		public Dictionary<KeyValuePair<int, int>, GameObject> anim;
		
		public FireballEvent(EntityController cont, float cd){
			controller = cont;
			cooldown = cd;
			anim = new Dictionary<KeyValuePair<int, int>, GameObject>();
		}
		
		public override bool Update(){
			foreach(KeyValuePair<KeyValuePair<int, int>, GameObject> animPair in anim){
				KeyValuePair<int, int> pair = LocalToGame (animPair.Key);
				int vx = pair.Key;
				int vy = pair.Value;
				GameObject animX = animPair.Value;
				animX.transform.position = controller.movement.ConvertPosition (vx, vy, -2.0f);
				animX.transform.localScale = new Vector3 
					(0.05f*(1-TimePassed()/cooldown) ,animX.transform.localScale.y,0.05f*(1-TimePassed()/cooldown));

			}
			if (TimePassed() > cooldown) {
				return false;
			}
			return true;
		}
		
		public override void CleanUp(){
			foreach (KeyValuePair<KeyValuePair<int, int>, GameObject> animPair in anim) {
				GameObject.Destroy (animPair.Value);
			}
		}

		int centerX = 0;
		int centerY = 4;
		
		protected override HashSet<KeyValuePair<int, int>> GetCoordinates (){
			HashSet<KeyValuePair<int,int>> set = new HashSet<KeyValuePair<int, int>> ();
			for (int x = centerX - 1; x <= centerX + 1; x++) {
				for(int y = centerY - 1; y <= centerY + 1; y++){
					set.Add (new KeyValuePair<int, int> (x, y));
				}
			}
			return set;
		}
		
		protected override void RunAttack(KeyValuePair<int, int> coords){
			KeyValuePair<int, int> pair = LocalToGame (coords);
			GameObject animObj;
			animObj = GameObject.CreatePrimitive (PrimitiveType.Plane);
			animObj.GetComponent<MeshRenderer> ().material.color = new Color (1, 0, 0);
			animObj.transform.position = controller.movement.ConvertPosition (pair.Key, pair.Value, -2.0f);
			animObj.transform.rotation = Quaternion.Euler (new Vector3 (Direction.Rotation(direction), 270, 90));
			animObj.transform.localScale = new Vector3 (0.05f, 1, 0.05f);
			anim.Add (coords, animObj);
		}
		
		protected override void Hit(EntityController control){
			control.combat.TakeDamage (10);
		}

		protected override bool ShouldCancel(HashSet<KeyValuePair<int, int>> casts){
			if (base.ShouldCancel (casts)) {
				return true;
			}
			return !(casts.Contains(new KeyValuePair<int, int>(centerX, centerY)));
		}
	}
}