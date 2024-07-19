using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/Navmesh/Navmesh Cut")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_navmesh_cut.php")]
	public class NavmeshCut : NavmeshClipper
	{
		public enum MeshType
		{
			Rectangle = 0,
			Circle = 1,
			CustomMesh = 2
		}

		[Tooltip("Shape of the cut")]
		public MeshType type;

		[Tooltip("The contour(s) of the mesh will be extracted. This mesh should only be a 2D surface, not a volume (see documentation).")]
		public Mesh mesh;

		public Vector2 rectangleSize = new Vector2(1f, 1f);

		public float circleRadius = 1f;

		public int circleResolution = 6;

		public float height = 1f;

		[Tooltip("Scale of the custom mesh")]
		public float meshScale = 1f;

		public Vector3 center;

		[Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
		public float updateDistance = 0.4f;

		[Tooltip("Only makes a split in the navmesh, but does not remove the geometry to make a hole")]
		public bool isDual;

		public bool cutsAddedGeom = true;

		[Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
		public float updateRotationDistance = 10f;

		[Tooltip("Includes rotation in calculations. This is slower since a lot more matrix multiplications are needed but gives more flexibility.")]
		[FormerlySerializedAs("useRotation")]
		public bool useRotationAndScale;

		private Vector3[][] contours;

		protected Transform tr;

		private Mesh lastMesh;

		private Vector3 lastPosition;

		private Quaternion lastRotation;

		private static readonly Dictionary<Int2, int> edges = new Dictionary<Int2, int>();

		private static readonly Dictionary<int, int> pointers = new Dictionary<int, int>();

		public static readonly Color GizmoColor = new Color(0.14509805f, 0.72156864f, 0.9372549f);

		protected override void Awake()
		{
			base.Awake();
			tr = base.transform;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
			lastRotation = tr.rotation;
		}

		public override void ForceUpdate()
		{
			lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		public override bool RequiresUpdate()
		{
			return (tr.position - lastPosition).sqrMagnitude > updateDistance * updateDistance || (useRotationAndScale && Quaternion.Angle(lastRotation, tr.rotation) > updateRotationDistance);
		}

		public virtual void UsedForCut()
		{
		}

		internal override void NotifyUpdated()
		{
			lastPosition = tr.position;
			if (useRotationAndScale)
			{
				lastRotation = tr.rotation;
			}
		}

		private void CalculateMeshContour()
		{
			if (mesh == null)
			{
				return;
			}
			edges.Clear();
			pointers.Clear();
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (VectorMath.IsClockwiseXZ(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]))
				{
					int num = triangles[i];
					triangles[i] = triangles[i + 2];
					triangles[i + 2] = num;
				}
				edges[new Int2(triangles[i], triangles[i + 1])] = i;
				edges[new Int2(triangles[i + 1], triangles[i + 2])] = i;
				edges[new Int2(triangles[i + 2], triangles[i])] = i;
			}
			for (int j = 0; j < triangles.Length; j += 3)
			{
				for (int k = 0; k < 3; k++)
				{
					if (!edges.ContainsKey(new Int2(triangles[j + (k + 1) % 3], triangles[j + k % 3])))
					{
						pointers[triangles[j + k % 3]] = triangles[j + (k + 1) % 3];
					}
				}
			}
			List<Vector3[]> list = new List<Vector3[]>();
			List<Vector3> list2 = ListPool<Vector3>.Claim();
			for (int l = 0; l < vertices.Length; l++)
			{
				if (!pointers.ContainsKey(l))
				{
					continue;
				}
				list2.Clear();
				int num2 = l;
				do
				{
					int num3 = pointers[num2];
					if (num3 == -1)
					{
						break;
					}
					pointers[num2] = -1;
					list2.Add(vertices[num2]);
					num2 = num3;
					if (num2 == -1)
					{
						Debug.LogError("Invalid Mesh '" + mesh.name + " in " + base.gameObject.name);
						break;
					}
				}
				while (num2 != l);
				if (list2.Count > 0)
				{
					list.Add(list2.ToArray());
				}
			}
			ListPool<Vector3>.Release(ref list2);
			contours = list.ToArray();
		}

		internal override Rect GetBounds(GraphTransform inverseTranform)
		{
			List<List<Vector3>> list = ListPool<List<Vector3>>.Claim();
			GetContour(list);
			Rect result = default(Rect);
			for (int i = 0; i < list.Count; i++)
			{
				List<Vector3> list2 = list[i];
				for (int j = 0; j < list2.Count; j++)
				{
					Vector3 vector = inverseTranform.InverseTransform(list2[j]);
					if (j == 0)
					{
						result = new Rect(vector.x, vector.z, 0f, 0f);
						continue;
					}
					result.xMax = Math.Max(result.xMax, vector.x);
					result.yMax = Math.Max(result.yMax, vector.z);
					result.xMin = Math.Min(result.xMin, vector.x);
					result.yMin = Math.Min(result.yMin, vector.z);
				}
			}
			ListPool<List<Vector3>>.Release(ref list);
			return result;
		}

		public void GetContour(List<List<Vector3>> buffer)
		{
			if (circleResolution < 3)
			{
				circleResolution = 3;
			}
			switch (type)
			{
			case MeshType.Rectangle:
			{
				List<Vector3> list = ListPool<Vector3>.Claim();
				list.Add(new Vector3(0f - rectangleSize.x, 0f, 0f - rectangleSize.y) * 0.5f);
				list.Add(new Vector3(rectangleSize.x, 0f, 0f - rectangleSize.y) * 0.5f);
				list.Add(new Vector3(rectangleSize.x, 0f, rectangleSize.y) * 0.5f);
				list.Add(new Vector3(0f - rectangleSize.x, 0f, rectangleSize.y) * 0.5f);
				bool reverse = (rectangleSize.x < 0f) ^ (rectangleSize.y < 0f);
				TransformBuffer(list, reverse);
				buffer.Add(list);
				break;
			}
			case MeshType.Circle:
			{
				List<Vector3> list = ListPool<Vector3>.Claim(circleResolution);
				for (int k = 0; k < circleResolution; k++)
				{
					list.Add(new Vector3(Mathf.Cos((float)(k * 2) * (float)Math.PI / (float)circleResolution), 0f, Mathf.Sin((float)(k * 2) * (float)Math.PI / (float)circleResolution)) * circleRadius);
				}
				bool reverse = circleRadius < 0f;
				TransformBuffer(list, reverse);
				buffer.Add(list);
				break;
			}
			case MeshType.CustomMesh:
			{
				if (mesh != lastMesh || contours == null)
				{
					CalculateMeshContour();
					lastMesh = mesh;
				}
				if (contours == null)
				{
					break;
				}
				bool reverse = meshScale < 0f;
				for (int i = 0; i < contours.Length; i++)
				{
					Vector3[] array = contours[i];
					List<Vector3> list = ListPool<Vector3>.Claim(array.Length);
					for (int j = 0; j < array.Length; j++)
					{
						list.Add(array[j] * meshScale);
					}
					TransformBuffer(list, reverse);
					buffer.Add(list);
				}
				break;
			}
			}
		}

		private void TransformBuffer(List<Vector3> buffer, bool reverse)
		{
			Vector3 vector = center;
			if (useRotationAndScale)
			{
				Matrix4x4 localToWorldMatrix = tr.localToWorldMatrix;
				for (int i = 0; i < buffer.Count; i++)
				{
					buffer[i] = localToWorldMatrix.MultiplyPoint3x4(buffer[i] + vector);
				}
				reverse ^= VectorMath.ReversesFaceOrientationsXZ(localToWorldMatrix);
			}
			else
			{
				vector += tr.position;
				for (int j = 0; j < buffer.Count; j++)
				{
					buffer[j] += vector;
				}
			}
			if (reverse)
			{
				buffer.Reverse();
			}
		}

		public void OnDrawGizmos()
		{
			if (tr == null)
			{
				tr = base.transform;
			}
			List<List<Vector3>> list = ListPool<List<Vector3>>.Claim();
			GetContour(list);
			Gizmos.color = GizmoColor;
			for (int i = 0; i < list.Count; i++)
			{
				List<Vector3> list2 = list[i];
				for (int j = 0; j < list2.Count; j++)
				{
					Vector3 from = list2[j];
					Vector3 to = list2[(j + 1) % list2.Count];
					Gizmos.DrawLine(from, to);
				}
			}
			ListPool<List<Vector3>>.Release(ref list);
		}

		internal float GetY(GraphTransform transform)
		{
			return transform.InverseTransform((!useRotationAndScale) ? (tr.position + center) : tr.TransformPoint(center)).y;
		}

		public void OnDrawGizmosSelected()
		{
			List<List<Vector3>> list = ListPool<List<Vector3>>.Claim();
			GetContour(list);
			Color color = Color.Lerp(GizmoColor, Color.white, 0.5f);
			color.a *= 0.5f;
			Gizmos.color = color;
			NavmeshBase navmeshBase = (NavmeshBase)((!(AstarPath.active != null)) ? null : (((object)AstarPath.active.data.recastGraph) ?? ((object)AstarPath.active.data.navmesh)));
			GraphTransform graphTransform = ((navmeshBase == null) ? GraphTransform.identityTransform : navmeshBase.transform);
			float y = GetY(graphTransform);
			float y2 = y - height * 0.5f;
			float y3 = y + height * 0.5f;
			for (int i = 0; i < list.Count; i++)
			{
				List<Vector3> list2 = list[i];
				for (int j = 0; j < list2.Count; j++)
				{
					Vector3 vector = graphTransform.InverseTransform(list2[j]);
					Vector3 vector2 = graphTransform.InverseTransform(list2[(j + 1) % list2.Count]);
					Vector3 point = vector;
					Vector3 point2 = vector2;
					Vector3 point3 = vector;
					Vector3 point4 = vector2;
					point.y = (point2.y = y2);
					point3.y = (point4.y = y3);
					Gizmos.DrawLine(graphTransform.Transform(point), graphTransform.Transform(point2));
					Gizmos.DrawLine(graphTransform.Transform(point3), graphTransform.Transform(point4));
					Gizmos.DrawLine(graphTransform.Transform(point), graphTransform.Transform(point3));
				}
			}
			ListPool<List<Vector3>>.Release(ref list);
		}
	}
}
