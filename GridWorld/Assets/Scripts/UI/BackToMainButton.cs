using UnityEngine;
using System.Collections;

public class BackToMainButton : MonoBehaviour{

	// Use this for initialization
	void Start(){
	
	}
	
	// Update is called once per frame
	void Update(){
	
	}

	public void ChangeMenu(){
		Application.LoadLevel("MainMenu");
	}
}
