using UnityEngine;
using System.Collections;

public class StealthField : MonoBehaviour {
	public int concealmentAmount;
	public float radius;

	private Transform radiusObject;
	
	[ExecuteInEditMode]
	void Start () {
		radiusObject = transform.FindChild("RadiusField");
	}

	[ExecuteInEditMode]
	void Update () {
		radiusObject.localScale = new Vector3(radius*2f, 0.1f, radius*2f);
	}
}
