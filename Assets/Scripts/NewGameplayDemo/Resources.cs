using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Resources : MonoBehaviour {
	private int totalProcessingPower;
	private int totalMemory;
	private int currentProcessingPower;
	private int currentMemory;
	private float currency;

	public Text memoryText;
	public Text processingPowerText;
	public Text currencyText;

	public int TotalMemory { get { return totalMemory; } }
	public int TotalProcessingPower { get { return totalProcessingPower; } }

	public int CurrentMemory { get { return currentMemory; } }
	public int CurrentProcessingPower { get { return currentProcessingPower; } }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateValues();
		currencyText.text = "Currency: " + currency;
	}

	void UpdateValues()
	{
		var installedPrograms = FindObjectsOfType<InstalledProgram>();
		var fileSystems = FindObjectsOfType<FileSystem>();
		var shells = FindObjectsOfType<Shell>(); 
		totalProcessingPower = 2;
		totalMemory = 2;
		currentMemory = 0;
		currentProcessingPower = 0;
		foreach(var fs in fileSystems)
		{
			var installedProgram = fs.GetComponent<InstalledProgram>();
			if(!installedProgram.placing && installedProgram.IsOwned) {
				totalMemory += fs.memoryGenerated;
			}
		}
		foreach(var shell in shells)
		{	
			var installedProgram = shell.GetComponent<InstalledProgram>();
			if(!installedProgram.placing && installedProgram.IsOwned) {
				totalProcessingPower += shell.processingPowerGenerated;
			}
		}
		foreach(var installedProgram in installedPrograms)
		{
			if(!installedProgram.placing && installedProgram.IsOwned) {
				currentMemory += installedProgram.memory;
				currentProcessingPower += installedProgram.processingPower;
			}
		}
		memoryText.text = "Memory Used: " +  currentMemory + "/" + totalMemory;
		processingPowerText.text = "Processing Power Used: " + currentProcessingPower + "/" + totalProcessingPower;
	}

	public void AddCurrency(float currency) 
	{
		this.currency += currency;
	}
}
