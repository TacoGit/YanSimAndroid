using System;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_navmesh_add.php")]
	public class NavmeshAdd : NavmeshClipper
	{
		public enum MeshType
		{
			Rectangle = 0,
			CustomMesh = 1
		}

		public MeshType type;

		public Mesh mesh;

		private Vector3[] verts;

		private int[] tris;

		public Vector2 rectangleSize = new Vector2(1f, 1f);

		public float meshScale = 1f;

		public Vector3 center;

		[FormerlySerializedAs("useRotation")]
		public bool useRotationAndScale;

		[Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
		public float updateDistance = 0.4f;

		[Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
		public float updateRotationDistance = 10f;

		protected Transform tr;

		private Vector3 lastPosition;

		private Quaternion lastRotation;

		public static readonly Color GizmoColor = new Color(0.36862746f, 0.9372549f, 0.14509805f);

		public Vector3 Center
		{
			get
			{
				return tr.position + ((!useRotationAndScale) ? center : tr.TransformPoint(center));
			}
		}

		public override bool RequiresUpdate()
		{
			return (tr.position - lastPosition).sqrMagnitude > updateDistance * updateDistance || (useRotationAndScale && Quaternion.Angle(lastRotation, tr.rotation) > updateRotationDistance);
		}

		public override void ForceUpdate()
		{
			lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		protected override void Awake()
		{
			base.Awake();
			tr = base.transform;
		}

		internal override void NotifyUpdated()
		{
			lastPosition = tr.position;
			if (useRotationAndScale)
			{
				lastRotation = tr.rotation;
			}
		}

		[ContextMenu("Rebuild Mesh")]
		public void RebuildMesh()
		{
			if (type == MeshType.CustomMesh)
			{
				if (mesh == null)
				{
					verts = null;
					tris = null;
				}
				else
				{
					verts = mesh.vertices;
					tris = mesh.triangles;
				}
				return;
			}
			if (verts == null || verts.Length != 4 || tris == null || tris.Length != 6)
			{
				verts = new Vector3[4];
				tris = new int[6];
			}
			tris[0] = 0;
			tris[1] = 1;
			tris[2] = 2;
			tris[3] = 0;
			tris[4] = 2;
			tris[5] = 3;
			verts[0] = new Vector3((0f - rectangleSize.x) * 0.5f, 0f, (0f - rectangleSize.y) * 0.5f);
			verts[1] = new Vector3(rectangleSize.x * 0.5f, 0f, (0f - rectangleSize.y) * 0.5f);
			verts[2] = new Vector3(rectangleSize.x * 0.5f, 0f, rectangleSize.y * 0.5f);
			verts[3] = new Vector3((0f - rectangleSize.x) * 0.5f, 0f, rectangleSize.y * 0.5f);
		}

		internal override Rect GetBounds(GraphTransform inverseTransform)
		{
			if (verts == null)
			{
				RebuildMesh();
			}
			Int3[] vbuffer = ArrayPool<Int3>.Claim((verts != null) ? verts.Length : 0);
			int[] tbuffer;
			GetMesh(ref vbuffer, out tbuffer, inverseTransform);
			Rect result = default(Rect);
			for (int i = 0; i < tbuffer.Length; i++)
			{
				Vector3 vector = (Vector3)vbuffer[tbuffer[i]];
				if (i == 0)
				{
					result = new Rect(vector.x, vector.z, 0f, 0f);
					continue;
				}
				result.xMax = Math.Max(result.xMax, vector.x);
				result.yMax = Math.Max(result.yMax, vector.z);
				result.xMin = Math.Min(result.xMin, vector.x);
				result.yMin = Math.Min(result.yMin, vector.z);
			}
			ArrayPool<Int3>.Release(ref vbuffer);
			return result;
		}

		public void GetMesh(ref Int3[] vbuffer, out int[] tbuffer, GraphTransform inverseTransform = null)
		{
			if (verts == null)
			{
				RebuildMesh();
			}
			if (verts == null)
			{
				tbuffer = ArrayPool<int>.Claim(0);
				return;
			}
			if (vbuffer == null || vbuffer.Length < verts.Length)
			{
				if (vbuffer != null)
				{
					ArrayPool<Int3>.Release(ref vbuffer);
				}
				vbuffer = ArrayPool<Int3>.Claim(verts.Length);
			}
			tbuffer = tris;
			if (useRotationAndScale)
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(tr.position + center, tr.rotation, tr.localScale * meshScale);
				for (int i = 0; i < verts.Length; i++)
				{
					Vector3 vector = matrix4x.MultiplyPoint3x4(verts[i]);
					if (inverseTransform != null)
					{
						vector = inverseTransform.InverseTransform(vector);
					}
					vbuffer[i] = (Int3)vector;
				}
				return;
			}
			Vector3 vector2 = tr.position + center;
			for (int j = 0; j < verts.Length; j++)
			{
				Vector3 vector3 = vector2 + verts[j] * meshScale;
				if (inverseTransform != null)
				{
					vector3 = inverseTransform.InverseTransform(vector3);
				}
				vbuffer[j] = (Int3)vector3;
			}
		}
	}
}
