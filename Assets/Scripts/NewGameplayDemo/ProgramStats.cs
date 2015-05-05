using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgramStats : MonoBehaviour {
	public InstalledProgram currentlySelectedProgram;
	public Transform statsPanel;
	public Text nameText;
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
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Debug.Log("Clicked");
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.000f))
			{
				var clickedProgram = hit.collider.GetComponent<InstalledProgram>();
				if(clickedProgram != null) {
					currentlySelectedProgram = clickedProgram;
				} else 
				{currentlySelectedProgram = null;
					Debug.Log("Clicked on: " + hit.collider.name);
				}
			} else {
				Debug.Log("Not Hit");
			}
		}
	}

	void UpdateStats()
	{
		if(currentlySelectedProgram != null) {
			statsPanel.gameObject.SetActive(true);
			nameText.text = currentlySelectedProgram.gameObject.name;
			concealmentText.text = currentlySelectedProgram.concealmentRating.ToString();
			powerText.text = currentlySelectedProgram.processingPower.ToString();
			memoryText.text = currentlySelectedProgram.memory.ToString();
			var stealthFields = FindObjectsOfType(typeof(StealthField));
			Debug.Log("Found " + stealthFields.Length + " stealth fields");
			var additionalConcealment = 0f;
			foreach(StealthField stealthField in stealthFields) {
				var distSquared = Vector3.SqrMagnitude(currentlySelectedProgram.transform.position - stealthField.transform.position);
				var fieldRadiusSquared = stealthField.radius * stealthField.radius;
				if(distSquared < fieldRadiusSquared)
				{
					additionalConcealment += stealthField.concealmentAmount;
				}
			}
			if(additionalConcealment > 0) {
				Debug.Log("Adding");
				concealmentText.text += "+" + additionalConcealment + " (" + (currentlySelectedProgram.concealmentRating + additionalConcealment) + ")";
			}
		} else {
			statsPanel.gameObject.SetActive(false);
		}
	}
}
