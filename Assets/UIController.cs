using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	public GameObject configurationUI;
	public GameObject gameUI;
	public ShellConfiguration shellConfiguration;
	public ShellCameraSwitcher shellCameraSwitcher;
	public Transform resourcePointPanel;

	public void SwitchToConfiguration()
	{
		gameUI.SetActive(false);
		configurationUI.SetActive(true);
		resourcePointPanel.gameObject.SetActive(false);
		shellConfiguration.CurrentShell = shellCameraSwitcher.currentShell;
		shellCameraSwitcher.enabled = false;

	}

	public void SwitchToGame()
	{
		gameUI.SetActive(true);
		configurationUI.SetActive(false);
		resourcePointPanel.gameObject.SetActive(false);
		shellCameraSwitcher.enabled = true;
	}

	public void SwitchToResourcePoints()
	{
		gameUI.SetActive(true);
		configurationUI.SetActive(false);
		resourcePointPanel.gameObject.SetActive(true);
		shellCameraSwitcher.enabled = false;
	}
}
