using UnityEngine;
using System.Collections;

public class Container : Interactable {
	public int capacity;
	public string[] itemIds;

	public override void Interact (GameObject player)
	{
		Debug.Log("Container interact");
	}
}
