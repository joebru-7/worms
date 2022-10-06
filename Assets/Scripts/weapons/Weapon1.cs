using System.Collections;
using UnityEngine;

public class Weapon1 : IWeapon
{
	public GameObject BulletPrefab;
	public Transform SpawnPoint;
	private Transform myTrans;
	public static BulletPool pool;
	public float force = 1000;

	public void Start()
	{
		pool ??= new BulletPool(BulletPrefab);
		myTrans = GetComponent<Transform>();
	}
	public override void Shoot()
	{
		//crate bullet
		//var bullet = GameObject.Instantiate<Rigidbody>(BulletPrefab,new Vector3(0,0,0),Quaternion.identity);

		var bullet = pool.Get();
		
		bullet.transform.position = SpawnPoint.position;
		bullet.GetComponent<Rigidbody>().AddForce((SpawnPoint.position- myTrans.position).normalized * force);

	}

	public override void Aim(float rad)
	{
		transform.Rotate(Vector3.right, Mathf.Rad2Deg * rad);
	}
}