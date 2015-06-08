using UnityEngine;
using System.Collections;

public class CircularMovement : MonoBehaviour {
	public float radius = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var height= transform.position.y;
		transform.position = new Vector3(radius * Mathf.Cos(Time.time), height, radius * Mathf.Sin(Time.time));
	}
}
