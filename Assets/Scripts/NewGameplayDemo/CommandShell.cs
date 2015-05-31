using UnityEngine;
using System.Collections;

public class CommandShell : AuraComponent {
	public int hitPoints = 10;
	public int concealmentRating = 75;
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
