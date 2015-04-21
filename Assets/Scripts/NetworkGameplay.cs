using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkMessageHandler))]
public class NetworkGameplay : MonoBehaviour {
	private NetworkMessageHandler networkMessageHandler;
	
	// Use this for initialization
	void Start () {
		networkMessageHandler = GetComponent<NetworkMessageHandler>();
		networkMessageHandler.CharacterInfo += OnCharacterInfo;
		networkMessageHandler.ZoneInfo += OnZoneInfo;
	}

	// Update is called once per frame
	void Update () {
	
	}
	void Destroy()
	{
		networkMessageHandler.CharacterInfo -= OnCharacterInfo;
		networkMessageHandler.ZoneInfo -= OnZoneInfo;
	}



	private void OnCharacterInfo(IDictionary<string, object> characterInfo)
	{
		Debug.Log("Character info received");
		// TODO: set character info { id: '', name: '', instanceId: '', position: { x: 0, y: 0 }, inventory: { } }
		// TODO: add player entity
	}

	private void OnZoneInfo(IDictionary<string, object> zoneInfo)
	{
		Debug.Log("Zone info received");
		// TODO: set zone info { id: '', name: '', entities: [], walls: [], height: 0, width: 0 }
		// TODO: add walls, add entities (except player)
	}
}
