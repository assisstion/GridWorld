using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CustomNetworkManager : NetworkManager{

	public GameObject gameControllerPrefab;
	public GameObject gameController;

	// Use this for initialization
	void Start(){

	}
	
	// Update is called once per frame
	void Update(){
	
	}

	public override void OnStartServer(){
		base.OnStartServer();
		gameController = Instantiate(gameControllerPrefab) as GameObject;
		NetworkServer.Spawn(gameController);
	}
}
