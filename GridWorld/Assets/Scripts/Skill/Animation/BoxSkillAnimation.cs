using UnityEngine;
using System.Collections.Generic;

public class BoxSkillAnimation : SkillAnimation{

	const float xScale = 0.05f;
	const float yScale = 0.05f;

	protected Dictionary<KeyValuePair<int, int>, GameObject> anim;

	protected Color color;
	protected HashSet<KeyValuePair<int, int>> coords;

	public BoxSkillAnimation(int x, int y, int direction, float length) : 
		this(x, y, direction, length, new HashSet<KeyValuePair<int, int>>(), Color.black){
		
	}

	public BoxSkillAnimation(int x, int y, int direction, float length, HashSet<KeyValuePair<int, int>> coords,
		Color color) : base(x, y, direction, length){
		this.coords = coords;
		this.color = color;
		anim = new Dictionary<KeyValuePair<int, int>, GameObject>();
	}

	public virtual Color GetColor(){
		return color;
	}

	public virtual HashSet<KeyValuePair<int, int>> GetCoords(){
		return coords;
	}

	public override void Animate(){
		foreach(KeyValuePair<int, int> coord in GetCoords()){
			KeyValuePair<int, int> pair = LocalToGame(coord);
			GameObject animObj;
			animObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
			animObj.GetComponent<MeshRenderer>().material.color = GetColor();
			animObj.transform.position = ConvertPosition(pair.Key, pair.Value, -2.0f);
			animObj.transform.rotation = Quaternion.Euler(new Vector3(Direction.Rotation(direction), 270, 90));
			animObj.transform.localScale = new Vector3(xScale, 1, yScale);
			anim.Add(coord, animObj);
		}
	}

	public override void Update(float timePassed){
		foreach(KeyValuePair<KeyValuePair<int, int>, GameObject> animPair in anim){
			KeyValuePair<int, int> pair = LocalToGame(animPair.Key);
			int vx = pair.Key;
			int vy = pair.Value;
			GameObject animX = animPair.Value;
			animX.transform.position = ConvertPosition(vx, vy, -2.0f);
			animX.transform.localScale = new Vector3 
				(xScale * (1 - timePassed / length), animX.transform.localScale.y, yScale * (1 - timePassed / length));
		}
	}

	public override void Destroy(){
		foreach(KeyValuePair<KeyValuePair<int, int>, GameObject> obj in anim){
			GameObject.Destroy(obj.Value);
		}
	}

	protected KeyValuePair<int, int> LocalToGame(KeyValuePair<int, int> local){
		return new KeyValuePair<int, int>(x + Direction.ValueX(direction) * local.Value + 
			Direction.ValueY(direction) * local.Key, y + Direction.ValueY(direction) 
			* local.Value + Direction.ValueX(direction) * local.Key);
	}

	public Vector3 ConvertPosition(int x, int y, float z){
		float gridSize = GameObject.FindGameObjectWithTag("CGameController")
			.GetComponent<ClientMapController>().GridSize();
		return new Vector3(x * gridSize, y * gridSize, z);
	}
}
