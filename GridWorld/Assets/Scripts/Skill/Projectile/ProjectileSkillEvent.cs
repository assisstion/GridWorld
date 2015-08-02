using UnityEngine;
using System.Collections.Generic;

public abstract class ProjectileSkillEvent : NoncombatAbstractSkillEvent{


	protected HashSet<Projectile> projectiles = new HashSet<Projectile>();

	public override void CleanUp(){

	}

	protected override bool PostCast(){
		foreach(float f in GetAngles()){
			Projectile p = GetNewProjectile();
			Vector3 converted = controller.movement.ConvertPosition(x, y, 0);
			p.x = converted.x;
			p.y = converted.y;
			p.direction = Direction.Rotation(this.direction) + f;
			p.Init();
			projectiles.Add(p);
		}
		return true;
	}

	protected abstract HashSet<float> GetAngles();

	protected float previousUpdate = 0;
	
	public override bool Update(){
		if(ShouldStopUpdate()){
			return false;
		}
		float currentTime = TimePassed();
		HashSet<Projectile> toBeDestroyed = new HashSet<Projectile>();
		foreach(Projectile proj in projectiles){
			if(!proj.UpdateMovement(currentTime - previousUpdate)){
				toBeDestroyed.Add(proj);
				continue;
			}
			foreach(KeyValuePair<int, int> pair in CollisionSquares(proj.CollisionRect())){
				int x = pair.Key;
				int y = pair.Value;
				if(!proj.owner.movement.IsGameSpace(x, y) || 
					(proj.clipping && !proj.owner.movement.CanPass(proj.owner.movement.map.tiles[x, y]))){
					toBeDestroyed.Add(proj);
					break;
				}
				GameObject obj = controller.movement.map.objects[x, y];
				if(obj != null){
					EntityController ctrl = GetControllerFromObject(obj);
					if(ctrl != null){
						if(proj.OnHit(ctrl)){
							toBeDestroyed.Add(proj);
							break;
						}
					}
				}
			}
		}
		foreach(Projectile proj in toBeDestroyed){
			proj.CleanUp();
			projectiles.Remove(proj);
		}
		toBeDestroyed.Clear();
		previousUpdate = currentTime;
		return true;
	}

	public EntityController GetControllerFromObject(GameObject target){
		if(target.tag.Equals("Enemy")){
			if(!controller.tag.Equals("Enemy")){
				EnemyBaseManager manager = target.GetComponent<EnemyBaseManager>();
				if(manager != null){
					return manager.controller;
				}
			}
		}
		else if(target.tag.Equals("Player")){
			if(!controller.tag.Equals("Player")){
				return target.GetComponent<PlayerController>();
			}
		}
		return null;
	}

	public HashSet<KeyValuePair<int, int>> CollisionSquares(Rect input){
		Vector3 min = controller.movement.UnconvertPosition(input.xMin, input.yMin, 0);
		Vector3 max = controller.movement.UnconvertPosition(input.xMax, input.yMax, 0);
		HashSet<KeyValuePair<int, int>> set = new HashSet<KeyValuePair<int, int>>();
		for(int x = Mathf.FloorToInt(min.x); x <= Mathf.CeilToInt(max.x); x++){
			for(int y = Mathf.FloorToInt(min.y); y <= Mathf.CeilToInt(max.y); y++){
				set.Add(new KeyValuePair<int, int>(x, y));
			}
		}
		return set;
	}

	protected override bool ShouldCancel(HashSet<KeyValuePair<int, int>> casts){
		return false;
	}

	protected virtual bool ShouldStopUpdate(){
		return projectiles.Count == 0;
	}

	protected abstract Projectile GetNewProjectile();
}
