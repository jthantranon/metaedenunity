using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkMessageHandler))]
public class CharacterSelection : MonoBehaviour {
	private NetworkMessageHandler networkMessageHandler;

	// Use this for initialization
	void Start () {
		networkMessageHandler = GetComponent<NetworkMessageHandler>();
		if(!GameNetwork.IsConnected) {
			Debug.Log("Waiting for connection");
			GameNetwork.Connected += (sender, e) => 
			{
				Debug.Log("Sending character list request");
				networkMessageHandler.SendCharacterListRequest();
			};
		} else {
			Debug.Log("Sending character list request");
			networkMessageHandler.SendCharacterListRequest();
		}
		networkMessageHandler.CharacterList += OnCharacterListRetrieved;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnCharacterListRetrieved(IDictionary<string, object>[] characters)
	{
		Debug.Log("Character list retrieved");
		// TODO: do something with the character list
	}
}
