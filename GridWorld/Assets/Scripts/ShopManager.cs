using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour, ShopButtonHandler {

	public MapGenerator map;
	public PlayerController player;
	public GameObject shop;
	public GameObject shopButton;
	public List<GameObject> addedButtons;

	int maxID = 5;

	public ShopManager(){
		addedButtons = new List<GameObject> ();
	}

	public List<int> CanUnlockSkills(){
		//todo add skill tree
		List<int> list = new List<int> ();
		for (int i = 0; i <= maxID; i++) {
			list.Add(i);
		}
		return list;
	}

	public void Shop(){
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
			map.NextWave ();
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
		map.NextWave ();
	}
}
