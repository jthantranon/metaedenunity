using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Database : MonoBehaviour {

	private LinkedList<DataMiner> dataMinerQueue = new LinkedList<DataMiner>();
	private ProgressBar progressBar;
	private float readTimer;
	private bool reading;

	public int QueueSize { get { return dataMinerQueue.Count; } }

	// Use this for initialization
	void Start () {
		progressBar = transform.FindChild("ProgressBar").GetComponent<ProgressBar>();
		GetComponent<InstalledProgram>().IsOwned = true;
	
	}
	
	// Update is called once per frame
	void Update () {
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

	public void QueueRead(DataMiner miner)
	{
		dataMinerQueue.AddLast(miner);
		if(!reading) {
			reading = true;
			progressBar.gameObject.SetActive(true);
			readTimer = miner.readSpeed;
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
