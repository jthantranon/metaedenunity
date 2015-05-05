using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InstalledProgram))]
public class Compiler2 : MonoBehaviour {
	private InstalledProgram installedProgram;
	private ProgressBar progressBar;
	private float compileTimer;
	private bool compiling;
	private GameObject compilingObjectPrefab;
	private string compilingObjectName;
	private GameObject compilerOptions;

	public GameObject inventoryContainer;
	public GameObject placementPrefab;
	public float compileSpeed = 3f;
	public GameObject fileSystemPrefab;
	public GameObject shellPrefab;
	public GameObject currencyMinerPrefab;
	public GameObject stealthFieldPrefab;

	// Use this for initialization
	void Start () {
		compilerOptions = GameObject.Find("CompilerOptions");
		progressBar = transform.FindChild("ProgressBar").GetComponent<ProgressBar>();
		progressBar.gameObject.SetActive(false);
		installedProgram = GetComponent<InstalledProgram>();
		if(!installedProgram.IsOwned) {
			compilerOptions.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(compiling) {
			compileTimer -= Time.deltaTime;
			if(compileTimer <= 0)
			{
				compiling = false;
				progressBar.gameObject.SetActive(false);
				var newObject = (GameObject)Instantiate(placementPrefab);
				newObject.GetComponent<InventoryItem>().placementPrefab = compilingObjectPrefab;
				newObject.GetComponent<InventoryItem>().SetItemName(compilingObjectName);
				newObject.transform.SetParent(inventoryContainer.transform, false);
			} else {
				var scale = (compileSpeed - compileTimer) / compileSpeed;
				progressBar.SetProgress(scale);
			}
		}
	}

	public void OnDecrypted()
	{
		compilerOptions.SetActive(true);
	}

	public void BeginCompile()
	{
		compiling = true;
		compileTimer = compileSpeed;
		progressBar.gameObject.SetActive(true);
	}

	public void CompileFileSystem()
	{
		if(!compiling) {
			compilingObjectPrefab = fileSystemPrefab;
			compilingObjectName = "File System";
			BeginCompile();
		}
	}

	public void CompileShell()
	{
		if(!compiling) {
			compilingObjectPrefab = shellPrefab;
			compilingObjectName = "Shell";
			BeginCompile();
		}
	}

	public void CompileCurrencyMiner()
	{
		if(!compiling) {
			compilingObjectPrefab = currencyMinerPrefab;
			compilingObjectName = "Currency Miner";
			BeginCompile();
		}
	}

	public void CompileStealthField()
	{
		if(!compiling) {
			compilingObjectPrefab = stealthFieldPrefab;
			compilingObjectName = "Stealth Field";
			BeginCompile();
		}
	}
}
