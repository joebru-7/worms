using UnityEngine;

public class DebugWeapon : IWeapon
{
	public override void Shoot()
	{
		Debug.Log("Pew");
	}
}
