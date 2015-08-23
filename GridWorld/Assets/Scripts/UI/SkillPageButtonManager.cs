using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillPageButtonManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

	int boxSize = 50;

	// Use this for initialization
	void Start(){
	
	}
	
	// Update is called once per frame
	void Update(){
	
	}

	public ClientSkillManager manager;
	public GameObject imageSource;
	GameObject tempDragImg;
	public string text;
	public GameObject gui;

	public void OnBeginDrag(PointerEventData data){
		if(data.button != 0){
			return;
		}
		tempDragImg = Instantiate(imageSource) as GameObject;
		gui = gameObject.transform.parent.parent.parent.parent.gameObject;
		tempDragImg.transform.SetParent(gui.transform);
		Text txt = tempDragImg.GetComponentInChildren<Text>();
		txt.text = text;
		RectTransform rt = tempDragImg.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(boxSize, boxSize);
		rt.position = data.pressPosition;
	}

	public void OnDrag(PointerEventData data){
		if(tempDragImg == null){
			return;
		}
		Vector3 worldPoint;
		RectTransformUtility.ScreenPointToWorldPointInRectangle
			(gui.gameObject.GetComponent<RectTransform>(), data.position, data.pressEventCamera, out worldPoint);
		tempDragImg.transform.position = worldPoint;
	}

	public void OnEndDrag(PointerEventData data){
		if(tempDragImg == null){
			return;
		}
		manager.Dropped(data.pressEventCamera, tempDragImg.GetComponent<RectTransform>(), text);
		Destroy(tempDragImg);
	}
}
