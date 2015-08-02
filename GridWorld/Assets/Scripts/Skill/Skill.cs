using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

public abstract class Skill{

	//protected string name;
	protected EntityController controller;

	public float manaCost{
		get{
			return _manaCost;
		}
	}

	float _manaCost;

	public Skill(EntityController control, float cd, float manaCost){
		this.controller = control;
		//name = skillName;
		_cooldown = cd; 
		_manaCost = manaCost;
	}

	public Skill(EntityController control) : this (control, 1.0f, 0){
		 
	}

	public EntityController GetController(){
		return controller;
	}

	public float cooldown{
		get{
			return _cooldown;
		}
	}

	float _cooldown;

	public float Activate(){
		 
		switch(GetCostType()){
			case CostType.Mana:
				if(controller.combat.mana < manaCost){
					break;
				}
				if(controller.combat.ActivateAnimation(GetSkillEvent())){
					controller.combat.mana -= manaCost;
					return cooldown;
				} 
				break;
			case CostType.Health:
				if(controller.combat.health < manaCost){
					break;
				}
				if(controller.combat.ActivateAnimation(GetSkillEvent())){
					controller.combat.health -= manaCost;
					return cooldown;
				}
				break;
			default:
				throw new Exception("Invalid CostType");
		}


		return 0.0f;
	}
	 
	public virtual CostType GetCostType(){
		return CostType.Mana;
	}

	public abstract HashSet<string> GetPrerequisites();

	public abstract int GetMinimumWave();

	public abstract SkillEvent GetSkillEvent();

	public string GetName(){ 
		return Skills.Attr(GetID()).title;
	}

	public string GetInfo(){
		return GetCostType().ToString() + ": " + manaCost + "\n" +
			"Cooldown: " + cooldown + " s\n" +
			GetCustomStat();
	}

	public abstract string GetBody();

	public abstract string GetCustomStat();
	
	public abstract SkillInfo GetID(); 
}

public enum CostType{
	Mana,
	Health
}