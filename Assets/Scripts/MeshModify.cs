using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Parabox.CSG;

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

	public bool isIntersecting(Triangle other)
	{
		//const float closenessConstant = 0.1e-5f;

		var me =  new Plane(this);
		var you = new Plane(other);

		var l = me.Intersect(you);

		int numIntersections = 0;
		Vector3[] p = new Vector3[2];

		for (int i = 0; i < 3; i++)
		{
			//Math3d.ClosestPointsOnTwoLines(out Vector3 p1, out Vector3 p2, verts[i], verts[i] - verts[(i + 1) % 3], l.Point, l.Vector);

			if (!Math3d.LineLineIntersection(out Vector3 p1, verts[i], verts[i] - verts[(i + 1) % 3], l.Point, l.Vector))
				continue;
			
			//if ((p1 - verts[i]).magnitude < closenessConstant) 
			//	return true;

			if (
				Math3d.PointOnWhichSideOfLineSegment(this.verts[i], this.verts[(i + 1) % 3], p1) == 0
				
				//)if ((p1 - p2).magnitude < float.Epsilon
				//)if((p1 - verts[i]).magnitude > 0.1e-20f
				//)if((p1 - verts[(i + 1) % 3]).magnitude > 0.1e-20f
				)
			{
				p[numIntersections] = p1;
				numIntersections++;
			}
		}
		if (numIntersections < 2)
			return false;

		for (int i = 0; i < 3; i++)
		{
			if (!Math3d.LineLineIntersection(out Vector3 p1, other.verts[i], other.verts[i] - other.verts[(i + 1) % 3], l.Point, l.Vector))
				continue;

			//if ((p1 - p2).magnitude < closenessConstant
			//	 )
			if(
				Math3d.PointOnWhichSideOfLineSegment(other.verts[i], other.verts[(i + 1) % 3], Math3d.ProjectPointOnLineSegment(other.verts[i], other.verts[(i + 1) % 3], p1)) == 0
				)
			{
				
				if(Math3d.PointOnWhichSideOfLineSegment(p[0], p[1], Math3d.ProjectPointOnLineSegment(p[0], p[1], p1)) == 0)
					return true;
			}
		}
		return false;
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

	public Line(Vector3 point, Vector3 vector)
	{
		this.Point = point;
		this.Vector = vector;
	}
}

struct LineSegment
{
	public Vector3 Start;
	public Vector3 End;

	public LineSegment(Vector3 start, Vector3 end)
	{
		Start = start;
		End = end;
	}
}


public class MeshModify : MonoBehaviour
{
	public Mesh mesh;
	public GameObject remover;

	void Test()
	{
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.localScale = Vector3.one * 1.3f;

		// Perform boolean operation
		Model result = CSG.Subtract(cube, sphere);

		// Create a gameObject to render the result
		var composite = new GameObject();
		composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
		composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();

		

		Model m = CSG.Subtract(gameObject,remover);
		var a = GetComponent<MeshFilter>();
		a.sharedMesh = m.mesh;
		GetComponent<MeshRenderer>().sharedMaterials = m.materials.ToArray();
	}

	// Start is called before the first frame update
	public void Subtract(GameObject subtracted)
	{
		var startTime = System.DateTime.Now;

		var a = GetComponent<MeshFilter>();
		var m = a.mesh;
		var x = m.GetVertexBuffer(0);
		x.Release();


		//generate triangels from meshes

		Vector3[] verts = m.vertices;
		List<Triangle> myTris = new();

		for (int i = 0; i < m.triangles.Length / 3; i++)
		{
			myTris.Add(new Triangle(
						a.transform.TransformPoint(verts[m.triangles[i * 3]]),
						a.transform.TransformPoint(verts[m.triangles[i * 3 + 1]]),
						a.transform.TransformPoint(verts[m.triangles[i * 3 + 2]])));
		}

		List<Triangle> otherTris = new();

		for (int i = 0; i < mesh.triangles.Length / 3; i++)
		{
			otherTris.Add(
				new Triangle(
				mesh.vertices[mesh.triangles[i * 3]], 
				mesh.vertices[mesh.triangles[i * 3 + 1]], 
				mesh.vertices[mesh.triangles[i * 3 + 2]]));
		}

		
		//remove intersecting triangles from meshes
		List<int> ind = new();
		List<int> jnd = new();

		for (int i = myTris.Count - 1; i >= 0; i--)
		{
			for (int j = otherTris.Count - 1; j >= 0; j--)
			{
				if (myTris[i].isIntersecting(otherTris[j]))
				{
					ind.Add(i);
					jnd.Add(j);
				}
			}
		}
		
		ind = ind.Distinct().ToList();
		jnd = jnd.Distinct().ToList();

		ind.Sort();
		jnd.Sort();


		for (int i = ind.Count() - 1; i >= 0; i--)
			myTris.RemoveAt(ind[i]);

		for (int j = jnd.Count() - 1; j >= 0; j--)
			otherTris.RemoveAt(jnd[j]);

		// TODO remove other half

		// TODO generate joining triangles


		// assemble triangles to single mesh

		
		List<Vector3> vrl = new();
		foreach (var t in myTris)
		{
			t.AddToList(vrl);
		}
		
		foreach (var t in otherTris)
		{
			t.AddToList(vrl);
		}

		for (int i = 0; i < vrl.Count; i++)
		{
			vrl[i] = a.transform.InverseTransformPoint(vrl[i]);
		}
		

		int[] indexes = new int[vrl.Count];
		for (int i = 0; i < vrl.Count; i++)
		{
			indexes[i] = i;
		}

		m.SetVertices(vrl);
		m.SetTriangles(indexes, 0);

		m.RecalculateNormals();
		m.RecalculateTangents();
		m.RecalculateBounds();
		m.MarkModified();
		m.Optimize();
		m.name = "custom";

		a.mesh = m;

		var stopTime = System.DateTime.Now;

		var timeTaken = stopTime - startTime;

		Debug.Log( timeTaken.Milliseconds + "ms, " + (timeTaken.TotalSeconds * 60) + " frames");


		/*
		List<int> ints = new List<int>();
		for (int i = 1; i < 100; i++)
		{
			myTris.Add(new Triangle(
				new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)),
				new Vector3(-Random.Range(-10, 10), -Random.Range(-10, 10), Random.Range(-10, 10)), 
				new Vector3(Random.Range(-10, 10) , -Random.Range(-10, 10), -Random.Range(-10, 10))));
			ints.Add(i*3);
			ints.Add(i*3+1);
			ints.Add(i*3+2);
		}


		List<Vector3> vrl = new();
		foreach (var t in myTris)
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
		*/

	}

	// Update is called once per frame
	void Update()
	{


	}
}
