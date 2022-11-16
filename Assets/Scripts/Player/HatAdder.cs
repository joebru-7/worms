using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HatAdder:MonoBehaviour
{
	[SerializeField] 
	private static List<GameObject> hats;
	private static System.Random rand = new(1);

	private void Start()
	{
		hats ??= Resources.LoadAll<GameObject>("Hats").ToList();

		int selection = rand.Next()%hats.Count;
		Instantiate(hats[selection],transform);
		hats.RemoveAt(selection);
	}
}
