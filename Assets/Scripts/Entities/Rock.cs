using UnityEngine;
using System.Collections;

public class Rock : Interactable {
	public int resourceCount;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Interact (GameObject player)
	{
		var playerController = player.GetComponent<PlayerController>();
		playerController.AddItem(Items.Rock, resourceCount);
		Destroy(gameObject);
	}
}
