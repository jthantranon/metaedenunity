using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	public GameObject configurationUI;
	public GameObject gameUI;
	public ShellConfiguration shellConfiguration;
	public ShellCameraSwitcher shellCameraSwitcher;

	public void SwitchToConfiguration()
	{
		gameUI.SetActive(false);
		configurationUI.SetActive(true);
		shellConfiguration.CurrentShell = shellCameraSwitcher.currentShell;
		shellCameraSwitcher.enabled = false;

	}

	public void SwitchToGame()
	{
		gameUI.SetActive(true);
		configurationUI.SetActive(false);
		shellCameraSwitcher.enabled = true;
	}
}
