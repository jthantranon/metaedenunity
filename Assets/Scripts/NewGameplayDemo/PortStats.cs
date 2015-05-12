using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(ProgramStats))]
public class PortStats : MonoBehaviour {
	private ProgramStats programStats;

	public Transform statsPanel;
	public Text destinationText;
	public Text inputText;
	public Text outputText;
	public Text bandwidthText;

	public Port selectedPort;

	// Use this for initialization
	void Start () {
		programStats = GetComponent<ProgramStats>();
	}
	
	// Update is called once per frame
	void Update () {
		if(programStats.currentlySelectedProgram != null && programStats.currentlySelectedProgram.GetComponent<Port>() != null) 
		{
			statsPanel.gameObject.SetActive(true);
			var port = programStats.currentlySelectedProgram.GetComponent<Port>();
			destinationText.text = port.destination;
			inputText.text = string.Format("In: {0} RAM / {1} CPU", port.InputMemory, port.InputProcessingPower);
			outputText.text = string.Format("Out: {0} RAM / {1} CPU", port.OutputMemory, port.OutputProcessingPower);
			bandwidthText.text = "Bandwidth: " + port.bandwidth;
		} else {
			statsPanel.gameObject.SetActive(false);
		}
	}
}
