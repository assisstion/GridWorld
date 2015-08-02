using UnityEngine;
using System.Collections.Generic;

public class EntityCombat : MonoBehaviour{
	
	public GameObject holder;
	public Skill[] skills;
	volatile bool actionLocked;
	List<SkillEvent> liveSkills;
	List<SkillEvent> toAdd;
	
	public virtual float health{
		set{
			_health = value;
		}
		get{
			return _health;
		}
	}

	protected float _health;

	public float maxHealth{
		get{
			return _maxHealth;
		}
	}

	protected float _maxHealth = 100;
	protected float baseHealthRegen = 1f; // per second
	
	public virtual float mana{
		set{
			_mana = value;
		}
		get{
			return _mana;
		}
	}

	protected float _mana;

	public float maxMana{
		get{
			return _maxMana;
		}
	}

	protected float _maxMana = 100;
	protected float baseManaRegen = 5f; // per second
	
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
	protected virtual void Start(){
		liveSkills = new List<SkillEvent>();
		toAdd = new List<SkillEvent>();
		health = maxHealth;
		mana = maxMana;
	}
	
	// Update is called once per frame
	protected virtual void Update(){
		ActionUpdate();
		SkillEventUpdate();
		Tick();
	}

	public virtual void Tick(){
		float delta = Time.deltaTime;
		health += delta * baseHealthRegen;
		if(health > maxHealth){
			health = maxHealth;
		}
		mana += delta * baseManaRegen;
		if(mana > maxMana){
			mana = maxMana;
		}
	}
	
	public virtual void ActionUpdate(){
		if(action > 0){
			action -= Time.deltaTime;
		}
		if(action < 0){
			action = 0;
		}
	}

	public float HealHealth(float healed){
		float temp = health + healed;
		if(temp < maxHealth){
			health = temp;
			return healed;
		}
		else{
			float temp2 = health;
			health = maxHealth;
			return health - temp2;
		}
	}
	
	public float TakeDamage(float dealt){
		if(health > dealt){
			health -= dealt;
			return dealt;
		}
		else{
			float tempHealth = health;
			health = 0;
			Remove();
			return tempHealth;
		}
	}
	
	protected void Remove(){
		foreach(SkillEvent sEvent in liveSkills){
			sEvent.CleanUp();
		}
		CleanUp();
		GameObject.Destroy(holder);
	}

	protected virtual void CleanUp(){

	}
	
	public void ActivateSkill(int button){
		Skill skill = skills[button];
		if(skill == null){
			return;
		}
		ActivateSkill(skill);
	}

	public void ActivateSkill(Skill skill){
		action = skill.Activate();
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
		if(skill.Initialize()){
			AddSkillEvent(skill);
			return true;
		}
		else{
			return false;
		}
	}
	
	public void SkillEventUpdate(){
		List<SkillEvent> toBeRemoved = new List<SkillEvent>();
		foreach(SkillEvent sEvent in liveSkills){
			if(!sEvent.Update()){
				sEvent.CleanUp();
				toBeRemoved.Add(sEvent);
			}
		}
		foreach(SkillEvent sEvent in toBeRemoved){
			liveSkills.Remove(sEvent);
		}
		toBeRemoved.Clear();
		foreach(SkillEvent sEvent in toAdd){
			liveSkills.Add(sEvent);
		}
		toAdd.Clear();
	}

	public void AddSkillEvent(SkillEvent skill){
		toAdd.Add(skill);
	}
}
