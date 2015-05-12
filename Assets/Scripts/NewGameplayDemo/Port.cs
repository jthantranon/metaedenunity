using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(InstalledProgram))]
public class Port : MonoBehaviour {
	private InstalledProgram installedProgram;
	private List<MonoBehaviour> connections = new List<MonoBehaviour>();
	private bool didUpdateConnections;
	private int inputMemory;
	private int outputMemory;
	private int inputProcessingPower;
	private int outputProcessingPower;
	private Transform upstream;
	private Transform downstream;

	public int bandwidth;
	public string destination;

	public int InputMemory { get { return inputMemory; } }
	public int InputProcessingPower { get { return inputProcessingPower; } }

	public int OutputMemory { get { return outputMemory; } }
	public int OutputProcessingPower { get { return outputProcessingPower; } }

	public float BandwidthPerConnection { get { return connections.Count > 0 ? bandwidth / connections.Count : 0; } }

	void Start () {
		installedProgram = GetComponent<InstalledProgram>();
		upstream = transform.FindChild("Upstream");
		downstream = transform.FindChild("Downstream");
		UpdateConnectionValues();
	}
	
	// Update is called once per frame
	void Update () {
		if(didUpdateConnections) {
			UpdateConnectionValues();
			didUpdateConnections = false;
		}
	}

	void UpdateConnectionValues()
	{
		inputMemory = outputMemory = inputProcessingPower = outputProcessingPower = 0;
		foreach(var connection in connections)
		{
			if(connection is InputSocket) {
				inputMemory++;
				inputProcessingPower++;
			}
			if(connection is OutputSocket) {
				outputMemory++;
				outputProcessingPower++;
			}
		}
		upstream.gameObject.SetActive(outputMemory + outputProcessingPower > 0);
		downstream.gameObject.SetActive(inputMemory + inputProcessingPower > 0);
	}

	public void AddConnection(InputSocket inputSocket)
	{
		connections.Add(inputSocket);
		didUpdateConnections = true;
	}

	public void RemoveConnection(InputSocket inputSocket)
	{
		connections.Remove(inputSocket);
		didUpdateConnections = true;
	}

	public void AddConnection(OutputSocket outpuSocket)
	{
		connections.Add(outpuSocket);
		didUpdateConnections = true;
	}
	
	public void RemoveConnection(OutputSocket outpuSocket)
	{
		connections.Remove(outpuSocket);
		didUpdateConnections = true;
	}
}
