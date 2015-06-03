using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgramStats : MonoBehaviour {
	public InstalledProgram currentlySelectedProgram;
	public CommandShell currentylSelectedShell;
	public Transform statsPanel;
	public Text nameText;
	public Text hitPointsText;
	public Text concealmentText;
	public Text powerText;
	public Text memoryText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UpdateSelection();
		UpdateStats();
	}

	void UpdateSelection()
	{
		if(Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.000f))
			{
				Debug.Log("Clicked on " + hit.collider.name);
				currentylSelectedShell = null;
				currentlySelectedProgram = null;
				var clickedProgram = hit.collider.GetComponent<InstalledProgram>();
				var clickedShell = hit.collider.GetComponent<CommandShell>();
				if(clickedProgram != null) {
					Debug.Log("Selected program");
					currentlySelectedProgram = clickedProgram;
				}  
				if(clickedShell != null) {
					Debug.Log("Selected shell");
					currentylSelectedShell = clickedShell;
				} 
			} else {
				currentlySelectedProgram = null;
				currentylSelectedShell = null;
				Debug.Log("Clicked on nothing");
			}
		}
	}

	void UpdateStats()
	{
		if(currentlySelectedProgram != null) {
			statsPanel.gameObject.SetActive(true);
			nameText.text = currentlySelectedProgram.gameObject.name;
			concealmentText.text = "Conceal: " + currentlySelectedProgram.concealmentRating.ToString();
			hitPointsText.text = "HP: " + currentlySelectedProgram.hitPoints.ToString();
			powerText.gameObject.SetActive(true);
			memoryText.gameObject.SetActive(true);
			powerText.text = "CPU: " + currentlySelectedProgram.processingPower.ToString();
			memoryText.text = "Memory: " + currentlySelectedProgram.memory.ToString();
			var stealthFields = FindObjectsOfType(typeof(StealthField));
			var additionalConcealment = 0f;
			foreach(StealthField stealthField in stealthFields) {
				var distSquared = Vector3.SqrMagnitude(currentlySelectedProgram.transform.position - stealthField.transform.position);
				var range = stealthField.Aura.range;
				var fieldRadiusSquared = range * range;
				if(distSquared < fieldRadiusSquared)
				{
					additionalConcealment += stealthField.concealmentAmount;
				}
			}
			if(additionalConcealment > 0) {
				Debug.Log("Adding");
				concealmentText.text += "+" + additionalConcealment + " (" + (currentlySelectedProgram.concealmentRating + additionalConcealment) + ")";
			}
		} else if(currentylSelectedShell != null) {
			statsPanel.gameObject.SetActive(true);
			nameText.text = currentylSelectedShell.gameObject.name;
			hitPointsText.text = "HP: " + currentylSelectedShell.hitPoints.ToString();
			concealmentText.text = "Conceal: " + currentylSelectedShell.concealmentRating.ToString();
			powerText.gameObject.SetActive(false);
			memoryText.gameObject.SetActive(false);
			var stealthFields = FindObjectsOfType(typeof(StealthField));
			var additionalConcealment = 0f;
			foreach(StealthField stealthField in stealthFields) {
				var distSquared = Vector3.SqrMagnitude(currentylSelectedShell.transform.position - stealthField.transform.position);
				var range = stealthField.Aura.range;
				var fieldRadiusSquared = range * range;
				if(distSquared < fieldRadiusSquared)
				{
					additionalConcealment += stealthField.concealmentAmount;
				}
			}
			if(additionalConcealment > 0) {
				Debug.Log("Adding");
				concealmentText.text += "+" + additionalConcealment + " (" + (currentylSelectedShell.concealmentRating + additionalConcealment) + ")";
			}
		} else {
			statsPanel.gameObject.SetActive(false);
		}
	}
}
