using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopButtonManager : MonoBehaviour {

	string name;
	int id;

	public Text titleText;
	public Text infoText;
	public Text bodyText;

	public ShopButtonHandler handler;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetText(string name, int id, string info, string body){
		this.name = name;
		this.id = id;
		titleText.text = Format(name, id);
		infoText.text = info;
		bodyText.text = body;
	}

	static string Format(string name, int id){
		return name + " (" + id + ")";
	}

	public string GetTitle(){
		return name;
	}

	public int GetID(){
		return id;
	}

	public string GetInfo(){
		return infoText.text;
	}

	public string GetBody(){
		return bodyText.text;
	}

	public void AttachHandler(ShopButtonHandler handler){
		this.handler = handler;
	}

	public void ActionPerformed(){
		handler.ButtonPressed (this);
	}
}
