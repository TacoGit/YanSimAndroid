using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/GraphUpdateScene")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_graph_update_scene.php")]
	public class GraphUpdateScene : GraphModifier
	{
		public Vector3[] points;

		private Vector3[] convexPoints;

		public bool convex = true;

		public float minBoundsHeight = 1f;

		public int penaltyDelta;

		public bool modifyWalkability;

		public bool setWalkability;

		public bool applyOnStart = true;

		public bool applyOnScan = true;

		public bool updatePhysics;

		public bool resetPenaltyOnPhysics = true;

		public bool updateErosion = true;

		public bool modifyTag;

		public int setTag;

		[HideInInspector]
		public bool legacyMode;

		private int setTagInvert;

		private bool firstApplied;

		[SerializeField]
		private int serializedVersion;

		[SerializeField]
		[FormerlySerializedAs("useWorldSpace")]
		private bool legacyUseWorldSpace;

		public void Start()
		{
			if (Application.isPlaying && !firstApplied && applyOnStart)
			{
				Apply();
			}
		}

		public override void OnPostScan()
		{
			if (applyOnScan)
			{
				Apply();
			}
		}

		public virtual void InvertSettings()
		{
			setWalkability = !setWalkability;
			penaltyDelta = -penaltyDelta;
			if (setTagInvert == 0)
			{
				setTagInvert = setTag;
				setTag = 0;
			}
			else
			{
				setTag = setTagInvert;
				setTagInvert = 0;
			}
		}

		public void RecalcConvex()
		{
			convexPoints = ((!convex) ? null : Polygon.ConvexHullXZ(points));
		}

		[Obsolete("World space can no longer be used as it does not work well with rotated graphs. Use transform.InverseTransformPoint to transform points to local space.", true)]
		private void ToggleUseWorldSpace()
		{
		}

		[Obsolete("The Y coordinate is no longer important. Use the position of the object instead", true)]
		public void LockToY()
		{
		}

		public Bounds GetBounds()
		{
			if (points == null || points.Length == 0)
			{
				Collider component = GetComponent<Collider>();
				Collider2D component2 = GetComponent<Collider2D>();
				Renderer component3 = GetComponent<Renderer>();
				Bounds bounds;
				if (component != null)
				{
					bounds = component.bounds;
				}
				else if (component2 != null)
				{
					bounds = component2.bounds;
					bounds.size = new Vector3(bounds.size.x, bounds.size.y, Mathf.Max(bounds.size.z, 1f));
				}
				else
				{
					if (!(component3 != null))
					{
						return new Bounds(Vector3.zero, Vector3.zero);
					}
					bounds = component3.bounds;
				}
				if (legacyMode && bounds.size.y < minBoundsHeight)
				{
					bounds.size = new Vector3(bounds.size.x, minBoundsHeight, bounds.size.z);
				}
				return bounds;
			}
			return GraphUpdateShape.GetBounds((!convex) ? points : convexPoints, (!legacyMode || !legacyUseWorldSpace) ? base.transform.localToWorldMatrix : Matrix4x4.identity, minBoundsHeight);
		}

		public void Apply()
		{
			if (AstarPath.active == null)
			{
				Debug.LogError("There is no AstarPath object in the scene", this);
				return;
			}
			GraphUpdateObject graphUpdateObject;
			if (points == null || points.Length == 0)
			{
				PolygonCollider2D component = GetComponent<PolygonCollider2D>();
				if (component != null)
				{
					Vector2[] array = component.points;
					Vector3[] array2 = new Vector3[array.Length];
					for (int i = 0; i < array2.Length; i++)
					{
						Vector2 vector = array[i] + component.offset;
						array2[i] = new Vector3(vector.x, 0f, vector.y);
					}
					Matrix4x4 matrix = base.transform.localToWorldMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90f, 0f, 0f), Vector3.one);
					GraphUpdateShape shape = new GraphUpdateShape(points, convex, matrix, minBoundsHeight);
					graphUpdateObject = new GraphUpdateObject(GetBounds());
					graphUpdateObject.shape = shape;
				}
				else
				{
					Bounds bounds = GetBounds();
					if (bounds.center == Vector3.zero && bounds.size == Vector3.zero)
					{
						Debug.LogError("Cannot apply GraphUpdateScene, no points defined and no renderer or collider attached", this);
						return;
					}
					graphUpdateObject = new GraphUpdateObject(bounds);
				}
			}
			else
			{
				GraphUpdateShape graphUpdateShape;
				if (legacyMode && !legacyUseWorldSpace)
				{
					Vector3[] array3 = new Vector3[points.Length];
					for (int j = 0; j < points.Length; j++)
					{
						array3[j] = base.transform.TransformPoint(points[j]);
					}
					graphUpdateShape = new GraphUpdateShape(array3, convex, Matrix4x4.identity, minBoundsHeight);
				}
				else
				{
					graphUpdateShape = new GraphUpdateShape(points, convex, (!legacyMode || !legacyUseWorldSpace) ? base.transform.localToWorldMatrix : Matrix4x4.identity, minBoundsHeight);
				}
				Bounds bounds2 = graphUpdateShape.GetBounds();
				graphUpdateObject = new GraphUpdateObject(bounds2);
				graphUpdateObject.shape = graphUpdateShape;
			}
			firstApplied = true;
			graphUpdateObject.modifyWalkability = modifyWalkability;
			graphUpdateObject.setWalkability = setWalkability;
			graphUpdateObject.addPenalty = penaltyDelta;
			graphUpdateObject.updatePhysics = updatePhysics;
			graphUpdateObject.updateErosion = updateErosion;
			graphUpdateObject.resetPenaltyOnPhysics = resetPenaltyOnPhysics;
			graphUpdateObject.modifyTag = modifyTag;
			graphUpdateObject.setTag = setTag;
			AstarPath.active.UpdateGraphs(graphUpdateObject);
		}

		private void OnDrawGizmos()
		{
			OnDrawGizmos(false);
		}

		private void OnDrawGizmosSelected()
		{
			OnDrawGizmos(true);
		}

		private void OnDrawGizmos(bool selected)
		{
			Color color = ((!selected) ? new Color(0.8901961f, 0.23921569f, 0.08627451f, 0.9f) : new Color(0.8901961f, 0.23921569f, 0.08627451f, 1f));
			if (selected)
			{
				Gizmos.color = Color.Lerp(color, new Color(1f, 1f, 1f, 0.2f), 0.9f);
				Bounds bounds = GetBounds();
				Gizmos.DrawCube(bounds.center, bounds.size);
				Gizmos.DrawWireCube(bounds.center, bounds.size);
			}
			if (points == null)
			{
				return;
			}
			if (convex)
			{
				color.a *= 0.5f;
			}
			Gizmos.color = color;
			Matrix4x4 matrix4x = ((!legacyMode || !legacyUseWorldSpace) ? base.transform.localToWorldMatrix : Matrix4x4.identity);
			if (convex)
			{
				color.r -= 0.1f;
				color.g -= 0.2f;
				color.b -= 0.1f;
				Gizmos.color = color;
			}
			if (selected || !convex)
			{
				for (int i = 0; i < points.Length; i++)
				{
					Gizmos.DrawLine(matrix4x.MultiplyPoint3x4(points[i]), matrix4x.MultiplyPoint3x4(points[(i + 1) % points.Length]));
				}
			}
			if (convex)
			{
				if (convexPoints == null)
				{
					RecalcConvex();
				}
				Gizmos.color = ((!selected) ? new Color(0.8901961f, 0.23921569f, 0.08627451f, 0.9f) : new Color(0.8901961f, 0.23921569f, 0.08627451f, 1f));
				for (int j = 0; j < convexPoints.Length; j++)
				{
					Gizmos.DrawLine(matrix4x.MultiplyPoint3x4(convexPoints[j]), matrix4x.MultiplyPoint3x4(convexPoints[(j + 1) % convexPoints.Length]));
				}
			}
			Vector3[] array = ((!convex) ? points : convexPoints);
			if (selected && array != null && array.Length > 0)
			{
				Gizmos.color = new Color(1f, 1f, 1f, 0.2f);
				float num = array[0].y;
				float num2 = array[0].y;
				for (int k = 0; k < array.Length; k++)
				{
					num = Mathf.Min(num, array[k].y);
					num2 = Mathf.Max(num2, array[k].y);
				}
				float num3 = Mathf.Max(minBoundsHeight - (num2 - num), 0f) * 0.5f;
				num -= num3;
				num2 += num3;
				for (int l = 0; l < array.Length; l++)
				{
					int num4 = (l + 1) % array.Length;
					Vector3 from = matrix4x.MultiplyPoint3x4(array[l] + Vector3.up * (num - array[l].y));
					Vector3 vector = matrix4x.MultiplyPoint3x4(array[l] + Vector3.up * (num2 - array[l].y));
					Vector3 to = matrix4x.MultiplyPoint3x4(array[num4] + Vector3.up * (num - array[num4].y));
					Vector3 to2 = matrix4x.MultiplyPoint3x4(array[num4] + Vector3.up * (num2 - array[num4].y));
					Gizmos.DrawLine(from, vector);
					Gizmos.DrawLine(from, to);
					Gizmos.DrawLine(vector, to2);
				}
			}
		}

		public void DisableLegacyMode()
		{
			if (!legacyMode)
			{
				return;
			}
			legacyMode = false;
			if (legacyUseWorldSpace)
			{
				legacyUseWorldSpace = false;
				for (int i = 0; i < points.Length; i++)
				{
					points[i] = base.transform.InverseTransformPoint(points[i]);
				}
				RecalcConvex();
			}
		}

		protected override void Awake()
		{
			if (serializedVersion == 0)
			{
				if (points != null && points.Length > 0)
				{
					legacyMode = true;
				}
				serializedVersion = 1;
			}
			base.Awake();
		}
	}
}
