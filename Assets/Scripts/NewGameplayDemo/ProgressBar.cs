using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	/// <summary>
	/// Sets the progress.
	/// </summary>
	/// <param name="progress">Progress value between 0 and 1.</param>
	public void SetProgress(float progress)
	{
		transform.localScale = new Vector3(progress, 1, 1);
		transform.localPosition = new Vector3(-.5f + (progress/2f), 2.5f, 0);
	}
}
