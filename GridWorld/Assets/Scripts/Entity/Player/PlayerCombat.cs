using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : EntityCombat{

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
		base.Start();
		skillLibrary = new List<Skill>();
		controller = this.gameObject.GetComponent<PlayerController>();
		manager.Initialize();
		skills = new Skill[100];
		Skill slash = Slash.Default(controller);
		AddSkill(slash, 0);
		skills[0] = slash;
		manager.SetSkill(slash, 0);
		if(PlayerController.DEBUG){
			//0 already added
			for(int i = 1; i <= Skills.GetMaxID(); i++){
				Skill s = Skills.GetDefaultFromSkillInfo(
					Skills.GetSkillInfoFromID(i), controller);
				AddSkill(s, i); 
			}
		}
	}

	protected override void Update(){
		base.Update();
		InputCheck();
	}

	public void InputCheck(){
		if(TryLockAction()){
			if(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1)){
				ActivateSkill(0);
			}
			else if(Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)){
				ActivateSkill(1);
			}
			else if(Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3)){
				ActivateSkill(2);
			}
			else if(Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4)){
				ActivateSkill(3);
			}
			else if(Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Keypad5)){
				ActivateSkill(4);
			}
			else if(Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Keypad6)){
				ActivateSkill(5);
			}
			else if(Input.GetKey(KeyCode.Alpha7) || Input.GetKey(KeyCode.Keypad7)){
				ActivateSkill(6);
			}
			else if(Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Keypad8)){
				ActivateSkill(7);
			}
			else if(Input.GetKey(KeyCode.Alpha9) || Input.GetKey(KeyCode.Keypad9)){
				ActivateSkill(8);
			}
			else if(Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Keypad0)){
				ActivateSkill(9);
			}
			UnlockAction();
		}
	}

	public void AddSkill(Skill s, int id){
		skillLibrary.Add(s);
	}

	protected override void CleanUp(){
		Application.LoadLevel("MainMenu");
	}
}
