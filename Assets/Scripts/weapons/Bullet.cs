using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;
public class Bullet : MonoBehaviour
{
	Rigidbody rb;
	Collider col;
	Transform trans;

	public GameObject Explosion;
	public GameObject Carver;

	private GameObject _explosion;
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
		trans = GetComponent<Transform>();
		//Carver = GameObject.CreatePrimitive(PrimitiveType.Cube);
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
		//trans.localScale = Vector3.one;
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
		col.enabled = false;

		_explosion = Instantiate(Explosion,transform.position,Quaternion.identity);
		Invoke(nameof(ExplosionCleanup), 1);

		var _carver = Instantiate(Carver, transform.position, transform.rotation);
		//Carver.transform.SetPositionAndRotation(transform.position, transform.rotation);
		var colls = Physics.OverlapSphere(transform.position, 3);

		foreach (var col in colls)
		{
			switch(col.tag)
			{
				case "Terrain":
					//Debug.Log("Terr " + col);

					Model m = CSG.Subtract(col.gameObject, _carver);
					Mesh mesh = m.mesh;
					mesh.name = "custom";
					mesh.Optimize();
					Debug.Log(mesh.triangles.Length);
					
					col.GetComponent<MeshFilter>().mesh = mesh;
					col.GetComponent<MeshFilter>().sharedMesh = mesh;
					col.GetComponent<MeshCollider>().sharedMesh = mesh;

					col.GetComponent<MeshRenderer>().materials = m.materials.ToArray();
					col.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
					col.transform.localScale = Vector3.one;

					break;
				case "Player":
					//Debug.Log("Play " + col);
					col.GetComponent<PlayerController>().Damage(5);
					break;
				default:
					break;
			}
		}

		Destroy(_carver);

		//Model result = CSG.Subtract(cube, sphere);

		
	}

	public void ExplosionCleanup()
	{
		trans.localScale = Vector3.one *.1f;
		Destroy(_explosion);
		gameObject.SetActive(false);
	}
}
