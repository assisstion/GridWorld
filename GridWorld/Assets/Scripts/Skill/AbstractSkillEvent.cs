using UnityEngine;
using System.Collections.Generic;

public abstract class AbstractSkillEvent : SkillEvent{

	protected float startTime;

	public virtual bool Initialize(){
		startTime = Time.time;
		direction = controller.movement.GetDirection();
		x = controller.movement.playerX;// + Direction.ValueX (direction);
		y = controller.movement.playerY;// + Direction.ValueY (direction);
		if(!CanRun()){
			return false;
		}
		HashSet<KeyValuePair<int, int>> willCast = new HashSet<KeyValuePair<int, int>>();
		HashSet<KeyValuePair<int, int>> set = GetCoordinates();
		foreach(KeyValuePair<int, int> pair in set){
			switch(CanCast(pair)){
				case CanCastValue.Yes:
					willCast.Add(pair);
					break;
				case CanCastValue.CancelThis:
					break;// Do nothing
				case CanCastValue.CancelAll:
					return false;
			}
		}
		if(ShouldCancel(willCast)){
			return false;
		}
		foreach(KeyValuePair<int, int> pair in willCast){
			RunAttack(pair);
		}
		foreach(KeyValuePair<int, int> pair in willCast){
			KeyValuePair<int, int> targetPos = LocalToGame(pair);
			GameObject target = controller.movement.map.objects[targetPos.Key, targetPos.Value];
			if(target != null){
				OnHit(target, pair);
			}
		}
		if(!PostCast()){
			return false;
		}
		//TODO add animator
		GameObject.FindGameObjectWithTag("CGameController")
			.GetComponent<ClientAnimation>()
				.RpcAnimateSkill(Skills.Attr(GetInfo()).id, x, y, direction, cooldown);
		return true;
	}

	protected int direction;
	protected int x;
	protected int y;
	protected float cooldown;
	protected EntityController controller;

	public float TimePassed(){
		return Time.time - startTime;
	}

	public abstract bool Update();

	public abstract void CleanUp();

	protected abstract HashSet<KeyValuePair<int, int>> GetCoordinates();

	protected virtual CanCastValue CanCast(KeyValuePair<int, int> coords){
		KeyValuePair<int, int> pair = LocalToGame(coords);
		int vx = pair.Key;
		int vy = pair.Value;
		if(!(controller.movement.IsGameSpace(vx, vy) 
			&& controller.movement.CanPass 
		      (vx, vy))){
			return CanCastValue.CancelThis;
		}
		else{
			return CanCastValue.Yes;
		}
	}

	protected abstract void RunAttack(KeyValuePair<int, int> coord);

	protected virtual bool OnHit(GameObject target, KeyValuePair<int, int> coord){
		if(target.tag.Equals("Enemy")){
			if(!controller.tag.Equals("Enemy")){
				EnemyBaseManager manager = target.GetComponent<EnemyBaseManager>();
				if(manager != null){
					Hit(manager.controller);
					return true;
				}
			}
		}
		else if(target.tag.Equals("Player")){
			if(!controller.tag.Equals("Player")){
				Hit(target.GetComponent<PlayerController>());
				return true;
			}
		}
		return false;
	}

	protected virtual bool PostCast(){
		return true;
	}

	protected abstract void Hit(EntityController controller);

	protected KeyValuePair<int, int> LocalToGame(KeyValuePair<int, int> local){
		return new KeyValuePair<int, int>(x + Direction.ValueX(direction) * local.Value + 
			Direction.ValueY(direction) * local.Key, y + Direction.ValueY(direction) 
			* local.Value + Direction.ValueX(direction) * local.Key);
	}

	public enum CanCastValue{
		Yes,
		CancelThis,
		CancelAll
	}

	public float GetCoolDown(){
		return cooldown;
	}
	
	protected virtual bool CanRun(){
		return true;
	}

	protected virtual bool ShouldCancel(HashSet<KeyValuePair<int, int>> casts){
		return casts.Count == 0;
	}

	public abstract SkillInfo GetInfo();

	/*public virtual float GetActionModifier(){
		return 0;
	}*/
}
