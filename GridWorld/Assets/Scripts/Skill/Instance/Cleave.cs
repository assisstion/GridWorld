using UnityEngine;
using System.Collections.Generic;

public class Cleave : Skill{

	float cd;

	public Cleave(EntityController control, float cd, float manaCost) 
	: base(control, "Cleave", cd, manaCost){
		this.cd = cd;
	} 

	public override SkillEvent GetSkillEvent(){
		return new CleaveEvent (controller, cd);
	}

	public static Cleave Default(EntityController control){
		return new Cleave (control, 1f, 25);
	}

	public override int GetID (){
		return 9;
	}

	public override string GetCustomStat(){
		return "Range: " + 1;
	}

	public override string GetBody(){
		return "Deal damage to all enemies in front of the caster";
	}

	public override HashSet<string> GetPrerequisites ()
	{
		HashSet<string> hs = new HashSet<string> ();
		hs.Add ("Slash");
		return hs;
	}
	
	public override int GetMinimumWave ()
	{
		return Skill.MinimumWaveFromTier (1);
	}

	public class CleaveEvent : AbstractSkillEvent{
		
		public Dictionary<KeyValuePair<int, int>, GameObject> anim;
		
		public CleaveEvent(EntityController cont, float cd){
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
		
		int extent = 1;
		
		protected override HashSet<KeyValuePair<int, int>> GetCoordinates (){
			HashSet<KeyValuePair<int,int>> set = new HashSet<KeyValuePair<int, int>> ();
			for (int x = -extent; x <= extent; x++) {
					set.Add (new KeyValuePair<int, int> (x, 1));
			}
			return set;
		}
		
		protected override void RunAttack(KeyValuePair<int, int> coords){
			KeyValuePair<int, int> pair = LocalToGame (coords);
			GameObject animObj;
			animObj = GameObject.CreatePrimitive (PrimitiveType.Plane);
			animObj.GetComponent<MeshRenderer> ().material.color = new Color (1f, 1f, 0);
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
			return !(casts.Contains(new KeyValuePair<int, int>(0,1)));
		}
	}
}
