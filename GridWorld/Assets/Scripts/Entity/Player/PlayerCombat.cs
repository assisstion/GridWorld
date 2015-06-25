using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : EntityCombat {

	public BarController healthBar;
	public BarController actionBar;
	public BarController manaBar;

	public override int health{
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

	public override int mana{
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
		
		for (int i = 0; i < 10; i++) {
			Skill tempSkill = new Slash(controller, 0.5f);
			//tempSkill.cooldown = (i+1) * 0.1f;
			skills[i] = tempSkill;
		}
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
}
