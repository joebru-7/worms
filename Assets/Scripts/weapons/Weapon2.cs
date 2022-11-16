using System;
using System.Collections;
using UnityEngine;

public class Weapon2 : IWeapon
{
	[SerializeField]
	private Transform _spawnPoint;

	[SerializeField]
	private int _particleAmt = 1000;

	[SerializeField]
	private readonly int _damage = 10;


	private ParticleSystem _ps;

	public void Start()
	{
		_ps = GetComponentInChildren<ParticleSystem>();
	}

	public void RenderRay(Vector3 from,Vector3 to)
	{
		_ps.Clear();
		var step = (to-from) / _particleAmt;
		for (int i = 0; i < _particleAmt; i++)
		{
			var e = new ParticleSystem.EmitParams
			{
				applyShapeToPosition = false,
				position = step * i + from
			};
			_ps.Emit(e,1);
		}
		_ps.Play();
	}
	public override void Shoot()
	{
		bool ishit = Physics.Raycast(_spawnPoint.position, (_spawnPoint.position - transform.position).normalized, out var hit);
		if (!ishit)
		{
			RenderRay(_spawnPoint.position, _spawnPoint.position+(_spawnPoint.position - transform.position).normalized*100);
			return;
		}
		else 
		{
			RenderRay(_spawnPoint.position, hit.point);
			if (hit.collider.gameObject.TryGetComponent<PlayerController>(out var player))
				player.Damage(_damage);
		}
	
	}

	public override void Aim(float rad)
	{
		transform.Rotate(Vector3.right, Mathf.Rad2Deg * rad);
	}
}