using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryItem : MonoBehaviour {
	private Resources resources;
	private Button button;
	private bool placing;
	private GameObject placementObject;
	private string itemName;
	private Text text;
	private CommandShell[] commandShells;
	private PlayerStats playerStats;

	public GameObject placementPrefab;


	// Use this for initialization
	void Start () {
		button = GetComponent<Button>();
		button.onClick = new Button.ButtonClickedEvent();
		button.onClick.AddListener(StartPlacement);
		resources = FindObjectOfType<Resources>();
		playerStats = FindObjectOfType<PlayerStats>();
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
					if(Utility.FlatDistance(commandShell.transform.position, hit.point) < commandShell.Aura.range) {
						isInsideRange = true;
						break;
					}
				}
				if(isInsideRange) {
					placing = false;
					placementObject.GetComponent<InstalledProgram>().placing = false;
					placementObject.GetComponent<Collider>().enabled = true;
					placementObject.BroadcastMessage("OnPlaced");
					Destroy(gameObject);
				}
			} else {
				if (isRaycastHit)
				{
					if(hit.collider.tag == "Floor") {
						placementObject.transform.position = new Vector3(Mathf.RoundToInt(hit.point.x / 5) * 5, 0, Mathf.RoundToInt(hit.point.z / 5) * 5);
					}
				}
			}
		} else {
			var remainingMemory = resources.TotalMemory - resources.CurrentMemory;
			var remainingProcessingPower = resources.TotalProcessingPower - resources.CurrentProcessingPower;
			var installedProgram = placementPrefab.GetComponent<InstalledProgram>();
			var generatesMemory = placementPrefab.GetComponent<FileSystem>() != null;
			var generatesProcessingPower = placementPrefab.GetComponent<Shell>() != null;
			if((installedProgram.memory > remainingMemory && !generatesMemory) 
			   || (installedProgram.processingPower > remainingProcessingPower && !generatesProcessingPower)) {
				button.interactable = false;
			} else {
				button.interactable = true;
			}
		}
	}

	public void SetItemName(string name)
	{
		var installedProgram = placementPrefab.GetComponent<InstalledProgram>();
		text = transform.FindChild("Text").gameObject.GetComponent<Text>();
		text.text = "Install " + name + " (" + installedProgram.memory + "/" + installedProgram.processingPower + ")";
	}

	void StartPlacement()
	{
		playerStats.ItemRemoved();
		placing = true;
		placementObject = (GameObject)Instantiate(placementPrefab);
		placementObject.GetComponent<InstalledProgram>().placing = true;
		placementObject.GetComponent<InstalledProgram>().IsOwned = true;
		placementObject.GetComponent<Collider>().enabled = false;

		commandShells = FindObjectsOfType<CommandShell>();
	}
}
