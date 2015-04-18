using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameNetwork))]
public class NetworkMessageHandler : MonoBehaviour {
	private GameNetwork gameNetwork;

	// Use this for initialization
	void Start () {
		gameNetwork = GetComponent<GameNetwork>();
		gameNetwork.OnConnected += (object sender, System.EventArgs e) => SendAuthenticationMessage();
	}
	
	// Update is called once per frame
	void Update () {
		while(gameNetwork.MessageQueue.Count > 0) {
			HandleMessage(gameNetwork.MessageQueue.Dequeue());
		}
	}

	void HandleMessage(IDictionary<string, object> message)
	{
		var messageType = message["messageType"] as string;
		Debug.Log(messageType);
		switch(messageType) 
		{
			case "userInfo":
			SendCharacterListRequest();
			break;
		case "characterList":
			HandleCharacterListMessage(message);
			break;
		}
	}

	void HandleCharacterListMessage(IDictionary<string, object> message)
	{
		var characters = message["characters"] as object[];
		foreach(IDictionary<string, object> character in characters)
		{
			Debug.Log("Character: " + character["name"] as string);
		}
	}

	private void SendAuthenticationMessage()
	{
		gameNetwork.Send ("{\"messageType\":\"authenticate\", \"token\":\""+(Authentication.Token)+"\"}");
	}

	private void SendCharacterListRequest()
	{
		gameNetwork.Send ("{\"messageType\":\"characterList\"}");
	}
}
