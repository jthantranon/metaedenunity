using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CommandShell : MonoBehaviour {
	public float commandRange;

	private float previousCommandRange;
	private Transform activeRangeIndicator;
	private Transform inactiveRangeIndicator;
	public bool isActive;
	public bool IsActive {
		get {
			return isActive;
		}
		set {
			isActive = value;
		}
	}

	// Use this for initialization
	void Start () {
		activeRangeIndicator = transform.FindChild("ActiveRangeIndicator");
		inactiveRangeIndicator = transform.FindChild("InactiveRangeIndicator");
	}

	void Update () {
		if(commandRange != previousCommandRange) {
			activeRangeIndicator.localScale = new Vector3(commandRange * 2f, 0.1f, commandRange * 2f);
			inactiveRangeIndicator.localScale = new Vector3(commandRange * 2f, 0.1f, commandRange * 2f);
			previousCommandRange = commandRange;
		}
		activeRangeIndicator.gameObject.SetActive(isActive);
		inactiveRangeIndicator.gameObject.SetActive(!isActive);
	}
}
