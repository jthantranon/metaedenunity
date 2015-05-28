﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VirusSweep : MonoBehaviour {
	private static readonly float sweepInterval = 30;
	private static readonly int sweepDamage;

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
		var stealthFields = FindObjectsOfType(typeof(StealthField));
		for(var i = 0; i < installedPrograms.Length; ++i)
		{
			var installedProgram = (InstalledProgram)installedPrograms[i];
			var totalConcealmentRating = installedProgram.concealmentRating;
			foreach(StealthField stealthField in stealthFields) {
				var range = stealthField.Aura.range;
				if(Vector3.SqrMagnitude(installedProgram.transform.position - stealthField.transform.position) 
				   < (range * range))
				{
					totalConcealmentRating += stealthField.concealmentAmount;
				}
			}
			if(random.Next(100) > totalConcealmentRating)
			{
				Debug.Log("Program caught by scan");
				installedProgram.hitPoints -= sweepDamage;
				if(installedProgram.hitPoints <= 0) {
					Debug.Log("Program destroyed");
					Destroy(installedProgram.gameObject);
				}
			}
		}
	}
}
