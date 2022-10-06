
using UnityEngine;

public abstract class IWeapon:MonoBehaviour
{
	abstract public void Shoot();
	abstract public void Aim(float rad);
}
