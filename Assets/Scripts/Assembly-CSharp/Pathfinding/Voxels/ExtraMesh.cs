using System;
using UnityEngine;

namespace Pathfinding.Voxels
{
	[Obsolete("Use RasterizationMesh instead")]
	public class ExtraMesh : RasterizationMesh
	{
		public ExtraMesh(Vector3[] vertices, int[] triangles, Bounds bounds)
			: base(vertices, triangles, bounds)
		{
		}

		public ExtraMesh(Vector3[] vertices, int[] triangles, Bounds bounds, Matrix4x4 matrix)
			: base(vertices, triangles, bounds, matrix)
		{
		}
	}
}
