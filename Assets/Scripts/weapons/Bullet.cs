using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private Rigidbody _rb;
	private Collider _col;
	private Transform _transform;

	public GameObject ExplosionPrefab;

	private GameObject _explosion;
	
	void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_col = GetComponent<Collider>();
		_transform = GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position.y < -100)
			gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		if (_rb == null) _rb = GetComponent<Rigidbody>();
		_rb.useGravity = true;

		if (_col == null) _col = GetComponent<Collider>();
		_col.enabled = true;
	}

	private void OnDisable()
	{
		_rb.useGravity = false;
		_rb.velocity = Vector3.zero;
		_col.enabled = false;
	}



	private void OnCollisionEnter(Collision collision)
	{
		_rb.useGravity = false;
		_rb.velocity = Vector3.zero;
		_col.enabled = false;

		_explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
		Invoke(nameof(ExplosionCleanup), 1);

		var colls = Physics.OverlapSphere(transform.position, 3);

		foreach (var col in colls)
		{
			switch(col.tag)
			{
				case "Player":
					col.GetComponent<PlayerController>().Damage(5);
					break;
				default:
					break;
			}
		}
	}

	public void ExplosionCleanup()
	{
		_transform.localScale = Vector3.one *.1f;
		Destroy(_explosion);
		gameObject.SetActive(false);
	}
}
