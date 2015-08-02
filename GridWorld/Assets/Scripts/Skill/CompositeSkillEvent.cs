using UnityEngine;
using System.Collections.Generic;

public class CompositeSkillEvent : SkillEvent{

	protected float startTime;
	protected List<SkillEvent> events;
	int currIndex;
	SkillEvent currEvent;
	float currTimePassed;
	EntityController controller;
	float cooldown;

	public CompositeSkillEvent(EntityController control, List<SkillEvent> events){
		this.events = events;
		controller = control;
		cooldown = 0;
		foreach(SkillEvent ev in events){
			cooldown += ev.GetCoolDown();
		}
	}

	public bool Initialize(){
		currIndex = -1;
		startTime = Time.time;
		return GoNextEvent();
	}

	public bool Update(){
		if(TimePassed() - currTimePassed >= currEvent.GetCoolDown()){
			if(!GoNextEvent()){
				return false;
			}
		}
		return true;
	}

	public void CleanUp(){

	}

	bool GoNextEvent(){
		bool pass = false;
		while(!pass){
			currIndex++;
			if(currIndex >= events.Count){
				return false;
			}
			currEvent = events[currIndex];
			pass = SetupEvent(currEvent);
		}
		return true;
	}

	bool SetupEvent(SkillEvent ev){
		if(controller.combat.ActivateAnimation(ev)){
			currTimePassed = TimePassed();
			return true;
		}
		else{
			return false;
		}
	}

	public float TimePassed(){
		return Time.time - startTime;
	}

	public float GetCoolDown(){
		return cooldown;
	}

	public float GetActionModifier(){
		float acMod = 0;
		foreach(SkillEvent ev in events){
			acMod += ev.GetActionModifier();
		}
		return acMod;
	}
}
