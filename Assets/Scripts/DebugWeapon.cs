using UnityEngine;

public class DebugWeapon : IWeapon
{
	public override void Aim(float rad)
	{
		return;
	}

	public override void Shoot()
	{
		Debug.Log("Pew");
	}
}
