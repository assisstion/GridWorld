using UnityEngine;
using System.Collections.Generic;

public class EntityCombat : MonoBehaviour {
	
	public Skill[] skills;
	volatile bool actionLocked;
	List<SkillEvent> liveSkills;
	
	public virtual int health {
		set {
			_health = value;
		}
		get{
			return _health;
		}
	}
	protected int _health;
	
	protected int maxHealth = 100;
	
	public virtual int mana{
		set{
			_mana = value;
		}
		get{
			return _mana;
		}
	}
	protected int _mana;
	
	protected int maxMana = 200;
	
	public virtual float action{
		set{
			_action = value;
		}
		get{
			return _action;
		}
	}
	protected float _action;
	
	protected float maxAction = 3.0f;
	
	
	// Use this for initialization
	protected virtual void Start () {
		liveSkills = new List<SkillEvent> ();
		health = maxHealth;
		skills = new Skill[10];
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		ActionUpdate ();
		SkillEventUpdate ();
		
	}
	
	public void ActionUpdate(){
		if (action > 0) {
			action -= Time.deltaTime;
		}
		if (action < 0) {
			action = 0;
		}
	}
	
	public int TakeDamage(int dealt){
		if (health > dealt) {
			health -= dealt;
			return dealt;
		} else {
			int tempHealth = health;
			health = 0;
			return tempHealth;
		}
	}
	
	public void ActivateSkill(int button){
		action = skills [button].Activate ();
	}
	
	public bool TryLockAction(){
		if(action != 0 || actionLocked){
			return false;
		}
		actionLocked = true;
		return true;
	}
	
	public void UnlockAction(){
		actionLocked = false;
	}
	
	public bool ActivateAnimation(SkillEvent skill){
		if (skill.Initialize ()) {
			liveSkills.Add (skill);
			return true;
		} else {
			return false;
		}
	}
	
	public void SkillEventUpdate(){
		List<SkillEvent> toBeRemoved = new List<SkillEvent> ();
		foreach (SkillEvent sEvent in liveSkills){
			if(!sEvent.Update()){
				sEvent.CleanUp();
				toBeRemoved.Add(sEvent);
			}
		}
		foreach (SkillEvent sEvent in toBeRemoved) {
			liveSkills.Remove(sEvent);
		}
		toBeRemoved.Clear ();
	}
}
