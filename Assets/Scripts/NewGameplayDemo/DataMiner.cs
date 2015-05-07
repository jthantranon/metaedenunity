using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InstalledProgram))]
public class DataMiner : MonoBehaviour {
	public Database targetDatabase;
	public float readSpeed = 2;
	public float range = 10;
	private Transform rangeIndicator;
	private InstalledProgram installedProgram;

	private ParticleSystem particles;

	[ExecuteInEditMode]
	void Start () {
		particles = transform.FindChild("Particles").GetComponent<ParticleSystem>();
		rangeIndicator = transform.FindChild("RangeIndicator");
		installedProgram = GetComponent<InstalledProgram>();
		particles.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(targetDatabase == null && !installedProgram.placing) {
			UpdateTargetDatabase();
		}
	}

	[ExecuteInEditMode]
	void Update()
	{
		rangeIndicator.localScale = new Vector3(range/1.5f, 0.1f, range/1.5f);
	}

	void UpdateParticles()
	{
		var distance = Vector3.Distance(transform.position, targetDatabase.transform.position);
		particles.transform.position = targetDatabase.transform.position;
		particles.startLifetime = distance / particles.startSpeed;
		particles.gameObject.transform.LookAt(gameObject.transform.position);
	}

	void UpdateTargetDatabase()
	{
		var databases = FindObjectsOfType(typeof(Database));
		var bestDatabase = (Database)null;
		var lowestQueueSize = int.MaxValue;
		foreach(Database database in databases)
		{
			var distance = Vector3.Distance(transform.position, database.transform.position);
			if(distance <= range && database.QueueSize < lowestQueueSize) {
				lowestQueueSize = database.QueueSize;
				bestDatabase = database;
			}
		}
		if(bestDatabase != null) {
			targetDatabase = bestDatabase;
			targetDatabase.QueueRead(this);
			UpdateParticles();
			particles.gameObject.SetActive(true);
		} else {
			particles.gameObject.SetActive(false);
		}
	}

	public void Grant(string fileType)
	{
		Debug.Log("Received a file: " + fileType);
		targetDatabase = null;
	}


}
