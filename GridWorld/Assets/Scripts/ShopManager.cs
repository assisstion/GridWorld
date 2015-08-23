using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ShopManager : NetworkBehaviour, Initializable{

	//public SkillManager skill;
	public MapGenerator map;
	public PlayerController player;
	int wave;

	public ShopManager(){

	}

	public void Start(){
		//shop.SetActive(false);
		//shop = GetComponentInChildren<ShopPlaceholder>().gameObject;
	}

	public void Init(){
		//
	}

	public List<int> CanUnlockSkills(){
		//todo add skill tree
		List<int> list = new List<int>();
		for(int i = 0; i <= Skills.GetMaxID(); i++){
			Skill skill = Skills.GetDefaultFromSkillInfo(Skills.GetSkillInfoFromID(i), player);
			if(wave < skill.GetMinimumWave()){
				continue;
			}
			bool can = true;
			HashSet<string> required = skill.GetPrerequisites();
			foreach(string s in required){
				if(!player.combat.skillLibrary.Exists(x => x != null && 
					Skills.Attr(x.GetID()).title == s)){
					can = false;
					break;
				}
			}
			if(can){
				list.Add(i);
			}
		}
		return list;
	}

	public void Shop(int wave){
		this.wave = wave;
		player.combat.SetHealth(player.combat.GetMaxHealth());
		player.combat.SetMana(player.combat.GetMaxMana());
		player.combat.SetAction(0f);
		List<Skill> missing = new List<Skill>();
		foreach(int i in CanUnlockSkills()){
			if(!player.combat.skillLibrary.Exists(x => x != null && Skills.Attr(x.GetID()).id == i)){
				missing.Add(Skills.GetDefaultFromSkillInfo(Skills.GetSkillInfoFromID(i), player));
			}
		}
		NetworkPlayerController netPlayer = player.combat.server.client
			.GetComponent<NetworkPlayerController>();
		//missing.TrimExcess ();
		if(missing.Count == 0){
			DisplaySkills(netPlayer);
			//map.NextWave ();
		}
		else{
			while(missing.Count > 3){
				missing.RemoveAt(Random.Range(0, missing.Count));
			}
			int[] ia = new int[missing.Count];
			for(int i = 0; i < missing.Count; i++){
				ia[i] = Skills.Attr(missing[i].GetID()).id;
			}
			netPlayer.RpcDisplayShop(ia);

			//PresentSkills(missing.ToArray());
		}
	}
	
	/*void PresentSkills(Skill[] skills){
		for(int i = 0; i < skills.Length; i++){
			GameObject obj = Instantiate(shopButton) as GameObject;
			obj.transform.SetParent(shop.transform);
			obj.transform.localPosition = new Vector3(-120 + (120 * i), -20, 0);
			ShopButtonManager manager = obj.GetComponent<ShopButtonManager>();
			manager.AttachHandler(this);
			manager.SetText(skills[i].GetName(), Skills.Attr(skills[i].GetID()).id + 1, skills[i].GetInfo(), skills[i].GetBody());
			addedButtons.Add(obj);
		}
	}*/

	public void AddSkill(int i){
		Skill s = (Skills.GetDefaultFromSkillInfo(Skills.GetSkillInfoFromID(i), player));
		player.combat.AddSkill(s, Skills.Attr(s.GetID()).id); 

		NetworkPlayerController netPlayer = player.combat.server.client
			.GetComponent<NetworkPlayerController>();
		DisplaySkills(netPlayer);
	}

	void DisplaySkills(NetworkPlayerController netPlayer){
		Skill[] sa = player.combat.skills;
		int[] ia = new int[sa.Length];
		for(int i = 0; i < sa.Length; i++){
			ia[i] = Skills.Attr(sa[i].GetID()).id;
		}
		netPlayer.RpcDisplaySkills(ia);
	}
}
