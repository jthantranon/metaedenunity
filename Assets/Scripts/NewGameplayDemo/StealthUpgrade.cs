using UnityEngine;
using System.Collections;

public class StealthUpgrade : ShellUpgrade {

	void Start()
	{
		UpgradeName = "Stealth";
	}

	public override void OnAdd()
	{
		gameObject.AddComponent<StealthField>().concealmentAmount = 30;

	}
	public override void OnRemove()
	{
		Destroy(gameObject.GetComponent<StealthField>());
	}
}
