using UnityEngine;
using System.Collections;

public abstract class AbstractSkillEvent : SkillEvent {

	protected float startTime;

	public virtual bool Initialize(){
		startTime = Time.time;
		return true;
	}

	public float TimePassed(){
		return Time.time - startTime;
	}

	public abstract bool Update();

	public abstract void CleanUp();

}
