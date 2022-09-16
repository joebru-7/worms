using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Triangle
{
	public Vector3[] verts;
	public Vector3 a { get => verts[0]; set { verts[0] = value; } }
	public Vector3 b { get => verts[1]; set { verts[1] = value; } }
	public Vector3 c { get => verts[2]; set { verts[2] = value; } }

	public Triangle(Vector3 a,Vector3 b,Vector3 c)
	{
		verts = new Vector3[3];
		this.a = a;
		this.b = b;
		this.c = c;
	}

	void AddToList( List<Vector3> li)
	{
		li.Add(a);
		li.Add(b);
		li.Add(c);

	}
}



public class MeshModify : MonoBehaviour
{
	public Mesh mesh;

	// Start is called before the first frame update
	void Start()
	{
		var a = GetComponent<MeshFilter>();
		var m = a.mesh;
		var x = m.GetVertexBuffer(0);
		x.Release();

		//List<Vector3> li = new();
		var verts = m.vertices;

		List<Triangle> tris = new();

		for (int i = 0; i < verts.Length / 3; i++)
		{
			tris.Add(new Triangle(verts[i], verts[i + 1], verts[i + 2]));
		}

		verts[2] = new Vector3(1, 1, 1);
		//li.Add(new Vector3(2,2,10));
		m.SetVertices(verts);

		m.Optimize();

		var tri = m.GetTriangles(0);

		MeshCollider coll = GetComponent<MeshCollider>();
		coll.sharedMesh = mesh;


	}

	// Update is called once per frame
	void Update()
	{


	}
}
