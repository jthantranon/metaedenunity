using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class GameNetwork : MonoBehaviour {
	private static readonly string PingMessage = "{\"messageType\":\"ping\"}";
	private static readonly float PingInterval = 2f;
	private static readonly int BufferSize = 1024;

	private Socket client;
	private StringBuilder stringBuilder = new StringBuilder();
	private String response = String.Empty;
	private float pingTimer = PingInterval;
	private bool connected;
	private Coroutine receiveCoroutine;

	public string hostname;
	public int port;

	void Start () {
		StartCoroutine(ConnectAsync());
	}

	void Update () {
		pingTimer -= Time.deltaTime;
		if(pingTimer <= 0) {
			Send(PingMessage);
			pingTimer = PingInterval;
		}
	}

	void OnDestroy()
	{
		connected = false;
		StopCoroutine(receiveCoroutine);
		client.Shutdown(SocketShutdown.Both);
		client.Close();
	}

	private IEnumerator ConnectAsync()
	{
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
			client.EndConnect(result);
			connected = true;
			SendAuthenticationMessage();
			receiveCoroutine = StartCoroutine(ReceiveAsync());
		}
		catch(Exception e) 
		{
			Debug.LogException(e);
		}
	}

	private IEnumerator ReceiveAsync()
	{
		while(connected) {
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
					Debug.Log("Response received: " + response);
					//TODO: parse the response JSON and add to the processing queue
					stringBuilder.Remove(0, stringBuilder.Length);
				}
			} 
		}
	}
	
	private void SendAuthenticationMessage()
	{
		Send ("{\"messageType\":\"authenticate\", \"token\":\""+(Authentication.Token)+"\"}");
	}

	private void SendCharacterListRequest()
	{
		Send ("{\"messageType\":\"characterList\"}");
	}
	
	private  void Send(String data) {
		var byteData = Encoding.ASCII.GetBytes(data);
		client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);
	}
	
	private  void SendCallback(IAsyncResult ar) {
		try {
			client.EndSend(ar);
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}
}