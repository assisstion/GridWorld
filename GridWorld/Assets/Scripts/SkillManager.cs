using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour {

	public GameObject skillShop;
	public MapGenerator map;
	public PlayerController player;
	public GameObject[] skillButtons;
	public GameObject skillButton;
	public GameObject skillBar;

	public List<GameObject> skillShopButtons;
	public GameObject skillShopButton;
	public GameObject skillShopPanel;

	int len = 10;

	public void Present(){
		skillShop.SetActive (true);
		foreach (Skill skill in player.combat.skillLibrary) {
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
		foreach (GameObject obj in skillShopButtons) {
			Destroy(obj);
		}
		skillShop.SetActive (false);
		map.NextWave ();
	}

	public void Clear(){
		foreach (GameObject button in skillButtons) {
			button.GetComponent<SkillButtonManager>().ClearSkill(player);
		}
	}

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		skillShopButtons = new List<GameObject> ();
	}

	public void Initialize(){
		skillButtons = new GameObject[len];
		for(int i = 0; i < len; i++){
			GameObject obj = Instantiate(skillButton) as GameObject;
			obj.transform.SetParent(skillBar.transform);
			obj.transform.position = new Vector3(50 + 90 * i, 50, 0);
			SkillButtonManager sbm = obj.GetComponent<SkillButtonManager>();
			sbm.Initialize();
			sbm.id = i;
			sbm.Reset();
			skillButtons[i] = obj;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetSkill(Skill s, int index){
		skillButtons [index].GetComponent<SkillButtonManager> ().SetSkill (s);
	}

	public void Dropped(Camera cam, RectTransform t, string text){
		foreach (GameObject button in skillButtons) {

			RectTransform rt = button.GetComponent<RectTransform>();
			Rect rect = new Rect(rt.position.x - rt.rect.width/2, 
			   rt.position.y - rt.rect.height/2, rt.rect.width, rt.rect.height);
			if(rect.Contains(RectTransformUtility.WorldToScreenPoint(cam, t.position))){
				button.GetComponent<SkillButtonManager>().DropSkill(text, player);
				return;
			}

			   
		}
	}
}
