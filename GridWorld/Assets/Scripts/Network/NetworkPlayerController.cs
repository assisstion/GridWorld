using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour{

	private float oldAction;
	private float actionExtrpl;

	[SyncVar]
	public float
		mana;
	[SyncVar]
	public float
		action;
	[SyncVar]
	public float
		health;

	[SyncVar]
	public float
		maxMana;
	[SyncVar]
	public float
		maxAction;
	[SyncVar]
	public float
		maxHealth;

	public NetworkMovement movement;

	public BarController healthBar;
	public BarController manaBar;
	public BarController actionBar;

	public GameObject serverPlayerPrefab;
	GameObject serverPlayer;

	public ClientShopManager shop;
	public ClientSkillManager skill;

	public MapGenerator map;


	// Use this for initialization
	void Start(){
		//skill = GetComponent<ClientSkillManager>();
		//shop = GetComponent<ClientShopManager>();
		if(isLocalPlayer){
			GetComponentInChildren<Camera>().enabled = true;
			GetComponentInChildren<Canvas>().enabled = true;
		}
		if(isServer){
			serverPlayer = Instantiate(serverPlayerPrefab) as GameObject;
			ServerOnlyScript sos = serverPlayer.GetComponent<ServerOnlyScript>();
			sos.client = this.gameObject;
			sos.Init();
			map = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapGenerator>();
		}
	}
	
	// Update is called once per frame
	void Update(){
		if(isLocalPlayer){
			InputCheck();
			MovementCheck();
			manaBar.percent = mana * 100.0f / maxMana;
			healthBar.percent = health * 100.0f / maxHealth;
			actionBar.percent = action * 100.0f / maxAction;
			if(action != oldAction){
				oldAction = action;
				actionExtrpl = action;
			}
			else{
				actionExtrpl -= Time.deltaTime;
			}
		}
	}
	
	public void InputCheck(){
		//if(TryLockAction()){
		if(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1)){
			CmdActivateSkill(0);
		}
		else if(Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)){
			CmdActivateSkill(1);
		}
		else if(Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3)){
			CmdActivateSkill(2);
		}
		else if(Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4)){
			CmdActivateSkill(3);
		}
		else if(Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Keypad5)){
			CmdActivateSkill(4);
		}
		else if(Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Keypad6)){
			CmdActivateSkill(5);
		}
		else if(Input.GetKey(KeyCode.Alpha7) || Input.GetKey(KeyCode.Keypad7)){
			CmdActivateSkill(6);
		}
		else if(Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Keypad8)){
			CmdActivateSkill(7);
		}
		else if(Input.GetKey(KeyCode.Alpha9) || Input.GetKey(KeyCode.Keypad9)){
			CmdActivateSkill(8);
		}
		else if(Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Keypad0)){
			CmdActivateSkill(9);
		}
		//	UnlockAction();
		//}
	}

	public void MovementCheck(){
		//if(controller.combat != null && controller.combat.TryLockAction()){
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
			CmdGoTowards(Direction.up);
		}
		else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
			CmdGoTowards(Direction.left);
		}
		else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
			CmdGoTowards(Direction.down);
		}
		else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
			CmdGoTowards(Direction.right);
		}
		//controller.combat.UnlockAction();
		//}
	}

	[Command]
	public void CmdActivateSkill(int id){
		PlayerCombat combat = serverPlayer.GetComponent<PlayerCombat>();
		if(combat.TryLockAction()){
			combat.ActivateSkill(id);
			combat.UnlockAction();
		}
	}

	[Command]
	void CmdGoTowards(int direction){
		PlayerCombat combat = serverPlayer.GetComponent<PlayerCombat>();
		//GetComponentInChildren<PlayerCombat>();
		PlayerMovement movement = serverPlayer.GetComponent<PlayerMovement>();
		if(combat != null && combat.TryLockAction()){
			movement.GoTowards(direction);
			combat.UnlockAction();
		}
	}

	[ClientRpc]
	public void RpcDisplayShop(int[] skills){
		if(!isLocalPlayer){
			return;
		}
		shop.DisplayShop(skills);
	}

	[Command]
	public void CmdEndShop(int i){
		ShopManager sm = serverPlayer.GetComponent<ShopManager>();
		sm.AddSkill(i);
	}

	[ClientRpc]
	public void RpcDisplaySkills(int[] skills){
		if(!isLocalPlayer){
			return;
		}
		skill.DisplaySkills(skills);
	}

	[ClientRpc]
	public void RpcSetSkill(int id, int index){
		if(!isLocalPlayer){
			return;
		}
		skill.SetSkill(Skills.GetDefaultFromSkillInfo(
			Skills.GetSkillInfoFromID(id), null), index);
	}

	[Command]
	public void CmdWaveReady(){
		//TODO implement multiplayer waiting
		map.NextWave();
	}

	[Command]
	public void CmdSetSkill(int skill, int index){
		PlayerController controller = serverPlayer.GetComponent<PlayerController>();
		PlayerCombat combat = serverPlayer.GetComponent<PlayerCombat>();
		combat.skills[index] = Skills.GetDefaultFromSkillInfo(
			Skills.GetSkillInfoFromID(skill), controller);
	}

	[Command]
	public void CmdClearSkills(){
		PlayerCombat combat = serverPlayer.GetComponent<PlayerCombat>();
		for(int i = 0; i < combat.skills.Length; i++){
			combat.skills[i] = null;
		}
	}

	/*[ClientRpc]
	public void RpcUpdateCamera(){
		if(!isLocalPlayer){
			return;
		}
		CameraController cam = GetComponentInChildren<CameraController>();
		cam.UpdateLocation(cam.player.transform.position.x, cam.player.transform.position.y);
	}*/
	/*
	[ClientRpc]
	public void RpcGenerateMap(int width, int height, byte[,] data, float gs){
		Debug.Log(width);
		if(!isLocalPlayer){
			return;
		}
		GameObject.FindGameObjectWithTag("CGameController")
			.GetComponent<ClientMapController>().LocalGenerateMap(width, height, data, gs);
	}*/
	[ClientRpc]
	public void RpcGenerateMap(){
		if(!isLocalPlayer){
			return;
		}
		GameObject.FindGameObjectWithTag("CGameController")
			.GetComponent<ClientMapController>().LocalGenerateMap();
	}
}
