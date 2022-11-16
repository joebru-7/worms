using System.Collections;
using UnityEngine;

public class Weapon1 : IWeapon
{
	private Transform myTrans;
	private static BulletPool pool;
	[SerializeField]
	private Transform SpawnPoint;
	[SerializeField]
	private GameObject BulletPrefab;
	[SerializeField]

	private float force = 1000;

	public void Start()
	{
		pool ??= new BulletPool(BulletPrefab);
		myTrans = GetComponent<Transform>();
	}
	public override void Shoot()
	{
		var bullet = pool.Get();
		
		bullet.transform.position = SpawnPoint.position;
		bullet.GetComponent<Rigidbody>().AddForce((SpawnPoint.position- myTrans.position).normalized * force);

	}

	public override void Aim(float rad)
	{
		transform.Rotate(Vector3.right, Mathf.Rad2Deg * rad);
	}
}