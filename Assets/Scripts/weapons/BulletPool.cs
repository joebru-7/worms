using System.Collections.Generic;
using UnityEngine;

public class PoolReturn:MonoBehaviour
{
	private BulletPool pool;

	public void SetPool(BulletPool pool) => this.pool = pool;


	private void OnDisable()
	{
		pool.Return(gameObject);
	}
}

public class BulletPool
{
	public GameObject prototype;
	private Stack<GameObject> _pool = new();

	public BulletPool(GameObject prototype)
	{
		this.prototype = prototype;
	}

	public GameObject Get()
	{
		var bullet = _pool.Count > 0 ? _pool.Pop() : GameObject.Instantiate(prototype);

		if (!bullet.TryGetComponent<PoolReturn>(out var ret))
			ret = bullet.AddComponent<PoolReturn>();
		ret.SetPool(this);
		bullet.SetActive(true);
		return bullet;
	}

	public void Return(GameObject g)
	{
		_pool.Push(g);
	}
}
