using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Pathfinding.Serialization.JsonFx;

public class GameNetwork : MonoBehaviour {
	private static readonly string PingMessage = "{\"messageType\":\"ping\"}";
	private static readonly float PingInterval = 2f;
	private static readonly int BufferSize = 1024;

	private static Socket client;

	public static Queue<IDictionary<string, object>> MessageQueue = new Queue<IDictionary<string, object>>();
	public static bool IsConnected;
	public static bool IsPolling;
	public static event EventHandler Connected;

	private StringBuilder stringBuilder = new StringBuilder();
	private String response = String.Empty;
	private float pingTimer = PingInterval;
	private Coroutine receiveCoroutine;

	public string hostname;
	public int port;


	void Start () {
		if(!IsConnected) {
			StartCoroutine(ConnectAsync());
		}
	}

	void Update () {
		pingTimer -= Time.deltaTime;
		if(pingTimer <= 0) {
			Send(PingMessage);
			pingTimer = PingInterval;
		}
	}

	public void CloseConnection()
	{
		Debug.Log("Destroying connection");
		IsConnected = false;
		IsPolling = false;
		client.Shutdown(SocketShutdown.Both);
		client.Close();
	}

	void OnDestroy()
	{
		if(receiveCoroutine != null) {
			StopCoroutine(receiveCoroutine);
		}
	}

	private IEnumerator ConnectAsync()
	{
		Debug.Log("Connecting");
		var ipHostInfo = Dns.GetHostEntry(hostname);
		var ipAddress = ipHostInfo.AddressList[0];
		var remoteEP = new IPEndPoint(ipAddress, port);
		
		client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		var result = client.BeginConnect(remoteEP, null, null);

		while(!result.IsCompleted) {
			yield return null;
		}
		try
		{
			Debug.Log("Connected");
			client.EndConnect(result);
			receiveCoroutine = StartCoroutine(ReceiveAsync());
			SendAuthenticationMessage();
		}
		catch(Exception e) 
		{
			Debug.LogException(e);
		}
	}

	private IEnumerator ReceiveAsync()
	{
		IsPolling = true;
		while(IsPolling) {
			var buffer = new byte[BufferSize];
			var result = client.BeginReceive(buffer, 0, BufferSize, 0, null, null);
			while(!result.IsCompleted) {
				yield return null;
			}
			var bytesRead = client.EndReceive(result);
			if (bytesRead > 0) {
				stringBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
				if(bytesRead < BufferSize) {
					if (stringBuilder.Length > 1) {
						response = stringBuilder.ToString();
					}
					stringBuilder.Remove(0, stringBuilder.Length);
					Debug.Log(response);
					var jsonReader = new JsonReader(response, new JsonReaderSettings());
					var resp = jsonReader.Deserialize();
					var responseObjects = resp as object[];
					foreach(IDictionary<string, object> responseObject in responseObjects)
					{
						if((responseObject["messageType"] as string) == "pong") {
							Debug.Log("Ping message!");
						} 
						else if((responseObject["messageType"] as string) == "userInfo") {
							Debug.Log("Connection authentication complete");
							IsConnected = true;
							if(Connected != null) 
							{
								Connected(this, new EventArgs());
							}
						} else {
							MessageQueue.Enqueue(responseObject);
						}
					}
				}
			} 
		}
	}
	
	public  void Send(String data) {
		Debug.Log ("sending message: " + data);
		var byteData = Encoding.ASCII.GetBytes(data);
		client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);
	}

	private void SendAuthenticationMessage()
	{
		Debug.Log("Sending authentication message");
		Send ("{\"messageType\":\"authenticate\", \"token\":\""+(Authentication.Token)+"\"}");
	}
	
	private  void SendCallback(IAsyncResult ar) {
		try {
			client.EndSend(ar);
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}
}