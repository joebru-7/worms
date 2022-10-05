using System.Collections;
using UnityEngine;

public class Weapon1 : IWeapon
{
	public GameObject BulletPrefab;
	public Transform SpawnPoint;
	private Transform myTrans;
	private Transform parentTrans;
	public static BulletPool pool;

	public void Start()
	{
		if (pool == null)
			pool = new BulletPool(BulletPrefab);
		myTrans = GetComponent<Transform>();
		parentTrans = GetComponentInParent<Transform>();
	}
	public override void Shoot()
	{
		//crate bullet
		//var bullet = GameObject.Instantiate<Rigidbody>(BulletPrefab,new Vector3(0,0,0),Quaternion.identity);

		var bullet = pool.Get();
		
		bullet.transform.position = SpawnPoint.position;
		bullet.GetComponent<Rigidbody>().AddForce((SpawnPoint.position- myTrans.position).normalized * 1000);

	}

	public override void Aim(float rad)
	{
		transform.Rotate(Vector3.right, Mathf.Rad2Deg * rad);
	}
}