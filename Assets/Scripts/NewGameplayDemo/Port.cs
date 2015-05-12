using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InstalledProgram))]
public class Port : MonoBehaviour {
	private InstalledProgram installedProgram;

	public int bandwidth;
	public string destination;

	public int InputMemory { get { return 0; } }
	public int InputProcessingPower { get { return 0; } }

	public int OutputMemory { get { return 0; } }
	public int OutputProcessingPower { get { return 0; } }

	void Start () {
		installedProgram = GetComponent<InstalledProgram>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
