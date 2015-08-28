using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : EntityCombat, Initializable{

	public NetworkPlayerController netPlayer;

	public List<Skill> skillLibrary;
	//public BarController healthBar;
	//public BarController actionBar;
	//public BarController manaBar;
	//public SkillManager manager;
	public ShopManager shop;

	public override EntityController Controller(){
		return GetComponent<PlayerController>();
	}

	public override void SetHealth(float value){
		base.SetHealth(value);
		netPlayer.health = GetHealth();
		if(netPlayer.maxHealth != GetMaxHealth()){
			netPlayer.maxHealth = GetMaxHealth();
		}
	}

	public override void SetMana(float value){
		base.SetMana(value);
		netPlayer.mana = GetMana();
		if(netPlayer.maxMana != GetMaxMana()){
			netPlayer.maxMana = GetMaxMana();
		}
	}

	public override void SetAction(float value){
		base.SetAction(value);
		netPlayer.action = GetAction();
		if(netPlayer.maxAction != GetMaxAction()){
			netPlayer.maxAction = GetMaxAction();
		}
	}

	
	//PlayerController controller;

	protected override void Start(){
		//
	}

	public override void Init(){

		netPlayer = GetComponent<ServerOnlyScript>().client.GetComponent<NetworkPlayerController>();

		base.Init();


		skillLibrary = new List<Skill>();
		//controller = this.gameObject.GetComponent<PlayerController>();
		
		MapGenerator map = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapGenerator>();
		
		//MP map
		//manager.map = map;
		shop.map = map;
		
		
		//manager.Initialize();
		skills = new Skill[100];
		Skill slash = Slash.Default();
		AddSkill(slash, 0);
		skills[0] = slash;

		netPlayer.RpcSetSkill(0, 0);

		//manager.SetSkill(slash, 0);
		if(PlayerController.DEBUG){
			//0 already added
			for(int i = 1; i <= Skills.GetMaxID(); i++){
				Skill s = Skills.GetDefaultFromSkillInfo(
					Skills.GetSkillInfoFromID(i));
				AddSkill(s, i); 
			}
		}

	}

	protected override void Update(){
		if(init){
			base.Update();
		}
	}

	public void AddSkill(Skill s, int id){
		skillLibrary.Add(s);
	}

	protected override void CleanUp(){
		Application.LoadLevel("MainMenu");
	}
}
