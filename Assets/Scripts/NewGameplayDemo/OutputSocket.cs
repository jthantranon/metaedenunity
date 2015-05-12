using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(InstalledProgram))]
public class OutputSocket : MonoBehaviour {
	private InstalledProgram installedProgram;
	private Transform rangeIndicator;
	private Transform streams;
	private List<GameObject> streamList = new List<GameObject>();
	private List<Port> connectedPorts = new List<Port>();
	
	public GameObject streamPrefab;
	public float range;
	
	public List<Port> ConnectedPorts { get { return connectedPorts; } }
	
	// Use this for initialization
	void Start () {
		installedProgram = GetComponent<InstalledProgram>();
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
	}
	
	void OnPlaced()
	{
		var ports = FindObjectsOfType<Port>();
		foreach(var port in ports)
		{
			var distance = Utility.FlatDistance(transform.position, port.transform.position);
			if(distance <= range) {
				var stream = Instantiate(streamPrefab) as GameObject;
				var particles = stream.GetComponent<ParticleSystem>();
				particles.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
				particles.startLifetime = distance / particles.startSpeed;
				particles.gameObject.transform.LookAt(new Vector3(port.transform.position.x, 0, port.transform.position.z));
				stream.transform.SetParent(streams);
				connectedPorts.Add(port);
				port.AddConnection(this);
				streamList.Add(stream);
			}
		}
	}
	
	void OnMoving()
	{
		foreach(var port in connectedPorts)
		{
			port.RemoveConnection(this);
		}
		foreach(var stream in streamList) {
			GameObject.Destroy(stream);
		}
		streamList.Clear();
		connectedPorts.Clear();
		rangeIndicator.localScale = new Vector3(range * 2, 0.1f, range * 2);
	}
}
