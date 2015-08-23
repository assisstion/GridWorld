using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ClientSkillManager : MonoBehaviour{

	int len = 4;

	public GameObject skillShop;

	public GameObject[] skillButtons;
	public GameObject skillButton;
	public GameObject skillBar;
	public List<GameObject> skillShopButtons;
	public GameObject skillShopButton;
	public GameObject skillShopPanel;

	public NetworkPlayerController netPlayer;
	bool init;
	
	// Use this for initialization
	void Start(){
		if(!init){
			Initialize();
		}
	}

	public void DisplaySkills(int[] skills){
		skillShop.SetActive(true);
		foreach(int i in skills){
			Skill skill = Skills.GetDefaultFromSkillInfo(Skills.GetSkillInfoFromID(i), null);
			GameObject obj = Instantiate(skillShopButton) as GameObject;
			obj.transform.SetParent(skillShopPanel.transform);
			SkillPageButtonManager button = obj.GetComponent<SkillPageButtonManager>();
			obj.GetComponentInChildren<Text>().text = skill.GetName();
			button.text = skill.GetName();
			button.manager = this;
			skillShopButtons.Add(obj);
		}
	}

	public void Close(){
		foreach(GameObject obj in skillShopButtons){
			Destroy(obj);
		}
		skillShop.SetActive(false);
		netPlayer.CmdWaveReady();
	}
	
	public void Clear(){
		netPlayer.CmdClearSkills();
		foreach(GameObject button in skillButtons){
			button.GetComponent<SkillButtonManager>().ClearSkill();
		}
	}
	
	public void Initialize(){
		init = true;
		skillShopButtons = new List<GameObject>();
		netPlayer = GetComponent<NetworkPlayerController>();
		skillButtons = new GameObject[len];
		for(int i = 0; i < len; i++){
			GameObject obj = Instantiate(skillButton) as GameObject;
			obj.transform.SetParent(skillBar.transform);
			obj.transform.localScale = new Vector3(1, 1, 1);
			obj.transform.localPosition = new Vector3(50 + 90 * i, 0, 0);
			//Debug.Log(obj.transform.localPosition.x);
			SkillButtonManager sbm = obj.GetComponent<SkillButtonManager>();
			sbm.Initialize(netPlayer);
			sbm.id = i;
			sbm.Reset();
			skillButtons[i] = obj;
		}
	}
	
	public void SetSkill(Skill s, int index){
		if(!init){
			Initialize();
		}
		skillButtons[index].GetComponent<SkillButtonManager>().SetSkill(s);
	}
	
	public void Dropped(Camera cam, RectTransform t, string text){
		foreach(GameObject button in skillButtons){
			
			RectTransform rt = button.GetComponent<RectTransform>();
			Rect rect = new Rect(rt.position.x - rt.rect.width / 2, 
			                     rt.position.y - rt.rect.height / 2, rt.rect.width, rt.rect.height);
			if(rect.Contains(RectTransformUtility.WorldToScreenPoint(cam, t.position))){
				button.GetComponent<SkillButtonManager>().DropSkill(text);
				return;
			}
			
			
		}
	}
}
