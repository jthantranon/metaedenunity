using UnityEngine;
using System.Collections;

public class InstalledProgram : MonoBehaviour {
	public int concealmentRating;
	public int memory;
	public int processingPower;
	public float decryptSpeed;

	public int hitPoints = 10;

	public bool placing;
	public bool isPublic;

	private bool isOwned;
	private float decryptTimer;
	private bool decrypting;
	private ProgressBar progressBar;

	// Use this for initialization
	void Start () {
		try
		{
			progressBar = transform.FindChild("ProgressBar").GetComponent<ProgressBar>();
			transform.FindChild("Encrypted").gameObject.SetActive(!isOwned && !isPublic);
			progressBar.gameObject.SetActive(false);
		} catch
		{}
	}
	
	// Update is called once per frame
	void Update () {
		if(decrypting) {
			decryptTimer -= Time.deltaTime;
			if(decryptTimer <= 0) {
				BroadcastMessage("OnDecrypted");
				IsOwned = true;
				progressBar.gameObject.SetActive(false);
			} else {
				var scale = (decryptSpeed - decryptTimer) / decryptSpeed;
				progressBar.SetProgress(scale);
			}
		}
	}

	public bool IsOwned {
		get { return isOwned; }
		set {
			isOwned = value;
			var encryptedTransform = transform.FindChild("Encrypted");
			if(encryptedTransform != null) {
				encryptedTransform.gameObject.SetActive(!isOwned && !isPublic);
			}
		}
	}

	public void StartDecrypt()
	{
		progressBar.gameObject.SetActive(true);
		decrypting = true;
		decryptTimer = decryptSpeed;
	}

	void OnMoving()
	{}

	void OnPlaced()
	{

	}
}
