using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkEnemyController : NetworkBehaviour{

	public GameObject serverEnemy;
	public GameObject serverEnemyPrefab;

	//MapGenerator map;

	// Use this for initialization
	public void Initialize(){
		if(isServer){
			serverEnemy = Instantiate(serverEnemyPrefab) as GameObject;
			serverEnemy.GetComponent<EnemyBaseManager>().holder = this.gameObject;
			ServerOnlyScript sos = serverEnemy.GetComponent<ServerOnlyScript>();
			sos.client = this.gameObject;
			sos.Init();

			//map = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapGenerator>();
		}
	}
	
	// Update is called once per frame
	void Update(){
	
	}
}
