using UnityEngine;
using System.Collections;

public class CommandShell : AuraComponent {
	public bool isActive;
	public bool IsActive {
		get {
			return isActive;
		}
		set {
			isActive = value;
		}
	}

	void Update () {
		Aura.ShowAura(isActive);
	}
}
