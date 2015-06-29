using UnityEngine;
using System.Collections;

public class ServerSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Network.incomingPassword = "password";
		bool useNat = !Network.HavePublicAddress ();
		Network.InitializeServer (4, 25000, useNat);
		MasterServer.RegisterHost ("test1", "test2", "test3");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnServerInitialized(){
		Debug.Log ("Server on");
	}

	public void LookForServers(){
		Debug.Log ("Searching...");
		MasterServer.RequestHostList ("test1");
	}

	bool canJoinServer;

	void OnMasterServerEvent(MasterServerEvent mse){
		HostData[] hostList = MasterServer.PollHostList();
		if (hostList != null) {
			for (int i = 0; i < hostList.Length; i++) {
				Debug.Log (hostList [i].gameName);
			}
			canJoinServer = true;
		} else {
			Debug.Log("No hosts");
		}
	}

	void OnConnectedServer(){
		Debug.Log ("Joined server");
	}
}
