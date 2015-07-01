using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	public GameObject[] skillButtons;
	public GameObject skillButton;
	public GameObject skillBar;

	int len = 10;

	void Awake(){

	}

	// Use this for initialization
	void Start () {

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
}
