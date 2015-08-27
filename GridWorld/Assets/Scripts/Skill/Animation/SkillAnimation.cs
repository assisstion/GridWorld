using UnityEngine;
using System.Collections.Generic;

public abstract class SkillAnimation{

	protected int x;
	protected int y;
	protected int direction;
	protected float length;

	public SkillAnimation(int x, int y, int direction, float length){
		this.x = x;
		this.y = y;
		this.direction = direction;
		this.length = length;
	}

	public abstract void Animate();

	public abstract void Update(float timePassed);

	public abstract void Destroy();

	public int GetX(){
		return x;
	}

	public int GetY(){
		return y;
	}

	public virtual float GetLength(){
		return length;
	}

	public int GetDirection(){
		return direction;
	}
}
