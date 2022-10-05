using System;
using System.Collections;
using UnityEngine;

public class Weapon2 : IWeapon
{
	public Transform SpawnPoint;
	private ParticleSystem _ps;
	public int particleAmt = 1000;

	public void Start()
	{
		_ps = GetComponentInChildren<ParticleSystem>();
	}

	public void RenderRay(Vector3 from,Vector3 to)
	{
		_ps.Clear();
		var step = (to-from)/ particleAmt;
		for (int i = 0; i < particleAmt; i++)
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
		bool ishit = Physics.Raycast(SpawnPoint.position, (SpawnPoint.position - transform.position).normalized, out var hit);
		if (!ishit)
		{
			RenderRay(SpawnPoint.position, SpawnPoint.position+(SpawnPoint.position - transform.position).normalized*100);
			return;
		}
		else if(hit.collider.gameObject.TryGetComponent<PlayerController>(out var player))
		{
			player.Damage(10);
		}
	
	}

	public override void Aim(float rad)
	{
		transform.Rotate(Vector3.right, Mathf.Rad2Deg * rad);
	}
}