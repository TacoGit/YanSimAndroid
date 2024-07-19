using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Voxels
{
	public class RasterizationMesh
	{
		public MeshFilter original;

		public int area;

		public Vector3[] vertices;

		public int[] triangles;

		public int numVertices;

		public int numTriangles;

		public Bounds bounds;

		public Matrix4x4 matrix;

		public bool pool;

		public RasterizationMesh()
		{
		}

		public RasterizationMesh(Vector3[] vertices, int[] triangles, Bounds bounds)
		{
			matrix = Matrix4x4.identity;
			this.vertices = vertices;
			numVertices = vertices.Length;
			this.triangles = triangles;
			numTriangles = triangles.Length;
			this.bounds = bounds;
			original = null;
			area = 0;
		}

		public RasterizationMesh(Vector3[] vertices, int[] triangles, Bounds bounds, Matrix4x4 matrix)
		{
			this.matrix = matrix;
			this.vertices = vertices;
			numVertices = vertices.Length;
			this.triangles = triangles;
			numTriangles = triangles.Length;
			this.bounds = bounds;
			original = null;
			area = 0;
		}

		public void RecalculateBounds()
		{
			Bounds bounds = new Bounds(matrix.MultiplyPoint3x4(vertices[0]), Vector3.zero);
			for (int i = 1; i < numVertices; i++)
			{
				bounds.Encapsulate(matrix.MultiplyPoint3x4(vertices[i]));
			}
			this.bounds = bounds;
		}

		public void Pool()
		{
			if (pool)
			{
				ArrayPool<int>.Release(ref triangles);
				ArrayPool<Vector3>.Release(ref vertices);
			}
		}
	}
}
