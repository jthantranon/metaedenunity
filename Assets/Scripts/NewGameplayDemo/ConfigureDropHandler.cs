using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ConfigureDropHandler : MonoBehaviour, IDropHandler {

	public void OnDrop (PointerEventData eventData)
	{
		var item = GetItem();
		if(!item) {
			ConfigureDragHandler.draggingObject.transform.SetParent(transform);
			ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
		}
	}

	private GameObject GetItem()
	{
		return transform.childCount == 1 ? transform.GetChild(0).gameObject : null;
	}
}
