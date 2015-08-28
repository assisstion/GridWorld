using UnityEngine;
using System.Collections.Generic;

public class SlashSkillAnimation : SkillAnimation{

	const float xScale = 0.05f;
	const float yScale = 0.05f;

	protected GameObject animObj;

	protected Color color;

	public SlashSkillAnimation(int x, int y, int direction, float length) : 
		this(x, y, direction, length, Color.white){
		
	}

	public SlashSkillAnimation(int x, int y, int direction, float length,
		Color color) : base(x, y, direction, length){
		this.color = color;
	}

	public virtual Color GetColor(){
		return color;
	}

	public override void Animate(){
		KeyValuePair<int, int> pair = LocalToGame(new KeyValuePair<int, int>(0, 1));
		animObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
		animObj.GetComponent<MeshRenderer>().material.color = GetColor();
		animObj.transform.position = ConvertPosition(pair.Key, pair.Value, -2.0f);
		animObj.transform.rotation = Quaternion.Euler(new Vector3(Direction.Rotation(direction), 270, 90));
		animObj.transform.localScale = new Vector3(0.1f, 1, 0.02f);
	}

	public override void Update(float timePassed){
		KeyValuePair<int, int> pair = LocalToGame(new KeyValuePair<int, int>(0, 1));
		int vx = pair.Key;
		int vy = pair.Value;
		float gridSize = GameObject.FindGameObjectWithTag("CGameController")
			.GetComponent<ClientMapController>().GridSize();
		animObj.transform.position = ConvertPosition(vx, vy, -2.0f) 
			- Direction.ToVector(direction).normalized
			* gridSize * (timePassed / length) / 2;
		animObj.transform.localScale = new Vector3 
			(0.1f * (1 - timePassed / length), 
			 animObj.transform.localScale.y, animObj.transform.localScale.z);
	}

	public override void Destroy(){
		GameObject.Destroy(animObj);
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
