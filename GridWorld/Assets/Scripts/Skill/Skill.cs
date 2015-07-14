using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

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

	public class SkillAttribute : Attribute
	{
		internal SkillAttribute(int id, String title, Type type)
		{
			this.id = id;
			this.title = title;
			this.type = type;
		}
		public string title { get; private set; }
		public int id { get; private set; }
		public Type type { get; private set; }
	}

	public enum SkillDB{
		[SkillAttribute(0, "Slash", typeof(Slash))] Slash,
		[SkillAttribute(1, "Lunge", typeof(Lunge))] Lunge,
		[SkillAttribute(2, "Fireball", typeof(Fireball))] Fireball,
		[SkillAttribute(3, "Heal", typeof(Heal))] Heal,
		[SkillAttribute(4, "Hyper", typeof(Hyper))] Hyper,
		[SkillAttribute(5, "Quake", typeof(Quake))] Quake,
		[SkillAttribute(6, "Dash", typeof(Dash))] Dash,
		[SkillAttribute(7, "Flare", typeof(Flare))] Flare,
		[SkillAttribute(8, "Minor Heal", typeof(MinorHeal))] MinorHeal,
		[SkillAttribute(9, "Cleave", typeof(Cleave))] Cleave
	}
	
	public static int GetMaxID(){
		return 9;
	}
	
	public static SkillDB GetSkillFromID(int id){
		return GetSkillFrom(x => Skill.Attr(x).id == id);
	}

	public static SkillDB GetSkillFromTitle(string title){
		return GetSkillFrom(x => Skill.Attr(x).title == title);
	}

	public static SkillAttribute Attr(SkillDB db){
		return EnumExtensions.GetAttribute<SkillAttribute>(db);
	}

	public static SkillDB GetSkillFrom(Predicate<SkillDB> pred){

		foreach (object obj in System.Enum.GetValues(typeof(SkillDB))) {
			SkillDB db = (SkillDB)obj;
			if(pred.Invoke(db)){
				return db;
			}
		}
		throw new Exception ("Cannot find skill with the given predicate");
	}

	public static Skill GetDefaultFromID(SkillDB db, PlayerController controller){
		return Attr (db).type.GetMethod("Default")
			.Invoke(null,new object[]{controller}) as Skill;
	}

	public abstract SkillDB GetID();

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
}
