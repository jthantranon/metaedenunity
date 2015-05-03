using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VirusSweep : MonoBehaviour {
	private static readonly float sweepInterval = 30;

	private float sweepTimer = sweepInterval;
	private System.Random random = new System.Random();

	public Text sweepText;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		sweepTimer -= Time.deltaTime;
		if(sweepTimer <= 0) {
			PerformSweep();
			sweepTimer = sweepInterval;
		}
		sweepText.text = "Next Virus Sweep: " + Mathf.Round(sweepTimer);
	}

	void PerformSweep() {
		var installedPrograms = FindObjectsOfType(typeof(InstalledProgram));
		for(var i = 0; i < installedPrograms.Length; ++i)
		{
			var installedProgram = (InstalledProgram)installedPrograms[i];
			if(random.Next(100) > installedProgram.concealmentRating)
			{
				Debug.Log("Program caught by scan");
				Destroy(installedProgram.gameObject);
			}
		}
	}
}
