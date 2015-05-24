using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Aura))]
public class AuraComponent : MonoBehaviour {
	private Aura aura;
	
	public Aura Aura {
		get {
			if(aura == null) {
				aura = GetComponent<Aura>();
			}
			return aura;
		}
	}
}
