using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour {

	public BarController healthBar;
	public BarController actionBar;
	public BarController manaBar;

	public Skill[] skills;
	volatile bool actionLocked;
	PlayerController controller;
	List<SkillEvent> liveSkills;

	public int health {
		set {
			_health = value;
			healthBar.percent = _health * 100.0f / maxHealth;
		}
		get{
			return _health;
		}
	}
	int _health;
	
	int maxHealth = 100;

	public int mana{
		set{
			_mana = value;
			manaBar.percent = _mana * 100.0f / maxMana;
		}
		get{
			return _mana;
		}
	}
	int _mana;

	int maxMana = 200;

	public float action{
		set{
			_action = value;
			actionBar.percent = _action * 100.0f / maxAction;
		}
		get{
			return _action;
		}
	}
	float _action;

	float maxAction = 3.0f;


	// Use this for initialization
	void Start () {
		liveSkills = new List<SkillEvent> ();
		controller = this.gameObject.GetComponent<PlayerController> ();
		health = maxHealth;
		skills = new Skill[10];
		for (int i = 0; i < 10; i++) {
			Skill tempSkill = new Slash(controller);
			//tempSkill.cooldown = (i+1) * 0.1f;
			skills[i] = tempSkill;
		}
	}
	
	// Update is called once per frame
	void Update () {
		ActionUpdate ();
		SkillEventUpdate ();
		InputCheck ();

	}

	public void ActionUpdate(){
		if (action > 0) {
			action -= Time.deltaTime;
		}
		if (action < 0) {
			action = 0;
		}
	}

	public void InputCheck(){
		if (TryLockAction ()) {
			if(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1)){
				ActivateSkill(0);
			} else if(Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)){
				ActivateSkill(1);
			} else if(Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3)){
				ActivateSkill(2);
			} else if(Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4)){
				ActivateSkill(3);
			} else if(Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Keypad5)){
				ActivateSkill(4);
			} else if(Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Keypad6)){
				ActivateSkill(5);
			} else if(Input.GetKey(KeyCode.Alpha7) || Input.GetKey(KeyCode.Keypad7)){
				ActivateSkill(6);
			} else if(Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Keypad8)){
				ActivateSkill(7);
			} else if(Input.GetKey(KeyCode.Alpha9) || Input.GetKey(KeyCode.Keypad9)){
				ActivateSkill(8);
			} else if(Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Keypad0)){
				ActivateSkill(9);
			}
			UnlockAction();
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
