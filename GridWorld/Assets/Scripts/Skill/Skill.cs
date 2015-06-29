using UnityEngine;
using System.Collections;

public abstract class Skill{

	protected string name;
	protected EntityController controller;
	public float manaCost{
		get{
			return _manaCost;
		}
	}
	float _manaCost;

	public Skill(EntityController control, string skillName, float cd, float manaCost){
		this.controller = control;
		name = skillName;
		_cooldown = cd;
		_manaCost = manaCost;
	}

	public Skill(EntityController control) : this (control, "generic_skill", 1.0f, 0){

	}

	public float cooldown{
		get{
			return _cooldown;
		}
	}
	float _cooldown;

	public float Activate(){
		if (controller.combat.mana < manaCost) {
			return 0.0f;
		}
		if (controller.combat.ActivateAnimation (GetSkillEvent ())) {
			controller.combat.mana -= manaCost;
			return cooldown;
		}
		return 0.0f;
	}

	public abstract SkillEvent GetSkillEvent ();

	public string GetName(){
		return name;
	}
}
