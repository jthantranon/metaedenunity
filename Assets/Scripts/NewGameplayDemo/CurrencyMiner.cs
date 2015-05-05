using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InstalledProgram))]
public class CurrencyMiner : MonoBehaviour {
	private InstalledProgram installedProgram;
	private ProgressBar progressBar;
	private Resources resources;

	public float miningSpeed = 3;
	public float miningTimer;
	public float miningAmount;

	// Use this for initialization
	void Start () {
		progressBar = transform.FindChild("ProgressBar").GetComponent<ProgressBar>();
		resources = GameObject.Find("Resources").GetComponent<Resources>();
		installedProgram = GetComponent<InstalledProgram>();
	}
	
	// Update is called once per frame
	void Update () {
		if(installedProgram.IsOwned && !installedProgram.placing) {
			progressBar.gameObject.SetActive(true);
			miningTimer -= Time.deltaTime;
			if(miningTimer <= 0) {
				resources.AddCurrency(miningAmount);
				miningTimer += miningSpeed;
			}
			var scale = (miningSpeed - miningTimer) / miningSpeed;
			progressBar.SetProgress(scale);
		}
	}
}
