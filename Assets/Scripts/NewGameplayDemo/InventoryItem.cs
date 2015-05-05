using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryItem : MonoBehaviour {
	private bool placing;
	private GameObject placementObject;
	private string itemName;
	private Text text;

	public GameObject placementPrefab;


	// Use this for initialization
	void Start () {
		var button = GetComponent<Button>();
		button.onClick = new Button.ButtonClickedEvent();
		button.onClick.AddListener(StartPlacement);
	}
	
	// Update is called once per frame
	void Update () {
		if(placing) {
			if(Input.GetMouseButtonDown(0)) {
				placing = false;
				placementObject.GetComponent<InstalledProgram>().placing = false;
				Destroy(gameObject);
			} else {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, 1000.000f))
				{
					if(hit.collider.tag == "Floor") {
						placementObject.transform.position = new Vector3(Mathf.RoundToInt(hit.point.x / 5) * 5, 0, Mathf.RoundToInt(hit.point.z / 5) * 5);
					}
				}
			}
		}
	}

	public void SetItemName(string name)
	{
		itemName = name;
		text = transform.FindChild("Text").gameObject.GetComponent<Text>();
		text.text = "Install " + name;
	}

	void StartPlacement()
	{
		placing = true;
		placementObject = (GameObject)Instantiate(placementPrefab);
		placementObject.GetComponent<InstalledProgram>().placing = true;
		placementObject.GetComponent<InstalledProgram>().IsOwned = true;
	}
}
