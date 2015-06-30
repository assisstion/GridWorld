using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

	public MapGenerator map;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerID){
		GameObject obj = GameObject.Instantiate(playerPrefab, new Vector3(0, 0, -1), 
		     Quaternion.Euler(new Vector3(270, 0, 0))) as GameObject;
		PlayerController pc = obj.GetComponent<PlayerController> ();
		pc.Initialize (map);
		NetworkServer.AddPlayerForConnection (conn, obj, playerControllerID);
	}
}
