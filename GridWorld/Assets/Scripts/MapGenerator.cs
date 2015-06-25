using UnityEngine;
using System.Collections;
using TargetDummyEnemy;

public class MapGenerator : MonoBehaviour {

	public GameObject targetDummy;
	public GameObject tile;
	public Material rockMaterial;
	public Material grassMaterial;
	public Material swampMaterial;
	public Material bgMaterial;
	
	public float gridSize = 1.0f;
	public float spacing = 0.01f;
	
	const float scaleConst = 0.1f;

	public int width = 16;
	public int height = 16;
	Vector3 origin = new Vector3 (0, 0, 0);
	
	
	public GameObject[,] tiles;
	public GameObject[,] objects;
	
	// Use this for initialization
	void Start () {
		tiles = new GameObject[width,height];
		objects = new GameObject[width, height];
		GenerateWorld ();
		GenerateEnemies ();
	}

	void GenerateWorld(){
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				tiles[x,y] = GetTile(x,y);
			}
		}
		GameObject plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
		plane.transform.position = new Vector3 (gridSize * (width - 1) / 2.0f, gridSize * (height - 1) / 2.0f, 0.01f);
		plane.transform.localScale = new Vector3 (scaleConst * gridSize * width, 1, scaleConst * gridSize * height);
		plane.transform.rotation = Quaternion.Euler (new Vector3 (270, 0, 0));
		MeshRenderer mr = plane.GetComponent<MeshRenderer> ();
		mr.material = bgMaterial;
	}

	void GenerateEnemies(){
		GameObject obj = Instantiate (targetDummy) as GameObject;
		TargetDummyController ctrl = obj.GetComponentInChildren<TargetDummyController> ();
		ctrl.map = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	GameObject GetTile(int x, int y){
		GameObject obj = Instantiate(tile) as GameObject;
		obj.transform.position = new Vector3 (x * gridSize, y * gridSize, 0) + origin;
		obj.transform.localScale = new Vector3(scaleConst*(gridSize-spacing),1,scaleConst*(gridSize-spacing));
		MeshRenderer mr = obj.GetComponent<MeshRenderer> ();
		string terrainType;
		mr.material = GetMaterial (x, y, out terrainType);
		GridController gc = obj.GetComponent<GridController> ();
		gc.terrainType = terrainType;
		return obj;
	}

	Material GetMaterial(int x, int y, out string terrainType){
		if (x == width - y && x % 2 == 0) {
			terrainType = "rock";
			return rockMaterial;
		}
		if (x == 7 || y == 7) {
			terrainType = "swamp";
			return swampMaterial;
		}
		terrainType = "grass";
		return grassMaterial;
	}
}
