using UnityEngine;
using System.Collections.Generic;

public abstract class LineProjectile : Projectile{

	protected float speed;
	protected float dist;
	protected float objWidth;
	protected float objHeight;
	float movedX = 0;
	float movedY = 0;

	protected LineProjectile(){

	}

	public LineProjectile(EntityController owner, float dist, float speed, GameObject obj, float w, float h){
		this.owner = owner;
		this.dist = dist;
		this.speed = speed;
		this.obj = obj;
		objWidth = w;
		objHeight = h;
		active = true;
	}

	public override void Init(){
		//Do nothing
	}

	public override float GetMaxDist(){
		return dist;
	}

	public override bool UpdateMovement(float deltaTime){
		if(!active){
			return false;
		}

		float deltaX = -Mathf.Sin(direction * Mathf.Deg2Rad) * GetSpeed() * deltaTime;
		float deltaY = Mathf.Cos(direction * Mathf.Deg2Rad) * GetSpeed() * deltaTime;
		movedY += deltaY;
		movedX += deltaX;
		y += deltaY;
		x += deltaX;
		obj.transform.position = new Vector3(x, y, obj.transform.position.z);
		if(movedX * movedX + movedY * movedY > dist * dist){
			active = false;
		}
		//collision code in ProjectileSkillEvent class
		return true;
	}

	public override float GetSpeed(){
		return speed;
	}

	//true = to be destroyed
	public override bool OnHit(EntityController controller){
		if(!active){
			return false;
		}
		if(Hit(controller)){
			active = false;
			return true;
		}
		return false;
	}

	public abstract bool Hit(EntityController controller);

	public override Rect CollisionRect(){
		Transform tf = obj.transform;
		return new Rect(tf.position.x - objWidth / 2, tf.position.y - objHeight / 2, objWidth, objHeight);
	}

	public override void CleanUp(){
		GameObject.Destroy(obj);
	}
}
