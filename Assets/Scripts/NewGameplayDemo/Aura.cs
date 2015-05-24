using UnityEngine;
using System.Collections;

public class Aura : MonoBehaviour {
	private Transform auraIndicator;
	public float range;
	public Color activeColor = new Color(1, 1, 1, .125f);
	public Color inactiveColor = new Color(1, 1, 1, .0625f);
	
	[ExecuteInEditMode]
	void Start () {
		auraIndicator = transform.FindChild("AuraIndicator");
	}

	[ExecuteInEditMode]
	void Update () {
		auraIndicator.localScale = new Vector3(range * 2f, 0.1f, range * 2f);
	}

	public void ShowAura(bool show)
	{
		auraIndicator.gameObject.GetComponent<MeshRenderer>().material.color = show ? activeColor : inactiveColor;
	}
}
