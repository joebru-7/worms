using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

	public void AddToList( List<Vector3> li)
	{
		li.Add(a);
		li.Add(b);
		li.Add(c);

	}

	public Line Intersection(Triangle other)
	{
		var me =  new Plane(this);
		var you = new Plane(other);


		var l = me.Intersect(you);

		return l;
	}
}

struct Plane
{
	public Vector3 position;
	public Vector3 normal;

	public Plane(Triangle t)
	{
		Math3d.PlaneFrom3Points(out normal, out position, t.a, t.b, t.c);
	}

	public Line Intersect(Plane other)
	{
		Line l = new();

		Math3d.PlanePlaneIntersection(out l.Point,out l.Vector, this.normal,this.position,other.normal,other.position);

		return l;

	}
}

struct Line
{
	public Vector3 Point;
	public Vector3 Vector;
	public float Len;

	public Line(Vector3 point, Vector3 vector, float len = 0)
	{
		this.Point = point;
		this.Vector = vector;
		this.Len = len;
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

		//var gb = m.GetVertexBuffer(0);

		List<Triangle> tris = new();

		for (int i = 0; i < verts.Length / 3; i++)
		{
			tris.Add(new Triangle(verts[i], verts[i + 1], verts[i + 2]));
		}

		List<int> ints = new List<int>();
		for (int i = 1; i < 100; i++)
		{
			tris.Add(new Triangle(
				new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)),
				new Vector3(-Random.Range(-10, 10), -Random.Range(-10, 10), Random.Range(-10, 10)), 
				new Vector3(Random.Range(-10, 10) , -Random.Range(-10, 10), -Random.Range(-10, 10))));
			ints.Add(i*3);
			ints.Add(i*3+1);
			ints.Add(i*3+2);
		}


		List<Vector3> vrl = new();
		foreach (var t in tris)
		{
			t.AddToList(vrl);
		}




		//verts[2] = new Vector3(1, 1, 1);
		//li.Add(new Vector3(2,2,10));
		//m.SetVertices(verts);

		m.SetVertices(vrl);
		m.SetTriangles(ints,0);

		m.Optimize();

		

		//var tri = m.GetTriangles(0);

		MeshCollider coll = GetComponent<MeshCollider>();
		coll.sharedMesh = m;


	}

	// Update is called once per frame
	void Update()
	{


	}
}
