using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameNetwork))]
public class NetworkMessageHandler : MonoBehaviour {
	private GameNetwork gameNetwork;

	public event System.Action<IDictionary<string, object>[]> CharacterList;
	public event System.Action<string> JoinedInstance;
	public event System.Action<IDictionary<string, object>> PlayerInfo;
	public event System.Action<IDictionary<string, object>> ZoneInfo;

	// Use this for initialization
	void Start () {
		gameNetwork = GetComponent<GameNetwork>();
	}
	
	// Update is called once per frame
	void Update () {
		while(GameNetwork.MessageQueue.Count > 0) {
			HandleMessage(GameNetwork.MessageQueue.Dequeue());
		}
	}

	void HandleMessage(IDictionary<string, object> message)
	{
		var messageType = message["messageType"] as string;
		Debug.Log(messageType);
		switch(messageType) 
		{
		case "characterList":
			HandleCharacterListMessage(message);
			break;
		case "playerInfo":
			HandlePlayerInfoMessage(message);
			break;
		case "instanceInfo":
			HandleZoneInfoMessage(message);
			break;
		case "joinedInstance":
			HandleJoinedInstanceMessage(message);
			break;
		}
	}

	void HandleCharacterListMessage(IDictionary<string, object> message)
	{
		var characterObjectList = (message["characters"] as object[]);
		var characters = new IDictionary<string, object>[characterObjectList.Length];
		for(var i = 0; i < characterObjectList.Length; ++i) {
			characters[i] = characterObjectList[i] as IDictionary<string, object>;
		}
		if(CharacterList != null) {
			CharacterList(characters);
		}
	}

	void HandlePlayerInfoMessage(IDictionary<string, object> message)
	{
		var info = message["info"] as IDictionary<string, object>;
		if(PlayerInfo != null) {
			PlayerInfo(info);
		}
	}

	void HandleJoinedInstanceMessage(IDictionary<string, object> message)
	{
		var instanceId = message["instanceId"] as string;
		if(JoinedInstance != null) {
			JoinedInstance(instanceId);
		}
	}

	void HandleZoneInfoMessage(IDictionary<string, object> message)
	{
		var info = message["info"] as IDictionary<string, object>;
		if(ZoneInfo != null) {
			ZoneInfo(info);
		}
	}

	public void SendCharacterListRequest()
	{
		gameNetwork.Send ("{\"messageType\":\"characterList\"}");
	}

	public void SendSelectCharacterMessage(string characterId)
	{
		gameNetwork.Send ("{\"messageType\":\"selectCharacter\",\"characterId\":\"" + characterId + "\"}");
	}
}
