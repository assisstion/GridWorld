using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public abstract class EntityCombat : NetworkBehaviour, DamageSource, Initializable{

	/*
	 * Valid keys:
	 * fury
	 * hyper
	 */
	public Dictionary<string, float> effects;


	public ServerOnlyScript server;

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

	public virtual void SetHealth(float value){
		_health = value;
	}
	
	public virtual float GetHealth(){
		return _health;
	}
	
	private float _health;

	public virtual float GetMaxHealth(){
		return _maxHealth;
	}

	protected float _maxHealth = 100;
	protected float baseHealthRegen = 1f; // per second

	public virtual void SetMana(float value){
		_mana = value;
	}
	
	public virtual float GetMana(){
		return _mana;
	}

	private float _mana;

	protected float _maxMana = 100;

	public virtual float GetMaxMana(){
		return _maxMana;
	}

	protected float baseManaRegen = 5f; // per second
	
	public virtual void SetAction(float value){
		_action = value;
	}

	public virtual float GetAction(){
		return _action;
	}

	private float _action;
	protected float maxAction = 3.0f;

	public virtual float GetMaxAction(){
		return maxAction;
	}

	protected bool init;
	
	
	// Use this for initialization
	protected virtual void Start(){

	}

	public virtual void Init(){
		server = GetComponent<ServerOnlyScript>();
		holder = server.client;

		effects = new Dictionary<string, float>();
		liveSkills = new List<SkillEvent>();
		toAdd = new List<SkillEvent>();
		SetHealth(GetMaxHealth());
		SetMana(GetMaxMana());

		//Debug.Log("Entitycombat Init");
		init = true;
	}
	
	// Update is called once per frame
	protected virtual void Update(){
		if(init){
			ActionUpdate();
			SkillEventUpdate();
			Tick();
			EffectCheck();
		}
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
		SetHealth(GetHealth() + delta * baseHealthRegen * HealthRegenMultiplier());
		if(GetHealth() > GetMaxHealth()){
			SetHealth(GetMaxHealth());
		}
		SetMana(GetMana() + delta * baseManaRegen * ManaRegenMultiplier());
		if(GetMana() > GetMaxMana()){
			SetMana(GetMaxMana());
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
		if(effects.ContainsKey("freeze")){
			return;
		}
		float actionSpeed = 1.0f;
		if(effects.ContainsKey("slow")){
			actionSpeed /= 2.0f;
		}
		if(GetAction() > 0){
			SetAction(GetAction() - Time.deltaTime * actionSpeed);
		}
		if(GetAction() < 0){
			SetAction(0);
		}
	}

	public float HealHealth(float healed){
		float temp = GetHealth() + healed;
		if(temp < GetMaxHealth()){
			SetHealth(temp);
			return healed;
		}
		else{
			float temp2 = GetHealth();
			SetHealth(GetMaxHealth());
			return GetHealth() - temp2;
		}
	}
	
	public float TakeDamage(DamageSource source, float dealt){
		if(GetHealth() > dealt){
			SetHealth(GetHealth() - dealt);
			if(source != null){
				source.DamageDealt(this, dealt);
			}
			return dealt;
		}
		else{
			float tempHealth = GetHealth();
			SetHealth(0);
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
		NetworkServer.Destroy(holder);
		Destroy(this.gameObject);
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

	public abstract EntityController Controller();

	public void ActivateSkill(Skill skill){
		SetAction(skill.Activate(Controller()));
		DelayedMod();
	}

	void DelayedMod(){
		if(delayedSetHealth != 0){
			if(delayedSetHealth == 1 || (delayedSetHealth == 2 && GetHealth() > delayedHealthSet) 
				|| (delayedSetHealth == 3 && GetHealth() < delayedHealthSet)){
				HealthMod(delayedHealthSet - GetHealth());
				delayedSetHealth = 0;
				delayedHealthSet = 0;
			}
		}
		if(delayedSetMana != 0){
			if(delayedSetMana == 1 || (delayedSetMana == 2 && GetMana() > delayedManaSet) 
				|| (delayedSetMana == 3 && GetMana() < delayedManaSet)){
				SetMana(delayedManaSet);
				delayedSetMana = 0;
				delayedManaSet = 0;
			}
		}
		if(delayedSetAction != 0){
			if(delayedSetAction == 1 || (delayedSetAction == 2 && GetAction() > delayedActionSet) 
				|| (delayedSetAction == 3 && GetAction() < delayedHealthSet)){
				SetAction(delayedActionSet);
				delayedSetAction = 0;
				delayedActionSet = 0;
			}
		}
		HealthMod(delayedHealthMod);
		SetMana(GetMana() + delayedManaMod);
		SetAction(GetAction() + delayedActionMod);
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
		if(GetAction() != 0 || actionLocked){
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
