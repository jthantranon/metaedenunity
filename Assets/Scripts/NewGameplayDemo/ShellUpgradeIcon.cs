using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class ShellUpgradeIcon : MonoBehaviour {
	public string upgradeName;
	public Sprite icon;

	[ExecuteInEditMode]
	void Start()
	{
		if(icon != null) 
		{
			var image = GetComponent<Image>();
			image.sprite = icon;
		}
		transform.GetChild(0).GetComponent<Text>().text = upgradeName;
	}
}
