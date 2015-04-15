using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class InteractionArea : MonoBehaviour {
	private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		meshRenderer.enabled = false;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 1000.000f))
		{
			var interactable = hit.collider.GetComponent<Interactable>();
			if(interactable != null) {
				transform.position = new Vector3(hit.collider.transform.position.x, 0.1f, hit.collider.transform.position.z);
				meshRenderer.enabled = true;
				transform.localScale = new Vector3(interactable.interactRadius, 0f, interactable.interactRadius);
			}
		}
	}
}
