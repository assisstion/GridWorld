﻿using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : EntityCombat {

	public BarController healthBar;
	public BarController actionBar;
	public BarController manaBar;

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
		
		controller = this.gameObject.GetComponent<PlayerController> ();
		skills = new Skill[10];
		for (int i = 0; i < 10; i++) {
			Skill tempSkill;
			switch(i){
			case 1:
				tempSkill = Lunge.Default(controller);
				break;
			case 2:
				tempSkill = Fireball.Default(controller);
				break;
			default:
				tempSkill = Slash.Default(controller);
				break;
				//tempSkill.cooldown = (i+1) * 0.1f;
			}
			skills[i] = tempSkill;
		}
	}

	protected override void Update(){
		base.Update ();
		InputCheck ();
	}

	
	public void InputCheck(){
		if (!isLocalPlayer) {
			return;
		}
		if (TryLockAction ()) {
			if(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1)){
				CmdActivateSkill(0);
			} else if(Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)){
				CmdActivateSkill(1);
			} else if(Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3)){
				CmdActivateSkill(2);
			} else if(Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4)){
				CmdActivateSkill(3);
			} else if(Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Keypad5)){
				CmdActivateSkill(4);
			} else if(Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Keypad6)){
				CmdActivateSkill(5);
			} else if(Input.GetKey(KeyCode.Alpha7) || Input.GetKey(KeyCode.Keypad7)){
				CmdActivateSkill(6);
			} else if(Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Keypad8)){
				CmdActivateSkill(7);
			} else if(Input.GetKey(KeyCode.Alpha9) || Input.GetKey(KeyCode.Keypad9)){
				CmdActivateSkill(8);
			} else if(Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Keypad0)){
				CmdActivateSkill(9);
			}
			UnlockAction();
		}
	}
}
