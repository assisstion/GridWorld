using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopButtonManager : MonoBehaviour {

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

	public void SetText(string title, string info, string body){
		titleText.text = title;
		infoText.text = info;
		bodyText.text = body;
	}

	public string GetTitle(){
		return titleText.text;
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
