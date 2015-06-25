using UnityEngine;
using System.Collections;

public class EnemyBaseMovement : EntityMovement {

	protected EnemyBaseController controller{
		get{
			return GetController();
		}
	}
	EnemyBaseController _controller;

	public EnemyBaseMovement(int x, int y, int dir) 
	: base(x, y, dir){
		
	}

	public EnemyBaseMovement(EnemyBaseController controller, 
	    	int x, int y, int dir) 
			: base(x, y, dir){
		_controller = controller;
	}

	protected virtual EnemyBaseController GetController(){
		return _controller;
	}

	// Use this for initialization
	protected override void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}
}
