using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerainTest : MonoBehaviour
{
	private Terrain _terr;

	// Start is called before the first frame update
	void Start()
	{
		_terr = GetComponent<Terrain>();

		var b = new bool[100, 100];
		for (var x = 0; x < 100; x++)
			for (var y = 0; y < 100; y++)
				b[x, y] = !(x > 20 && x < 80 && y > 20 && y < 80);
		_terr.terrainData.SetHoles(0, 0, b);

		//_terr.terrainData.SetHoles(5, 5, b);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
