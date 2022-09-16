using System.Collections;
using UnityEngine;

public class Weapon1 :  IWeapon
{
	public GameObject BulletPrefab;
	public Transform SpawnPoint;
	private Transform myTrans;
	public static BulletPool pool;

	public void Start()
	{
		if (pool == null)
			pool = new BulletPool(BulletPrefab);
		myTrans = GetComponent<Transform>();
	}
	public override void Shoot()
	{
		//crate bullet
		//var bullet = GameObject.Instantiate<Rigidbody>(BulletPrefab,new Vector3(0,0,0),Quaternion.identity);

		var bullet = pool.Get();
		
		bullet.transform.position = SpawnPoint.position;
		bullet.GetComponent<Rigidbody>().AddForce((SpawnPoint.position- myTrans.position).normalized * 1000);

	}
}