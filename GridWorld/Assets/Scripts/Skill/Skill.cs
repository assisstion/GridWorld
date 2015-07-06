using UnityEngine;
using System.Collections.Generic;

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
		if (controller.combat.mana < manaCost) {
			return 0.0f;
		}
		if (controller.combat.ActivateAnimation (GetSkillEvent ())) {
			controller.combat.mana -= manaCost;
			return cooldown;
		}
		return 0.0f;
	}

	public abstract HashSet<string> GetPrerequisites ();

	public abstract int GetMinimumWave();

	public abstract SkillEvent GetSkillEvent ();

	public string GetName(){
		return name;
	}

	public string GetInfo(){
		return "Mana: " + manaCost + "\n" +
			"Cooldown: " + cooldown + " s\n" +
			GetCustomStat ();
	}

	public abstract string GetBody();

	public abstract string GetCustomStat ();

	public static Skill GetDefaultFromTitle(string title, PlayerController controller){
		switch (title) {
		case "Slash":
			return Slash.Default(controller);
		case "Lunge":
			return Lunge.Default(controller);
		case "Fireball":
			return Fireball.Default(controller);
		case "Heal":
			return Heal.Default(controller);
		case "Hyper":
			return Hyper.Default(controller);
		case "Quake":
			return Quake.Default(controller);
		case "Dash":
			return Dash.Default(controller);
		case "Flare":
			return Flare.Default(controller);
		case "Minor Heal":
			return MinorHeal.Default(controller);
		case "Cleave":
			return Cleave.Default(controller);
		default:
			return Slash.Default(controller);
		}
	}

	public abstract int GetID();

	public static string GetTitleFromID(int id){
		switch (id) {
		case 0:
			return "Slash";
		case 1:
			return "Lunge";
		case 2:
			return "Fireball";
		case 3:
			return "Heal";
		case 4:
			return "Hyper";
		case 5:
			return "Quake";
		case 6:
			return "Dash";
		case 7:
			return "Flare";
		case 8:
			return "Minor Heal";
		case 9:
			return "Cleave";
		default:
			return "";
		}
	}

	public static int MinimumWaveFromTier(int tier){
		switch (tier) {
		case 0:
			return 0;
		case 1:
			return 1;
		case 2:
			return 3;
		case 3:
			return 6;
		case 4:
			return 10;
		default:
			return 0;
		}
	}

	public static int GetMaxID(){
		return 9;
	}
}
