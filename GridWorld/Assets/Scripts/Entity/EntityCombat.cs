using UnityEngine;
using System.Collections.Generic;

public class EntityCombat : MonoBehaviour, DamageSource{

	/*
	 * Valid keys:
	 * fury
	 * hyper
	 */
	public Dictionary<string, float> effects;


	/*
	 * delayedSet modes:
	 * 0 = false
	 * 1 = true for all circumstances
	 * 2 = true if it would decrease
	 * 3 = true if it would increase
	 */
	public int delayedSetMana;
	public int delayedSetHealth;
	public int delayedSetAction;

	public float delayedManaSet;
	public float delayedHealthSet;
	public float delayedActionSet;

	public float delayedManaMod;
	public float delayedHealthMod;
	public float delayedActionMod;

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
		effects = new Dictionary<string, float>();
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
		EffectCheck();
	}

	public void EffectCheck(){
		HashSet<string> toBeRemoved = new HashSet<string>();
		foreach(KeyValuePair<string, float> pair in effects){
			//Tick(pair.Key, Time.deltaTime);
			if(pair.Value < Time.time){
				toBeRemoved.Add(pair.Key);
			}
		}
		foreach(string s in toBeRemoved){
			//RemoveEffect(s);
			effects.Remove(s);
		}
	}
	
	public void AddEffect(string effect, float duration){
		float time = Time.time + duration; 
		if(effects.ContainsKey(effect)){
			float tx = effects[effect];
			if(tx > time){
				time = tx;
			}
		}
		effects[effect] = time;
	}

	public virtual void Tick(){
		float delta = Time.deltaTime;
		health += delta * baseHealthRegen * HealthRegenMultiplier();
		if(health > maxHealth){
			health = maxHealth;
		}
		mana += delta * baseManaRegen * ManaRegenMultiplier();
		if(mana > maxMana){
			mana = maxMana;
		}
		DelayedMod();
	}

	public virtual float HealthRegenMultiplier(){
		return 1.0f;
	}

	public virtual float ManaRegenMultiplier(){
		float v = 1.0f;
		if(effects.ContainsKey("meditate")){
			v *= 4;
		}
		return v;
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
	
	public float TakeDamage(DamageSource source, float dealt){
		if(health > dealt){
			health -= dealt;
			if(source != null){
				source.DamageDealt(this, dealt);
			}
			return dealt;
		}
		else{
			float tempHealth = health;
			health = 0;
			if(source != null){
				source.DamageDealt(this, tempHealth);
				source.EntityDestroyed(this);
			}
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
		DelayedMod();
	}

	void DelayedMod(){
		if(delayedSetHealth != 0){
			if(delayedSetHealth == 1 || (delayedSetHealth == 2 && health > delayedHealthSet) 
				|| (delayedSetHealth == 3 && health < delayedHealthSet)){
				HealthMod(delayedHealthSet - health);
				delayedSetHealth = 0;
				delayedHealthSet = 0;
			}
		}
		if(delayedSetMana != 0){
			if(delayedSetMana == 1 || (delayedSetMana == 2 && health > delayedManaSet) 
				|| (delayedSetMana == 3 && health < delayedManaSet)){
				mana = delayedManaSet;
				delayedSetMana = 0;
				delayedManaSet = 0;
			}
		}
		if(delayedSetAction != 0){
			if(delayedSetAction == 1 || (delayedSetAction == 2 && health > delayedActionSet) 
				|| (delayedSetAction == 3 && health < delayedHealthSet)){
				action = delayedActionSet;
				delayedSetAction = 0;
				delayedActionSet = 0;
			}
		}
		HealthMod(delayedHealthMod);
		mana += delayedManaMod;
		action += delayedActionMod;
		delayedHealthMod = 0;
		delayedManaMod = 0;
		delayedActionMod = 0;
	}

	void HealthMod(float f){
		if(f > 0){
			HealHealth(f);
		}
		else if(f < 0){
			//todo: no source
			TakeDamage(null, f);
		}
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

	public virtual void DamageDealt(EntityCombat combat, float amt){
		//Do nothing
	}

	public virtual void EntityDestroyed(EntityCombat combat){
		if(effects.ContainsKey("fury")){
			delayedActionSet = 0.5f;
			delayedSetAction = 2;
		}
		if(effects.ContainsKey("bloodlust")){
			delayedHealthMod += 30;
		}
	}
}
