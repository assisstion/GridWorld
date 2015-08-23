using UnityEngine;
using System.Collections;

public class EnemyBaseCombat : EntityCombat{

	public EnemyBaseController controller{
		get{
			return GetController();
		}
	}

	EnemyBaseController _controller;

	public EnemyBaseCombat(){

	}

	public EnemyBaseCombat(EnemyBaseController controller){
		_controller = controller;
	}

	// Use this for initialization
	protected override void Start(){
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update(){
		base.Update();
	}

	protected virtual EnemyBaseController GetController(){
		return _controller;
	}

	public override void ActionUpdate(){
		base.ActionUpdate();
		if(GetAction() == 0){
			if(controller.target == null){
				GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
				if(objs.Length > 0){
					int i = Random.Range(0, objs.Length);
					controller.target = objs[i].GetComponent<PlayerController>();
				}
			}
			else{
				PerformAction();
			}
		}
	}

	protected virtual void PerformAction(){
		//to be overriden
	}

	protected override void CleanUp(){
		controller.map.EnemyDestroyed();
	}
}
