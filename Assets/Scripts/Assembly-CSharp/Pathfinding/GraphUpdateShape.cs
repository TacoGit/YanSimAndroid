using UnityEngine;

namespace Pathfinding
{
	public class GraphUpdateShape
	{
		private Vector3[] _points;

		private Vector3[] _convexPoints;

		private bool _convex;

		private Vector3 right = Vector3.right;

		private Vector3 forward = Vector3.forward;

		private Vector3 up = Vector3.up;

		private Vector3 origin;

		public float minimumHeight;

		public Vector3[] points
		{
			get
			{
				return _points;
			}
			set
			{
				_points = value;
				if (convex)
				{
					CalculateConvexHull();
				}
			}
		}

		public bool convex
		{
			get
			{
				return _convex;
			}
			set
			{
				if (_convex != value && value)
				{
					CalculateConvexHull();
				}
				_convex = value;
			}
		}

		public GraphUpdateShape()
		{
		}

		public GraphUpdateShape(Vector3[] points, bool convex, Matrix4x4 matrix, float minimumHeight)
		{
			this.convex = convex;
			this.points = points;
			origin = matrix.MultiplyPoint3x4(Vector3.zero);
			right = matrix.MultiplyPoint3x4(Vector3.right) - origin;
			up = matrix.MultiplyPoint3x4(Vector3.up) - origin;
			forward = matrix.MultiplyPoint3x4(Vector3.forward) - origin;
			this.minimumHeight = minimumHeight;
		}

		private void CalculateConvexHull()
		{
			_convexPoints = ((points == null) ? null : Polygon.ConvexHullXZ(points));
		}

		public Bounds GetBounds()
		{
			return GetBounds((!convex) ? points : _convexPoints, right, up, forward, origin, minimumHeight);
		}

		public static Bounds GetBounds(Vector3[] points, Matrix4x4 matrix, float minimumHeight)
		{
			Vector3 vector = matrix.MultiplyPoint3x4(Vector3.zero);
			Vector3 vector2 = matrix.MultiplyPoint3x4(Vector3.right) - vector;
			Vector3 vector3 = matrix.MultiplyPoint3x4(Vector3.up) - vector;
			Vector3 vector4 = matrix.MultiplyPoint3x4(Vector3.forward) - vector;
			return GetBounds(points, vector2, vector3, vector4, vector, minimumHeight);
		}

		private static Bounds GetBounds(Vector3[] points, Vector3 right, Vector3 up, Vector3 forward, Vector3 origin, float minimumHeight)
		{
			if (points == null || points.Length == 0)
			{
				return default(Bounds);
			}
			float num = points[0].y;
			float num2 = points[0].y;
			for (int i = 0; i < points.Length; i++)
			{
				num = Mathf.Min(num, points[i].y);
				num2 = Mathf.Max(num2, points[i].y);
			}
			float num3 = Mathf.Max(minimumHeight - (num2 - num), 0f) * 0.5f;
			num -= num3;
			num2 += num3;
			Vector3 vector = right * points[0].x + up * points[0].y + forward * points[0].z;
			Vector3 vector2 = vector;
			for (int j = 0; j < points.Length; j++)
			{
				Vector3 vector3 = right * points[j].x + forward * points[j].z;
				Vector3 rhs = vector3 + up * num;
				Vector3 rhs2 = vector3 + up * num2;
				vector = Vector3.Min(vector, rhs);
				vector = Vector3.Min(vector, rhs2);
				vector2 = Vector3.Max(vector2, rhs);
				vector2 = Vector3.Max(vector2, rhs2);
			}
			return new Bounds((vector + vector2) * 0.5f + origin, vector2 - vector);
		}

		public bool Contains(GraphNode node)
		{
			return Contains((Vector3)node.position);
		}

		public bool Contains(Vector3 point)
		{
			point -= origin;
			Vector3 p = new Vector3(Vector3.Dot(point, right) / right.sqrMagnitude, 0f, Vector3.Dot(point, forward) / forward.sqrMagnitude);
			if (convex)
			{
				if (_convexPoints == null)
				{
					return false;
				}
				int i = 0;
				int num = _convexPoints.Length - 1;
				for (; i < _convexPoints.Length; i++)
				{
					if (VectorMath.RightOrColinearXZ(_convexPoints[i], _convexPoints[num], p))
					{
						return false;
					}
					num = i;
				}
				return true;
			}
			return _points != null && Polygon.ContainsPointXZ(_points, p);
		}
	}
}
