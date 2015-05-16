using UnityEngine;
using System.Collections;

public class ShellCameraSwitcher : MonoBehaviour {
	public float cameraPanSpeed;
	public Transform mobileShell;
	public Vector3 commandShellViewOffset;

	private CommandShell[] availableShells;
	private CommandShell currentShell;
	private int currentShellIndex;
	private bool inMobileShell;
	private float mobileRotateX;
	private float mobileRotateY;

	// Use this for initialization
	void Start () {
		availableShells = FindObjectsOfType<CommandShell>();
		SwitchToNextCommandShell();
	}
	
	// Update is called once per frame
	void Update () {
		if(!inMobileShell) {
			UpdateCommandShellCameraPosition();
		} else {
			UpdateMobileShellCameraPosition();
		}
	}

	private void UpdateCommandShellCameraPosition()
	{
		availableShells = FindObjectsOfType<CommandShell>();
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
		Physics.Raycast(ray, out hit, 1000.000f);
		var cameraOffset = new Vector2(hit.point.x - transform.position.x, hit.point.z - transform.position.z);
		float newCameraX = transform.position.x;
		float newCameraZ = transform.position.z;
		if(Input.mousePosition.x <= 0) {
			newCameraX -= cameraPanSpeed * Time.deltaTime;
		}
		if(Input.mousePosition.x >= Screen.width) {
			newCameraX += cameraPanSpeed * Time.deltaTime;
		}
		if(Input.mousePosition.y <= 0) {
			newCameraZ -= cameraPanSpeed * Time.deltaTime;
		}
		if(Input.mousePosition.y >= Screen.height) {
			newCameraZ += cameraPanSpeed * Time.deltaTime;
		}
		var newPosition = new Vector2(newCameraX, newCameraZ);
		var shellPositionVector2 = new Vector2(currentShell.transform.position.x, currentShell.transform.position.z) - cameraOffset;
		var distanceFromShell = Vector2.Distance(newPosition, shellPositionVector2);
		if(distanceFromShell > currentShell.commandRange) {
			var vec = (newPosition - shellPositionVector2).normalized;
			newPosition = shellPositionVector2 + (vec * currentShell.commandRange);
		}
		transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.y);
	}

	private void UpdateMobileShellCameraPosition()
	{
		transform.position = mobileShell.position + Vector3.back * 10;
		transform.LookAt(transform.position + Vector3.forward);
		mobileRotateX += 5f * Input.GetAxis("Mouse X");
		mobileRotateY -= 5f * Input.GetAxis("Mouse Y");
		if(mobileRotateY < -90) {
			mobileRotateY = -90;
		}
		if(mobileRotateY > 90) {
			mobileRotateY = 90;
		}
		transform.Rotate(mobileRotateY, mobileRotateX, 0);
	}

	public void SwitchToNextCommandShell()
	{
		++currentShellIndex;
		if(currentShellIndex >= availableShells.Length) {
			currentShellIndex = 0;
		}
		SwitchCommandShell();
	}

	public void SwitchToPreviousCommandShell()
	{
		--currentShellIndex;
		if(currentShellIndex <= 0) {
			currentShellIndex = availableShells.Length - 1;
		}
		SwitchCommandShell();
	}

	private void SwitchCommandShell()
	{
		if(currentShell != null) {
			currentShell.IsActive = false;
		}
		currentShell = availableShells[currentShellIndex];
		currentShell.IsActive = true;
		inMobileShell = false;
		transform.position = currentShell.transform.position + commandShellViewOffset;
		transform.LookAt(new Vector3(currentShell.transform.position.x, 0, currentShell.transform.position.z));
	}

	public void SwitchToMobileShell()
	{
		currentShellIndex = -1;
		currentShell.IsActive = false;
		currentShell = null;
		inMobileShell = true;
	}
}
