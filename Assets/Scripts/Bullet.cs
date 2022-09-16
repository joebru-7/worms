using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	Rigidbody rb;
	Collider col;
	Transform trans;
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
		trans = GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position.y < -100) 
			gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		if (rb == null) rb = GetComponent<Rigidbody>();
		rb.useGravity = true;
		if (col == null) col = GetComponent<Collider>();
		col.enabled = true;
	}

	private void OnDisable()
	{
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
		col.enabled = false;
	}



	private void OnCollisionEnter(Collision collision)
	{
		trans.localScale = Vector3.one;
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
		col.enabled = false;
		

		Invoke(nameof(ExplosionCleanup), 1);
	}

	public void ExplosionCleanup()
	{
		trans.localScale = Vector3.one *.1f;
		gameObject.SetActive(false);
	}
}
