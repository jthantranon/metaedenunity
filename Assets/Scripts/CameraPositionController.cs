using UnityEngine;
using System.Collections;

public class CameraPositionController : MonoBehaviour {
	private Vector3 offsetDirection = new Vector3(-1, 1, -1).normalized;

	public GameObject target;
	public float distanceToTarget;
	public float minDistanceToTarget;
	public float maxDistanceToTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(distanceToTarget < minDistanceToTarget) {
			distanceToTarget = minDistanceToTarget;
		}
		if(distanceToTarget > maxDistanceToTarget) {
			distanceToTarget = maxDistanceToTarget;
		}
		transform.position = target.transform.position + (offsetDirection * distanceToTarget);
		transform.LookAt(transform.transform.position);
	}
}
