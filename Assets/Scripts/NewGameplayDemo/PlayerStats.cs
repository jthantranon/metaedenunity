using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	public int researchPoints = 50;
	public int maxInventorySize = 5;
	public int currentInventorySize = 0;
	public Text inventoryText;
	public Text researchPointsText;

	void Start()
	{
		UpdateInventoryText();
		UpdateResearchPointsText();
	}

	public void IncreaseInventorySize()
	{
		var rank = maxInventorySize - 4;
		var cost = rank * 100;
		if(researchPoints >= cost)
		{
			researchPoints -= cost;
			maxInventorySize++;
			UpdateInventoryText();
			UpdateResearchPointsText();
		}
	}

	public void ItemAdded()
	{
		++currentInventorySize;
		researchPoints += 25;
		UpdateInventoryText();
		UpdateResearchPointsText();
	}

	public void ItemRemoved()
	{
		--currentInventorySize;
		UpdateInventoryText();
	}

	private void UpdateInventoryText()
	{
		inventoryText.text = string.Format("{0} / {1}", currentInventorySize, maxInventorySize);
	}

	private void UpdateResearchPointsText()
	{
		researchPointsText.text = "Research Points: " + researchPoints;
	}
}
