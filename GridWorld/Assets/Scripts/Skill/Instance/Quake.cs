using UnityEngine;
using System.Collections.Generic;

public class Quake : Skill{

	float cd;

	public Quake(EntityController control, float cd, float manaCost) 
			: base(control, "Quake", cd, manaCost){
		this.cd = cd;
	} 

	public override SkillEvent GetSkillEvent(){
		return new QuakeEvent (controller, cd);
	}

	public static Quake Default(EntityController control){
		return new Quake (control, 2f, 50);
	}

	public override SkillDB GetID (){
		return SkillDB.Quake;
	}

	public override string GetCustomStat(){
		return "Range: " + 1;
	}

	public override string GetBody(){
		return "Deal damage to all enemies around the caster";
	}

	public override HashSet<string> GetPrerequisites ()
	{
		HashSet<string> hs = new HashSet<string> ();
		hs.Add ("Cleave");
		return hs;
	}
	
	public override int GetMinimumWave ()
	{
		return Skill.MinimumWaveFromTier (3);
	}

	public class QuakeEvent : AbstractSkillEvent{
		
		public Dictionary<KeyValuePair<int, int>, GameObject> anim;
		
		public QuakeEvent(EntityController cont, float cd){
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
				for(int y = -extent; y <= extent; y++){
					if(x == 0 && y == 0){
						continue;
					}
					set.Add (new KeyValuePair<int, int> (x, y));
				}
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
	}
}
