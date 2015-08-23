using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ServerOnlyScript : NetworkBehaviour{

	public GameObject client;
	public MonoBehaviour[] inits;

	public void Init(){
		foreach(MonoBehaviour i in inits){
			if(i is Initializable){
				(i as Initializable).Init();
			}
		}
	}
}
