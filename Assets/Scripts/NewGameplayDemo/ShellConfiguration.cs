using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShellConfiguration : MonoBehaviour, IHasChanged {
	public ShellSlot[] slots;
	public ShellSlot[] availableUpgrades;

	private CommandShell currentShell;


	void Start()
	{
		HasChanged();
	}

	public CommandShell CurrentShell
	{
		get { return currentShell; }
		set { 
			if(currentShell != value) 
			{
				currentShell = value;
				var allUpgrades = new List<ShellUpgradeIcon>();
				foreach(var shellSlot in slots)
				{
					if(shellSlot.transform.childCount > 0) 
					{
						var oldChild = shellSlot.transform.GetChild(0);
						allUpgrades.Add(oldChild.GetComponent<ShellUpgradeIcon>());
						oldChild.SetParent(null);
					}
				}
				foreach(var shellSlot in availableUpgrades)
				{
					if(shellSlot.transform.childCount > 0) 
					{
						var oldChild = shellSlot.transform.GetChild(0);
						allUpgrades.Add(oldChild.GetComponent<ShellUpgradeIcon>());
						oldChild.SetParent(null);
					}
				}
				var shellUpgrades = currentShell.gameObject.GetComponents<ShellUpgrade>();
				var nextSlot = 0;
				for(var i = 0; i < shellUpgrades.Length; ++i)
				{
					var upgrade = shellUpgrades[i];
					for(var j = 0; j < allUpgrades.Count; ++j) 
					{
						var up = allUpgrades[j];
						if(up.upgradeName == upgrade.UpgradeName)
						{
							up.transform.SetParent(slots[nextSlot++].transform);
							up.transform.position = up.transform.parent.position;
							allUpgrades.Remove(up);
							break;
						}
					}
				}
				var nextAvailable = 0;
				foreach(var remaining in allUpgrades)
				{
					
					remaining.transform.SetParent(availableUpgrades[nextAvailable++].transform);
					remaining.transform.position = remaining.transform.parent.position;
				}
			}
		}
	}

	public void HasChanged ()
	{
		if(currentShell != null) {
			RemoveOldShellComponents();
			AddNewShellComponents();
		}
	}

	void RemoveOldShellComponents()
	{
		var upgrades = currentShell.GetComponents<ShellUpgrade>();
		foreach(var upgrade in upgrades)
		{
			upgrade.OnRemove();
			Destroy(upgrade);
		}
	}

	void AddNewShellComponents()
	{
		foreach(var shellSlot in slots)
		{
			if(shellSlot.transform.childCount > 0) 
			{
				var upgradeType = shellSlot.transform.GetChild(0).GetComponent<ShellUpgradeIcon>().upgradeName;
				switch(upgradeType) {
				case "Mobility":
					currentShell.gameObject.AddComponent<MobilityUpgrade>().OnAdd();
					break;
				case "Stealth":
					currentShell.gameObject.AddComponent<StealthUpgrade>().OnAdd();
					break;
				}
			}
		}
	}
}
