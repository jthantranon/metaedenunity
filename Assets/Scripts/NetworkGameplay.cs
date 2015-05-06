using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkMessageHandler))]
public class NetworkGameplay : MonoBehaviour {
	private NetworkMessageHandler networkMessageHandler;

	public GameObject wallPrefab;
	public GameObject smallRockPrefab;
	public GameObject teleporterPrefab;
	public GameObject statuePrefab;
	public GameObject wispPrefab;
	public GameObject floor;

	// Use this for initialization
	void Start () {
		networkMessageHandler = GetComponent<NetworkMessageHandler>();
		networkMessageHandler.CharacterInfo += OnCharacterInfo;
		networkMessageHandler.ZoneInfo += OnZoneInfo;
		networkMessageHandler.JoinedInstance += OnJoinedInstance;
		networkMessageHandler.WorldStateDelta += OnWorldStateDelta;
	}

	// Update is called once per frame
	void Update () {
	
	}
	void Destroy()
	{
		networkMessageHandler.CharacterInfo -= OnCharacterInfo;
		networkMessageHandler.ZoneInfo -= OnZoneInfo;
		networkMessageHandler.JoinedInstance -= OnJoinedInstance;
		networkMessageHandler.WorldStateDelta -= OnWorldStateDelta;
	}

	private void OnJoinedInstance(string instanceId)
	{
		Debug.Log("Joined instance");
		Application.LoadLevel("FirstScene");
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
		AdjustFloor (zoneInfo);
		AddEntities (zoneInfo);
		AddWalls (zoneInfo);
	}

	void AdjustFloor (IDictionary<string, object> zoneInfo)
	{
		var height = System.Convert.ToSingle (zoneInfo ["height"]);
		var width = System.Convert.ToSingle (zoneInfo ["width"]);
		floor.transform.localScale = new Vector3 (width / 10f, 1, height / 10f);
		floor.transform.position = new Vector3 (width / 2f, 0, height / 2f);
	}

	void AddEntities (IDictionary<string, object> zoneInfo)
	{
		var entities = zoneInfo ["entities"] as IEnumerable<object>;
		foreach (IDictionary<string, object> entity in entities) {
			var type = entity ["type"] as string;
			switch(type){
			case "rock":
				Debug.Log ("this is a rock");
				break;
			case "teleporter":
				AddTeleporter(entity);
				break;
			case "statue":
				AddStatue(entity);
				break;
			case "wisp":
				AddWisp(entity);
				break;
			default:
				Debug.LogWarning ("this is default:"+ type);
				break;
			}
		}
	}

	void AddWisp (IDictionary<string,object> entity){
		Debug.LogWarning ("rendering Wisp");
		var wispObject = (GameObject)Instantiate (wispPrefab);
		var position = ConvertToVector3 (entity ["position"]);
		wispObject.transform.position = position;
	}

	void AddStatue (IDictionary<string,object> entity){
		Debug.Log ("rendering Statue");
		var statueObject = (GameObject)Instantiate (statuePrefab);
		var position = ConvertToVector3 (entity ["position"]);
		statueObject.transform.position = position;
	}

	void AddTeleporter (IDictionary<string, object> entity){
		Debug.Log ("rendering teleporter");
		var teleporterObject = (GameObject)Instantiate (teleporterPrefab);
		var position = ConvertToVector3 (entity ["position"]);
		teleporterObject.transform.position = position;
	}

	void AddWalls (IDictionary<string, object> zoneInfo)
	{
		var walls = zoneInfo ["walls"] as IEnumerable<object>;
		foreach (IDictionary<string, object> wall in walls) {
			var startVector = ConvertToVector3 (wall ["start"]);
			var endVector = ConvertToVector3 (wall ["end"]);
			var middle = new Vector3 ((startVector.x + endVector.x) / 2f, 0, (startVector.z + endVector.z) / 2f);
			var diff = new Vector3 ((startVector.x - endVector.x), 0, (startVector.z - endVector.z));
			var magnitude = diff.magnitude;
			var wallObject = (GameObject)Instantiate (wallPrefab, middle, Quaternion.identity);
			wallObject.transform.position = middle;
			wallObject.transform.localScale = new Vector3 (magnitude, 1, 1);
			wallObject.transform.Rotate (new Vector3 (0, Mathf.Atan (diff.z / diff.x) * 180 / Mathf.PI, 0));
			// var meshRenderer = wallObject.GetComponent < MeshRenderer > ();
			// meshRenderer.material.color = new Color(.5,.5,.5);
		}
		//TODO: read and use entity info
	}

	void OnWorldStateDelta (IDictionary<string,object> wsd)
	{
		foreach (var key in wsd.Keys) {
			var firstObj = wsd[key];
//			Debug.LogWarning(key + " = " + firstObj);
			if(firstObj is IDictionary<string,object>){
				var firstDict = firstObj as IDictionary<string,object>;
				foreach (var nextKey in firstDict){
//					Debug.LogWarning(nextKey.Key);
//					Debug.LogWarning(nextKey.Value);
				}
			}
		}
		foreach (var kvp in wsd) {
//			Debug.Log(kvp.Key + " = " + kvp.Value);
		}
	}


	private Vector3 ConvertToVector3(object vecObject)
	{
		var vec = vecObject as IDictionary<string, object>;
		return new Vector3(System.Convert.ToSingle(vec["x"]), 0f, System.Convert.ToSingle(vec["y"]));
	}
}
