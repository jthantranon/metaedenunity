using UnityEngine;
using System.Collections;

public class CommandShellDeployer : MonoBehaviour {
	public GameObject commandShellPrefab;
	public int count = 1;

	private bool placing;
	private GameObject placingObject;
	private CommandShell[] commandShells;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(placing) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var isRaycastHit = Physics.Raycast(ray, out hit, 1000.000f);
			if(Input.GetMouseButtonDown(0)) {
				var isInsideRange = false;
				foreach(var commandShell in commandShells)
				{
					if(Utility.FlatDistance(commandShell.transform.position, hit.point) < commandShell.commandRange) {
						isInsideRange = true;
						break;
					}
				}
				if(isInsideRange) {
					--count;
					placing = false;
					placingObject.BroadcastMessage("OnPlaced");
					Destroy(gameObject);
				}
			} else {
				if (isRaycastHit)
				{
					if(hit.collider.tag == "Floor") {
						placingObject.transform.position = new Vector3(Mathf.RoundToInt(hit.point.x / 5) * 5, 0, Mathf.RoundToInt(hit.point.z / 5) * 5);
					}
				}
			}
		} 
	}

	public void DeployShell()
	{
		if(count > 0) {
			placing = true;
			commandShells = FindObjectsOfType<CommandShell>();
			placingObject = Instantiate(commandShellPrefab) as GameObject;
		}
	}
}
