using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TargetDummyEnemy;
using Random = System.Random;

public class MapGenerator : MonoBehaviour {

	Random generator;
	int seed;
	public GameObject targetDummy;
	public GameObject fighter;
	public GameObject tile;
	public Material rockMaterial;
	public Material grassMaterial;
	public Material swampMaterial;
	public Material bgMaterial;
	public ShopManager shopManager;
	public Text waveText;
	public Text enemiesLeftText;
	
	public float gridSize = 1.0f;
	public float spacing = 0.01f;
	
	const float scaleConst = 0.1f;

	public int width = 16;
	public int height = 16;

	Vector3 origin = new Vector3 (0, 0, 0);
	
	
	public GameObject[,] tiles;
	public GameObject[,] objects;

	protected int waveLeft{
		get{
			return _waveLeft;
		}
		set{
			_waveLeft = value;
			enemiesLeftText.text = "Enemies Left: " + _waveLeft;
		}
	}
	int _waveLeft;
	protected int wave{
		get{
			return _wave;
		}
		set{
			_wave = value;
			waveText.text = "Wave: " + _wave;
		}
	}
	int _wave;

	int enemyCount;
	int swampCount;
	HashSet<KeyValuePair<int,int>> swamps = new HashSet<KeyValuePair<int,int>>();
	
	// Use this for initialization
	void Start () {
		swampCount = width * height / 8;
		seed = new Random ().Next ();
		generator = new Random (seed);
		tiles = new GameObject[width,height];
		objects = new GameObject[width, height];
		GenerateWorld ();
		NextWave ();
	}

	void GenerateWorld(){
		for (int i = 0; i < swampCount; i++) {
			swamps.Add(new KeyValuePair<int, int>(
				(int)(generator.NextDouble() * width),
				(int)(generator.NextDouble() * height)));
		}
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

	public void EnemyDestroyed(){
		waveLeft--;
		if (waveLeft == 0) {
			shopManager.Shop();
		}
	}



	public void NextWave(){
		wave++;
		enemyCount = (int)(width * height / 4.0f * (1 - Mathf.Pow(0.95f, wave))) + 1;
		float waveValue = 0.5f * (1 - Mathf.Pow (0.8f, wave));
		Debug.Log(wave + ":" + enemyCount + "," + waveValue);
		for (int i = 0; i < enemyCount; i++) {
			GameObject obj;
			EnemyBaseController ctrl;
			if(UnityEngine.Random.value > waveValue){
				obj = Instantiate (targetDummy) as GameObject;
				TargetDummyController tdc = obj.GetComponentInChildren<TargetDummyController> ();
				tdc.SetMode(UnityEngine.Random.Range(0,4));
				ctrl = tdc; 
			}
			else{
				obj = Instantiate (fighter) as GameObject;
				ctrl = obj.GetComponentInChildren<FighterEnemy.FighterController> ();
			}
			ctrl.target = GameObject.FindObjectOfType<PlayerController>();
			ctrl.map = this;
			ctrl.Initialize ();
			int tries = 10000;
			int counter = 0;
			while (counter < tries) {
				int x = (int)(generator.NextDouble() * width);
				int y = (int)(generator.NextDouble() * width);
				if (ctrl.movement.CanMoveTo (x, y) && !(x == 0 && y == 0)) {
					ctrl.movement.Setup (x, y, 
					     Direction.RandomDirection());
					break;
				}
				counter++;
			}
			if (counter >= tries) {
				Debug.Log ("What is this unluckiness?");
			}
			waveLeft = enemyCount;
		}
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
		if (swamps.Contains(new KeyValuePair<int, int>(x, y))) {
			terrainType = "swamp";
			return swampMaterial;
		}
		terrainType = "grass";
		return grassMaterial;
	}
}
