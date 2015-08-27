using UnityEngine;
using System.Collections.Generic;

public class EmptySkillAnimation : SkillAnimation{

	public EmptySkillAnimation() : base(0, 0, Direction.up, 0){
	}

	public override void Animate(){
		//
	}

	public override void Destroy(){
		//
	}

	public override void Update(float timePassed){
		//
	}
}
