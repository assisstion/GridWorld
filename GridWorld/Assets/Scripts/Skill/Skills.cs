using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;


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

public enum SkillInfo{
	[SkillAttribute(-1, "None", null)] None,
	[SkillAttribute(0, "Slash", typeof(Slash))] Slash,
	[SkillAttribute(1, "Lunge", typeof(Lunge))] Lunge,
	[SkillAttribute(2, "Fireball", typeof(Fireball))] Fireball,
	[SkillAttribute(3, "Heal", typeof(Heal))] Heal,
	[SkillAttribute(4, "Hyper", typeof(Hyper))] Hyper,
	[SkillAttribute(5, "Quake", typeof(Quake))] Quake,
	[SkillAttribute(6, "Dash", typeof(Dash))] Dash,
	[SkillAttribute(7, "Flare", typeof(Flare))] Flare,
	[SkillAttribute(8, "Minor Heal", typeof(MinorHeal))] MinorHeal,
	[SkillAttribute(9, "Cleave", typeof(Cleave))] Cleave,
	[SkillAttribute(10, "Magic Bolt", typeof(MagicBolt))] MagicBolt
}

public class Skills{

	public static int GetMaxID(){
		return 10;
	}
	
	public static SkillInfo GetSkillInfoFromID(int id){
		return GetSkillInfoFrom(x => Skills.Attr(x).id == id);
	}

	public static SkillInfo GetSkillInfoFromTitle(string title){
		return GetSkillInfoFrom(x => Skills.Attr(x).title == title);
	}

	public static SkillAttribute Attr(SkillInfo db){
		return EnumExtensions.GetAttribute<SkillAttribute>(db);
	}

	public static SkillInfo GetSkillInfoFrom(Predicate<SkillInfo> pred){

		foreach (object obj in System.Enum.GetValues(typeof(SkillInfo))) {
			SkillInfo db = (SkillInfo)obj;
			if(pred.Invoke(db)){
				return db;
			}
		}
		throw new Exception ("Cannot find skill with the given predicate");
	}

	public static Skill GetDefaultFromSkillInfo(SkillInfo db, PlayerController controller){
		return Attr (db).type.GetMethod("Default")
			.Invoke(null,new object[]{controller}) as Skill;
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
}
