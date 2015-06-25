using UnityEngine;
using System.Collections;

public abstract class Skill{

	protected string name;
	protected PlayerController controller;

	public Skill(PlayerController control, string skillName, float cd){
		this.controller = control;
		name = skillName;
		_cooldown = cd;
	}

	public Skill(PlayerController control) : this (control, "generic_skill", 1.0f){

	}

	public float cooldown{
		get{
			return _cooldown;
		}
	}
	float _cooldown;

	public float Activate(){
		if (controller.combat.ActivateAnimation (GetSkillEvent ())) {
			
			return cooldown;
		}
		return 0.0f;
	}

	public abstract SkillEvent GetSkillEvent ();

	public string GetName(){
		return name;
	}
}
