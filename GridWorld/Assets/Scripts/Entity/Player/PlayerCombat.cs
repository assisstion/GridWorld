using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : EntityCombat {

	public List<Skill> skillLibrary;
	public BarController healthBar;
	public BarController actionBar;
	public BarController manaBar;
	public SkillManager manager;

	public override float health{
		set{
			base.health = value;
			healthBar.percent = _health * 100.0f / maxHealth;
		}
		get{
			return base.health;
		}
	}

	public override float action{
		set{
			base.action = value;
			actionBar.percent = _action * 100.0f / maxAction;
		}
		get{
			return base.action;
		}
	}

	public override float mana{
		set{
			base.mana = value;
			manaBar.percent = _mana * 100.0f / maxMana;
		}
		get{
			return base.mana;
		}
	}

	
	PlayerController controller;

	protected override void Start(){
		base.Start ();
		skillLibrary = new List<Skill> ();
		controller = this.gameObject.GetComponent<PlayerController> ();
		manager.Initialize ();
		skills = new Skill[100];
		Skill slash = Slash.Default (controller);
		AddSkill (slash, 0);
		skills [0] = slash;
		manager.SetSkill (slash, 0);
		/*for (int i = 0; i < 10; i++) {
			Skill tempSkill;
			switch(i){
			case 1:
				tempSkill = Lunge.Default(controller);
				break;
			case 2:
				tempSkill = Fireball.Default(controller);
				break;
			case 3:
				tempSkill = Heal.Default(controller);
				break;
			case 4:
				tempSkill = Hyper.Default(controller);
				break;
			case 5:
				tempSkill = Quake.Default(controller);
				break;
			default:
				tempSkill = Slash.Default(controller);
				break;
				//tempSkill.cooldown = (i+1) * 0.1f;
			}
			skills[i] = tempSkill;
		}*/
	}

	protected override void Update(){
		base.Update ();
		InputCheck ();
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

	public void AddSkill(Skill s, int id){
		/*int index = id;
		while (skills[index] != null) {
			index ++;
			if(index >= skills.Length){
				Debug.Log("No more space for skill");
				break;
			}
		}
		skills [index] = s;
		manager.SetSkill (s, index);
		*/
		skillLibrary.Add (s);
	}

	protected override void CleanUp(){
		Application.LoadLevel ("MainMenu");
	}
}
