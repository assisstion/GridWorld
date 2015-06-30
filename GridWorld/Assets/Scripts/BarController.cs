using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class BarController : NetworkBehaviour {

	public float percent {
		set {
			_percent = value;
			bar.rectTransform.anchoredPosition = new Vector3(-(100-_percent), 0, 0);
		}
		get{
			return _percent;
		}
	}
	float _percent;
	public Image bar;
	//RectTransform transform;

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		//transform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
