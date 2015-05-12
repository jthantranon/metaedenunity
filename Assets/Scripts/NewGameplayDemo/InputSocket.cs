using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(InstalledProgram))]
public class InputSocket : MonoBehaviour {
	private InstalledProgram installedProgram;
	private Transform rangeIndicator;
	private Transform streams;
	private GameObject stream;
	private Port connectedPort;


	private LinkedList<DataMiner> dataMinerQueue = new LinkedList<DataMiner>();
	private ProgressBar progressBar;
	private float readTimer;
	private bool reading;

	public GameObject streamPrefab;
	public float range;

	public Port ConnectedPort { get { return connectedPort; } }
	public int QueueSize { get { return dataMinerQueue.Count; } }

	// Use this for initialization
	void Start () {
		installedProgram = GetComponent<InstalledProgram>();
		progressBar = transform.FindChild("ProgressBar").GetComponent<ProgressBar>();
		rangeIndicator = transform.FindChild("RangeIndicator");
		streams = transform.FindChild("Streams");
		OnMoving();
		OnPlaced();
	}
	
	// Update is called once per frame
	void Update () {
		if(installedProgram.placing || (HoverCheck.HoveringProgram != null && HoverCheck.HoveringProgram.gameObject == gameObject)) {
			rangeIndicator.gameObject.SetActive(true);
		} else {
			rangeIndicator.gameObject.SetActive(false);
		}
		if(reading) {
			readTimer -= Time.deltaTime;
			var currentMiner = dataMinerQueue.First.Value;
			if(readTimer <= 0) {
				currentMiner.Grant("Basic Code Snippets");
				RemoveFromQueue(currentMiner);
				if(dataMinerQueue.Count > 0) {
					readTimer = dataMinerQueue.First.Value.readSpeed;
				}
			} else {
				var scale = (currentMiner.readSpeed - readTimer) / currentMiner.readSpeed;
				progressBar.SetProgress(scale);
			}
		}
	}

	void OnPlaced()
	{
		var ports = FindObjectsOfType<Port>();
		foreach(var port in ports)
		{
			var distance = Utility.FlatDistance(transform.position, port.transform.position);
			if(distance <= range) {
				stream = Instantiate(streamPrefab) as GameObject;
				var particles = stream.GetComponent<ParticleSystem>();
				particles.transform.position = new Vector3(port.transform.position.x, 0, port.transform.position.z);
				particles.startLifetime = distance / particles.startSpeed;
				particles.gameObject.transform.LookAt(gameObject.transform.position);
				stream.transform.SetParent(streams);
				connectedPort = port;
				port.AddConnection(this);
				if(dataMinerQueue.Count > 0) {
					reading = true;
					var miner = dataMinerQueue.First.Value;
					progressBar.gameObject.SetActive(true);
					readTimer = Mathf.Min (miner.readSpeed, connectedPort.BandwidthPerConnection);
				}
				break;
			}
		}
	}

	void OnMoving()
	{
		if(connectedPort != null)
		{
			connectedPort.RemoveConnection(this);
			connectedPort = null;
		}
		if(stream != null) {
			GameObject.Destroy(stream);
			stream = null;
		}
		rangeIndicator.localScale = new Vector3(range * 2, 0.1f, range * 2);
	}

	public void QueueRead(DataMiner miner)
	{
		dataMinerQueue.AddLast(miner);
		if(!reading) {
			reading = true;
			progressBar.gameObject.SetActive(true);
			if(connectedPort != null) {
				readTimer = Mathf.Min (miner.readSpeed, connectedPort.BandwidthPerConnection);
			} else {
				readTimer = float.MaxValue;
			}
		}
	}
	
	public void RemoveFromQueue(DataMiner miner)
	{
		dataMinerQueue.Remove(miner);
		if(dataMinerQueue.Count == 0) {
			reading = false;
			progressBar.gameObject.SetActive(false);
		}
	}
}
