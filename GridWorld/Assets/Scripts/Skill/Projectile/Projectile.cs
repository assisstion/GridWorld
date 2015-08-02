using UnityEngine;
using System.Collections.Generic;

public abstract class Projectile{
	
	public EntityController owner;
	public GameObject obj;
	public float x;
	public float y;
	public float direction; //degrees
	public bool active;
	public bool clipping; //true if disappears upon striking walls


	public abstract void Init();

	public abstract float GetMaxDist();

	public abstract bool UpdateMovement(float deltaTime);

	public abstract float GetSpeed();

	public abstract bool OnHit(EntityController controller);

	public abstract Rect CollisionRect();

	public abstract void CleanUp();
}
