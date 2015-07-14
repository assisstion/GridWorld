using UnityEngine;
using System.Collections.Generic;

public class Slash : Skill{

	float cd;

	public Slash(EntityController control, float cd, float manaCost) 
			: base(control, cd, manaCost){
		this.cd = cd;
	} 

	public override SkillEvent GetSkillEvent(){
		return new SlashSkillEvent (controller, cd);
	}

	public static Slash Default(EntityController control){
		return new Slash (control, 0.35f, 0);
	}

	public override SkillInfo GetID (){
		return SkillInfo.Slash;
	}

	public override string GetCustomStat(){
		return "";
	}

	public override string GetBody(){
		return "A basic slash that damages enemies";
	}

	public override HashSet<string> GetPrerequisites ()
	{
		HashSet<string> hs = new HashSet<string> ();
		return hs;
	}
	
	public override int GetMinimumWave ()
	{
		return Skills.MinimumWaveFromTier (0);
	}

	public class SlashSkillEvent : AbstractSkillEvent{

		
		protected GameObject animObj;

		public SlashSkillEvent(EntityController cont, float cd){
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
