using UnityEngine;
using System.Collections;

public class EnemyBaseCombat : EntityCombat {

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
	protected override void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

	protected virtual EnemyBaseController GetController(){
		return _controller;
	}
}
