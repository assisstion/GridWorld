using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {

	float speed = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void UpdateLocation(float x, float y, int direction){
		transform.position = new Vector3 (x, y, transform.position.z);
		transform.localRotation = Quaternion.Euler(new Vector3 (90,
			(Direction.Rotation (direction) + 90) % 360, 0));
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
