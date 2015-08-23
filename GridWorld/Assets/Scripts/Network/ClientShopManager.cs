using UnityEngine;
using System.Collections.Generic;

public class ClientShopManager : MonoBehaviour, ShopButtonHandler{

	public GameObject shop;
	public GameObject shopButton;
	
	public List<GameObject> addedButtons;

	public NetworkPlayerController netPlayer;

	void Start(){
		addedButtons = new List<GameObject>();
		netPlayer = GetComponent<NetworkPlayerController>();
	}

	public void DisplayShop(int[] skills){
		if(skills.Length == 0){
			netPlayer.CmdEndShop(-1);
			return;
		}
		shop.SetActive(true);
		
		for(int i = 0; i < skills.Length; i++){
			//TODO
			Skill skill = Skills.GetDefaultFromSkillInfo(
				Skills.GetSkillInfoFromID(skills[i]), null);
			GameObject obj = Instantiate(shopButton) as GameObject;
			obj.transform.SetParent(shop.transform);
			obj.transform.localPosition = new Vector3(-120 + (120 * i), -20, 0);
			ShopButtonManager manager = obj.GetComponent<ShopButtonManager>();
			manager.AttachHandler(this);
			manager.SetText(skill.GetName(), Skills.Attr(skill.GetID()).id + 1, 
			                skill.GetInfo(), skill.GetBody());
			addedButtons.Add(obj);
		}
	}

	public void ButtonPressed(ShopButtonManager manager){
		foreach(GameObject obj in addedButtons){
			Destroy(obj);
		}
		addedButtons.Clear();
		shop.SetActive(false);
		//TODO sub 1 or not?
		netPlayer.CmdEndShop(manager.GetID() - 1);
		//skill.Present();
		//map.NextWave ();
	}

}
