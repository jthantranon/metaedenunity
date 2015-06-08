using UnityEngine;
using System.Collections;

public class Pillar : MonoBehaviour {
	private Transform walls;
	private Transform roof;

	public float height = 1;

	[ExecuteInEditMode]
	void Start () {
		walls = transform.FindChild("Walls");
		roof = transform.FindChild("Roof");
	}
	
	[ExecuteInEditMode]
	void Update () {
		roof.localPosition = new Vector3(0, height + .05f, 0);
		walls.localScale = new Vector3(1, height, 1);
		walls.localPosition = new Vector3(0, height/2, 0);
	}
}
