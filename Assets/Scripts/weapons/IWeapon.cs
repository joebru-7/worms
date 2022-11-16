
using UnityEngine;

//Not a interface to make it play nice with inspector
public abstract class IWeapon:MonoBehaviour
{
	abstract public void Shoot();
	abstract public void Aim(float rad);
}
