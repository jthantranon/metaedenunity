﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameNetwork))]
public class NetworkMessageHandler : MonoBehaviour {
	private GameNetwork gameNetwork;
	private Queue<IDictionary<string, object>> unhandledQueue = new Queue<IDictionary<string, object>>();

	public event System.Action<IDictionary<string, object>[]> CharacterList;
	public event System.Action<string> JoinedInstance;
	public event System.Action<IDictionary<string, object>> CharacterInfo;
	public event System.Action<IDictionary<string, object>> ZoneInfo;
	public event System.Action<IDictionary<string, object>> WorldStateDelta;
	public event System.Action<string> EntityDestroyed;

	public delegate void ResourceGainedDelegate(string type, int count);
	public event ResourceGainedDelegate ResourceGained;
	
	public delegate void PlayerJoinedDelegate(string playerId, IDictionary<string, object> character);
	public event PlayerJoinedDelegate PlayerJoined;

	// Use this for initialization
	void Start () {
		gameNetwork = GetComponent<GameNetwork>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		while(GameNetwork.MessageQueue.Count > 0) 
		{
			var message = GameNetwork.MessageQueue.Dequeue();
			if(!HandleMessage(message))
			{
				unhandledQueue.Enqueue(message);
			}
		}
		while(unhandledQueue.Count > 0)
		{
			GameNetwork.MessageQueue.Enqueue(unhandledQueue.Dequeue());
		}
	}

	bool HandleMessage(IDictionary<string, object> message)
	{
		var messageType = message["messageType"] as string;
		Debug.Log(messageType);
		var handled = false;
		switch(messageType) 
		{
		case "characterList":
			handled = HandleCharacterListMessage(message);
			break;
		case "playerInfo":
			handled = HandleCharacterInfoMessage(message);
			break;
		case "instanceInfo":
			handled = HandleZoneInfoMessage(message);
			break;
		case "joinedInstance":
			handled = HandleJoinedInstanceMessage(message);
			break;
		case "resourceGained":
			handled = HandleResourceGainedMessage(message);
			break;
		case "playerJoined":
			handled = HandlePlayerJoinedMessage(message);
			break;
		case "entityDestroyed":
			handled = HandleEntityDestroyedMessage(message);
			break;
		case "worldStateDelta":
			handled = HandleWorldStateDeltaMessage(message);
			break;
		}
		return handled;
	}

	bool HandleCharacterListMessage(IDictionary<string, object> message)
	{
		var characterObjectList = (message["characters"] as object[]);
		var characters = new IDictionary<string, object>[characterObjectList.Length];
		for(var i = 0; i < characterObjectList.Length; ++i) {
			characters[i] = characterObjectList[i] as IDictionary<string, object>;
		}
		if(CharacterList != null) {
			CharacterList(characters);
			return true;
		}
		return false;
	}

	bool HandleCharacterInfoMessage(IDictionary<string, object> message)
	{
		var info = message["info"] as IDictionary<string, object>;
		if(CharacterInfo != null) {
			CharacterInfo(info);
			return true;
		}
		return false;
	}

	bool HandleJoinedInstanceMessage(IDictionary<string, object> message)
	{
		var instanceId = message["instanceId"] as string;
		if(JoinedInstance != null) {
			JoinedInstance(instanceId);
			return true;
		}
		return false;
	}

	bool HandleZoneInfoMessage(IDictionary<string, object> message)
	{
		var info = message["info"] as IDictionary<string, object>;
		if(ZoneInfo != null) {
			ZoneInfo(info);
			return true;
		} 
		return false;
	}

	bool HandleResourceGainedMessage(IDictionary<string, object> message)
	{
		var type = message["type"] as string;
		var count = System.Convert.ToInt32(message["count"]);
		if(ResourceGained != null) {
			ResourceGained(type, count);
			return true;
		}
		return false;
	}

	bool HandlePlayerJoinedMessage(IDictionary<string, object> message)
	{
		var playerId = message["playerId"] as string;
		var character = message["character"] as IDictionary<string, object>;
		if(PlayerJoined != null)
		{
			PlayerJoined(playerId, character);
			return true;
		}
		return false;
	}

	bool HandleEntityDestroyedMessage(IDictionary<string, object> message)
	{
		var entityId = message["entityId"] as string;
		if(EntityDestroyed != null)
		{
			EntityDestroyed(entityId);
			return true;
		}
		return false;
	}

	bool HandleWorldStateDeltaMessage(IDictionary<string, object> message)
	{
		var wsd = message["worldStateDelta"] as IDictionary<string, object>;
		if(WorldStateDelta != null) {
			WorldStateDelta(wsd);
			return true;
		}
		return false;
	}


	public void SendCharacterListRequest()
	{
		gameNetwork.Send ("{\"messageType\":\"characterList\"}");
	}

	public void SendSelectCharacterMessage(string characterId)
	{
		gameNetwork.Send ("{\"messageType\":\"selectCharacter\",\"characterId\":\"" + characterId + "\"}");
	}

	public void SendCreateCharacterMessage(string name)
	{
		gameNetwork.Send ("{\"messageType\":\"createCharacter\",\"character\":{\"name\":\"" + name + "\"}}");
	}

	public void SendMoveToPointMessage(Vector3 position)
	{
		gameNetwork.Send ("{\"messageType\":\"moveToPoint\",\"targetPoint\":{\"x\":" + position.x + ", \"y\":" + position.y + "}}");
	}
}
