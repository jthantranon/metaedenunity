using UnityEngine;
using System.Collections;

public class HoverCheck : MonoBehaviour {
	private static InstalledProgram hoveringProgram;
	public static InstalledProgram HoveringProgram { get { return hoveringProgram; } }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 1000.000f))
		{
			var program = hit.collider.GetComponent<InstalledProgram>();
			if(program != null) {
				hoveringProgram = program;
			} else {
					hoveringProgram = null;
			}
		} else {
				hoveringProgram = null;
		}
	}	
}
