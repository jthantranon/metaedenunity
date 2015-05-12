using System;
using UnityEngine;

public static class Utility
{
	/// <summary>
	/// Since some objects are super-tall, we use this to calculate distance along the y-plane.
	/// </summary>
	/// <returns>The distance.</returns>
	/// <param name="position1">Position1.</param>
	/// <param name="position2">Position2.</param>
	public static float FlatDistance(Vector3 position1, Vector3 position2)
	{
		return Vector3.Distance(new Vector3(position1.x, 0, position1.z), new Vector3(position2.x, 0, position2.z));
	}
}

