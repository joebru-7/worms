using System.Collections.Generic;
using UnityEngine;

public class PoolReturn:MonoBehaviour
{
	public BulletPool pool;

	private void OnDisable()
	{
		pool.Return(gameObject);
	}
}

public class BulletPool
{
	public GameObject prototype;
	private Stack<GameObject> _pool = new();
	//private Transform holder;

	public BulletPool(GameObject prototype)
	{
		this.prototype = prototype;
		//holder = new GameObject().transform;
	}

	public GameObject Get()
	{
		var bullet = _pool.Count > 0 ? _pool.Pop() : GameObject.Instantiate(prototype);
		//bullet.transform.parent = null;

		if (!bullet.TryGetComponent<PoolReturn>(out var ret))
			ret = bullet.AddComponent<PoolReturn>();
		ret.pool = this;
		bullet.SetActive(true);
		return bullet;
	}

	public void Return(GameObject g)
	{
		//g.transform.parent = holder;
		_pool.Push(g);
	}
}
