using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InstalledProgram))]
public class DataMiner : MonoBehaviour {
	public Database targetDatabase;
	public InputSocket targetInputSocket;
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
		rangeIndicator.localScale = new Vector3(range/1.5f, 0.1f, range/1.5f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(targetDatabase == null && targetInputSocket == null && !installedProgram.placing) {
			UpdateTargetDatabase();
			if(targetDatabase == null) {
				UpdateTargetInputSocket();
			}
		}
	}

	[ExecuteInEditMode]
	void Update()
	{
		if(installedProgram.placing || (HoverCheck.HoveringProgram != null && HoverCheck.HoveringProgram.gameObject == gameObject)) {
			rangeIndicator.gameObject.SetActive(true);
		} else {
			rangeIndicator.gameObject.SetActive(false);
		}
	}

	void UpdateParticles()
	{
		Vector3 targetPosition = targetDatabase != null ? targetDatabase.transform.position : targetInputSocket.transform.position;
		var distance = Utility.FlatDistance(transform.position, targetPosition);
		particles.transform.position = targetPosition;
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

	void UpdateTargetInputSocket()
	{
		var inputSockets = FindObjectsOfType(typeof(InputSocket));
		var bestSocket = (InputSocket)null;
		var lowestQueueSize = int.MaxValue;
		foreach(InputSocket socket in inputSockets)
		{
			if(socket.ConnectedPort != null) {
				var distance = Utility.FlatDistance(transform.position, socket.transform.position);
				if(distance <= range && socket.QueueSize < lowestQueueSize) {
					lowestQueueSize = socket.QueueSize;
					bestSocket = socket;
				}
			}
		}
		if(bestSocket != null) {
			targetInputSocket = bestSocket;
			targetInputSocket.QueueRead(this);
			UpdateParticles();
			particles.gameObject.SetActive(true);
			Debug.Log("Found socket");
		} else {
			particles.gameObject.SetActive(false);
		}
	}

	public void Grant(string fileType)
	{
		Debug.Log("Received a file: " + fileType);
		targetDatabase = null;
		targetInputSocket = null;
	}


}
