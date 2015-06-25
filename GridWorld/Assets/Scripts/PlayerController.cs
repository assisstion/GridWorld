using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public PlayerMovement movement;
	public PlayerCombat combat;

	// Use this for initialization
	void Start () {
		movement = this.gameObject.GetComponent<PlayerMovement> ();
		combat = this.gameObject.GetComponent<PlayerCombat> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
