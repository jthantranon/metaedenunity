using UnityEngine;
using System.Collections;

public class PillarFloor : MonoBehaviour {
	private Pillar[,] pillars;

	public GameObject pillarPrefab;
	public int rowCount;
	public int columnCount;
	public bool animate = false;


	// Use this for initialization
	void Start () {
		pillars = new Pillar[rowCount, columnCount];
		float hexOffset = .577f * 1.5f;
		float startX = -rowCount / 2f;
		float startZ = -columnCount / 2f;
		for(var i = 0; i < rowCount; ++i)
		{
			for(var j = 0; j < columnCount; ++j)
			{
				var pillarObject = Instantiate(pillarPrefab) as GameObject;

				pillarObject.transform.position = new Vector3(startX + j * hexOffset, 0, startZ + i+ (j % 2 == 1 ? .5f : 0) );

				var pillarComponent = pillarObject.GetComponent<Pillar>();
				pillarComponent.height = Mathf.PI + Mathf.Sin(i + j);

				pillarObject.transform.SetParent(transform);

				pillars[i, j] = pillarComponent;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(animate) {
			var timeChange = Time.unscaledTime;
			var pillarComponent = (Pillar)null;
			for(var i = 0; i < rowCount; ++i)
			{
				for(var j = 0; j < columnCount; ++j)
				{
					pillarComponent = pillars[i, j];
					pillarComponent.height = Mathf.Sin(timeChange + pillarComponent.transform.position.magnitude);
				}
			}
		}
	}
}
