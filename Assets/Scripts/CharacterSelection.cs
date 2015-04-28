using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkMessageHandler))]
public class CharacterSelection : MonoBehaviour {
	private NetworkMessageHandler networkMessageHandler;

	// Use this for initialization
	void Start () {
		networkMessageHandler = GetComponent<NetworkMessageHandler>();
		networkMessageHandler.CharacterList += OnCharacterListRetrieved;
		networkMessageHandler.JoinedInstance += OnJoinedInstance;
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Destroy()
	{
		networkMessageHandler.CharacterList -= OnCharacterListRetrieved;
		networkMessageHandler.JoinedInstance -= OnJoinedInstance;
	}

	private void OnCharacterListRetrieved(IDictionary<string, object>[] characters)
	{
		Debug.Log("Character list retrieved");
		if(characters.Length > 0) {
			SelectCharacter(characters[0]["_id"] as string);
		} else {
			CreateCharacter("test");
		}
		// TODO: do something with the character list [{ id: '', name: '' }, { id: '', name: '' }]
	}

	private void OnJoinedInstance(string instanceId)
	{
		Debug.Log("Joined instance");
		Application.LoadLevel("FirstScene");
	}

	public void SelectCharacter(string characterId)
	{
		networkMessageHandler.SendSelectCharacterMessage(characterId);
	}

	public void CreateCharacter(string name)
	{
		networkMessageHandler.SendCreateCharacterMessage(name);
	}
}
