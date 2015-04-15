using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
	private CharacterController characterController;

	public Vector3 targetPosition;
	public bool isMoving;
	public float targetRadius = 0.5f;
	public float speed = 1f;


	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isMoving) {
			var vec = (targetPosition - gameObject.transform.position);
			var dir = vec.normalized;
			var dist = vec.magnitude;
			if(dist >= targetRadius) {
				var amount = Mathf.Min(dist - targetRadius, speed * Time.deltaTime);
				var move = dir * amount;
				//gameObject.transform.position += move;
				characterController.Move(move);
			} else {
				isMoving = false;
			}
		}
	}

	public void AddItem(int type, int count)
	{
		Debug.Log("Player received item!");
	}
}
