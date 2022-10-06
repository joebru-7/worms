using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HatAdder:MonoBehaviour
{
	public static List<GameObject> hats;

	private void Start()
	{
		hats ??= Resources.LoadAll<GameObject>("Hats").ToList();

		int selection = new System.Random().Next()%hats.Count;
		Instantiate(hats[selection],transform);
		hats.RemoveAt(selection);
	}
}
