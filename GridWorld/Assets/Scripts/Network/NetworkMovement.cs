using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkMovement : NetworkBehaviour{

	public bool convMover;

	private const float z = -1;
	
	[SyncVar]
	public int
		x;
	[SyncVar]
	public int
		y;
	//[SyncVar]
	//public float
	//	gridSize;
	[SyncVar]
	public int
		direction;
	[SyncVar]
	public float
		gridSize;

	public GameObject mover;

	// Use this for initialization
	void Start(){
	
	}
	
	// Update is called once per frame
	void Update(){
		//if(isLocalPlayer){
		if(convMover){
			mover.transform.rotation = Quaternion.Euler(
				new Vector3(Direction.Rotation(direction), 270, 90));
			mover.transform.position = ConvertPosition(x, y, z);
			if(!isLocalPlayer){
				return;
			}
			CameraController cam = GetComponentInChildren<CameraController>();
			cam.UpdateLocation(cam.player.transform.position.x, cam.player.transform.position.y);
		}
		//}
	}
	
	Vector3 ConvertPosition(int x, int y, float z){
		return new Vector3(x * gridSize, y * gridSize, z);
	}
}
