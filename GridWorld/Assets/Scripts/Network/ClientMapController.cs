using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ClientMapController : NetworkBehaviour{

	bool init;

	[SyncVar]
	SyncListUInt
		serverData = new SyncListUInt();
	[SyncVar]
	int
		serverWidth;
	[SyncVar]
	int
		serverHeight;
	//byte[,] serverData;
	[SyncVar]
	float
		serverGs;

	float gridSize;

	public GameObject tile;
	public GameObject[,] tiles;
	public Material rockMaterial;
	public Material grassMaterial;
	public Material swampMaterial;
	public Material bgMaterial;
	const float scaleConst = 0.1f;
	public float spacing = 0.01f;
	Vector3 origin = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start(){

	}
	
	// Update is called once per frame
	void Update(){
	
	}

	public void GenerateMap(int width, int height, byte[,] data, float gs){
		this.serverWidth = width;
		this.serverHeight = height;
		//serverData;
		DataToList(width, height, data);
		//this.serverData = data;
		this.serverGs = gs;
	}

	void DataToList(int width, int height, byte[,] data){
		for(int i = 0; i < width * height;){
			byte b1 = data[i % width, i / width];
			i++;
			byte b2 = data[i % width, i / width];
			i++;
			byte b3 = data[i % width, i / width];
			i++;
			byte b4 = data[i % width, i / width];
			i++;
			uint u = ((uint)b1 << 24) ^ ((uint)b2 << 16) ^ ((uint)b3 << 8) ^ ((uint)b4 << 0);
			serverData.Add(u);
		}
	}

	byte[,] ListToData(int width, int height){
		int x = 0;
		int y = 0;

		byte[,] data = new byte[width, height];
		foreach(uint u in serverData){
			for(int s = 24; s >= 0; s -= 8){
				byte b1 = (byte)(u >> s);
				data[x, y] = b1;
				x++;
				if(x >= width){
					x = 0;
					y++;
				}
			}
		}
		return data;
	}

	/*public void Generate(NetworkPlayerController npc){
		Debug.Log("RPC trying");
		Debug.Log(this.hasAuthority);
		Debug.Log(this.isServer);
		Debug.Log(this.isClient);
		RpcGenerateMap();
		//npc.RpcGenerateMap(serverWidth, serverHeight, serverData, serverGs);
	}*/

	/*[ClientRpc]
	public void RpcGenerateMap(){
		//Debug.Log("RPC success");
		//if(!isLocalPlayer){
		//	return;
		//}
		GameObject.FindGameObjectWithTag("CGameController")
			.GetComponent<ClientMapController>().LocalGenerateMap();
	}*/

	[ClientRpc]
	public void RpcTest(){
		Debug.Log("Rpc works");
	}

	//public override 

	public void LocalGenerateMap(){
		LocalGenerateMap(serverWidth, serverHeight, ListToData(serverWidth, serverHeight), serverGs);
	}

	//[ClientRpc]
	public void LocalGenerateMap(int width, int height, byte[,] data, float gs){
		/*Debug.Log("...");
		if(!isLocalPlayer){
			return;
		}*/
		//Debug.Log("...");
		if(init){
			return;
		}
		init = true;
		//Debug.Log("Generating map...");

		this.gridSize = gs;
		
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.transform.position = new Vector3(gridSize * (width - 1) / 2.0f, gridSize * (height - 1) / 2.0f, 0.01f);
		plane.transform.localScale = new Vector3(scaleConst * gridSize * width, 1, scaleConst * gridSize * height);
		plane.transform.rotation = Quaternion.Euler(new Vector3(270, 0, 0));
		MeshRenderer mr = plane.GetComponent<MeshRenderer>();
		mr.material = bgMaterial;

		tiles = new GameObject[width, height];

		//Debug.Log(data[0, 0]);

		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				tiles[x, y] = GetTile(x, y, data[x, y]);
			}
		}
	}

	GameObject GetTile(int x, int y, byte material){
		GameObject obj = Instantiate(tile) as GameObject;
		obj.transform.position = new Vector3(x * gridSize, y * gridSize, 0) + origin;
		obj.transform.localScale = new Vector3(scaleConst * (gridSize - spacing), 1, scaleConst * (gridSize - spacing));
		MeshRenderer mr = obj.GetComponent<MeshRenderer>();
		//string terrainType;
		mr.material = GetMaterial(material);// GetMaterial(x, y, out terrainType);
		GridController gc = obj.GetComponent<GridController>();
		gc.terrainType = MapGenerator.ToTerrainType(material);
		return obj;
	}
	
	Material GetMaterial(byte terrainType){
		switch(terrainType){
			case MapGenerator.rock:
				return rockMaterial;
			case MapGenerator.swamp:
				return swampMaterial;
			case MapGenerator.grass:
				return grassMaterial;
			default:
				throw new UnityException("Invalid terrain type");
		}
	}

	public float GridSize(){
		return gridSize;
	}
}
