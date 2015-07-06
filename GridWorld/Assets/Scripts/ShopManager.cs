using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour, ShopButtonHandler {

	public SkillManager skill;
	public MapGenerator map;
	public PlayerController player;
	public GameObject shop;
	public GameObject shopButton;
	public List<GameObject> addedButtons;
	int wave;

	public ShopManager(){
		addedButtons = new List<GameObject> ();
	}

	public List<int> CanUnlockSkills(){
		//todo add skill tree
		List<int> list = new List<int> ();
		for (int i = 0; i <= Skill.GetMaxID(); i++) {
			Skill skill = Skill.GetDefaultFromTitle(Skill.GetTitleFromID(i), player);
			if(wave < skill.GetMinimumWave()){
				continue;
			}
			HashSet<string> required = skill.GetPrerequisites();
			foreach(string s in required){
				if(!System.Array.Exists<Skill>(player.combat.skills, (x => x != null && 
				                Skill.GetTitleFromID(x.GetID()) == s))){
					continue;
				}
			}
			list.Add(i);
		}
		return list;
	}

	public void Shop(int wave){
		this.wave = wave;
		shop.SetActive (true);
		player.combat.health = player.combat.maxHealth;
		player.combat.mana = player.combat.maxMana;
		player.combat.action = 0f;
		List<Skill> missing = new List<Skill> ();
		foreach(int i in CanUnlockSkills()){
			if(!System.Array.Exists<Skill>(player.combat.skills, (x => x != null && x.GetID() == i))){
				missing.Add(Skill.GetDefaultFromTitle(Skill.GetTitleFromID(i), player));
			}
		}
		//missing.TrimExcess ();
		if (missing.Count == 0) {
			shop.SetActive (false);
			skill.Present();
			//map.NextWave ();
		} else {
			while(missing.Count > 3){
				missing.RemoveAt(Random.Range(0, missing.Count));
			}
			PresentSkills (missing.ToArray ());
		}
	}
	
	void PresentSkills(Skill[] skills){
		for (int i = 0; i < skills.Length; i++) {
			GameObject obj = Instantiate(shopButton) as GameObject;
			obj.transform.SetParent(shop.transform);
			obj.transform.localPosition = new Vector3(-120 + (120 * i), -20, 0);
			ShopButtonManager manager = obj.GetComponent<ShopButtonManager>();
			manager.AttachHandler(this);
			manager.SetText(skills[i].GetName(), skills[i].GetID()+1, skills[i].GetInfo(), skills[i].GetBody());
			addedButtons.Add(obj);
		}
	}
	
	public void ButtonPressed(ShopButtonManager manager){
		Skill s = (Skill.GetDefaultFromTitle (manager.GetTitle (), player));
		player.combat.AddSkill(s, s.GetID()); 
		foreach (GameObject obj in addedButtons) {
			Destroy(obj);
		}
		addedButtons.Clear ();
		shop.SetActive (false);
		skill.Present ();
		//map.NextWave ();
	}
}
