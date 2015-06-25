using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	float speed = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void ManualMovement(){
		
		//Manual Movement
		if(Input.GetKey(KeyCode.W)){
			transform.Translate(Vector3.up * speed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.A)){
			transform.Translate(Vector3.left * speed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.S)){
			transform.Translate(Vector3.down * speed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.D)){
			transform.Translate(Vector3.right * speed * Time.deltaTime);
		}
	}
}
