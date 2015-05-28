using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ConfigureDragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
	public static GameObject draggingObject;
	private Transform startParent;
	private Vector3 startPosition;

	public void OnBeginDrag (PointerEventData eventData)
	{
		draggingObject = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		draggingObject = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if(transform.parent != startParent) {
			transform.position = transform.parent.position;
		} else {
			transform.position = startPosition;
		}
	}


}
