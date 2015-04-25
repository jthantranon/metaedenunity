using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraPositionController))]
public class KeyboardMouseInput : MonoBehaviour {
	private CameraPositionController cameraPositionController;
	public PlayerController playerController;
	private GameObject network;
	// Use this for initialization
	void Start () {
		cameraPositionController = GetComponent<CameraPositionController>();
		network = GameObject.Find ("Network");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.000f))
			{
				if(hit.collider.tag == "Floor") {
					playerController.targetPosition = new Vector3(hit.point.x, playerController.gameObject.transform.position.y, hit.point.z);
					playerController.isMoving = true;
					network.GetComponent<NetworkMessageHandler>().SendMoveToPointMessage(playerController.targetPosition);
				} else {
					var interactable = hit.collider.GetComponent<Interactable>();
					if(interactable != null) {
						interactable.Interact(playerController.gameObject);
					}
				}
			}
		} 
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
		{
			cameraPositionController.distanceToTarget += 1;
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // backward
		{
			cameraPositionController.distanceToTarget -= 1;
		}
	}
}
