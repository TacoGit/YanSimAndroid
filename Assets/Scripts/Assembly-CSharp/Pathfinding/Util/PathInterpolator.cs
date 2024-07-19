using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	public class PathInterpolator
	{
		private List<Vector3> path;

		private float distanceToSegmentStart;

		private float currentDistance;

		private float currentSegmentLength = float.PositiveInfinity;

		private float totalDistance = float.PositiveInfinity;

		public virtual Vector3 position
		{
			get
			{
				float t = ((!(currentSegmentLength > 0.0001f)) ? 0f : ((currentDistance - distanceToSegmentStart) / currentSegmentLength));
				return Vector3.Lerp(path[segmentIndex], path[segmentIndex + 1], t);
			}
		}

		public Vector3 tangent
		{
			get
			{
				return path[segmentIndex + 1] - path[segmentIndex];
			}
		}

		public float remainingDistance
		{
			get
			{
				return totalDistance - distance;
			}
			set
			{
				distance = totalDistance - value;
			}
		}

		public float distance
		{
			get
			{
				return currentDistance;
			}
			set
			{
				currentDistance = value;
				while (currentDistance < distanceToSegmentStart && segmentIndex > 0)
				{
					PrevSegment();
				}
				while (currentDistance > distanceToSegmentStart + currentSegmentLength && segmentIndex < path.Count - 2)
				{
					NextSegment();
				}
			}
		}

		public int segmentIndex { get; private set; }

		public bool valid
		{
			get
			{
				return path != null;
			}
		}

		public void SetPath(List<Vector3> path)
		{
			this.path = path;
			currentDistance = 0f;
			segmentIndex = 0;
			distanceToSegmentStart = 0f;
			if (path == null)
			{
				totalDistance = float.PositiveInfinity;
				currentSegmentLength = float.PositiveInfinity;
				return;
			}
			if (path.Count < 2)
			{
				throw new ArgumentException("Path must have a length of at least 2");
			}
			currentSegmentLength = (path[1] - path[0]).magnitude;
			totalDistance = 0f;
			Vector3 vector = path[0];
			for (int i = 1; i < path.Count; i++)
			{
				Vector3 vector2 = path[i];
				totalDistance += (vector2 - vector).magnitude;
				vector = vector2;
			}
		}

		public void MoveToSegment(int index, float fractionAlongSegment)
		{
			if (path != null)
			{
				if (index < 0 || index >= path.Count - 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				while (segmentIndex > index)
				{
					PrevSegment();
				}
				while (segmentIndex < index)
				{
					NextSegment();
				}
				distance = distanceToSegmentStart + Mathf.Clamp01(fractionAlongSegment) * currentSegmentLength;
			}
		}

		public void MoveToClosestPoint(Vector3 point)
		{
			if (path == null)
			{
				return;
			}
			float num = float.PositiveInfinity;
			float fractionAlongSegment = 0f;
			int index = 0;
			for (int i = 0; i < path.Count - 1; i++)
			{
				float num2 = VectorMath.ClosestPointOnLineFactor(path[i], path[i + 1], point);
				Vector3 vector = Vector3.Lerp(path[i], path[i + 1], num2);
				float sqrMagnitude = (point - vector).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					fractionAlongSegment = num2;
					index = i;
				}
			}
			MoveToSegment(index, fractionAlongSegment);
		}

		public void MoveToLocallyClosestPoint(Vector3 point, bool allowForwards = true, bool allowBackwards = true)
		{
			if (path != null)
			{
				while (allowForwards && segmentIndex < path.Count - 2 && (path[segmentIndex + 1] - point).sqrMagnitude <= (path[segmentIndex] - point).sqrMagnitude)
				{
					NextSegment();
				}
				while (allowBackwards && segmentIndex > 0 && (path[segmentIndex - 1] - point).sqrMagnitude <= (path[segmentIndex] - point).sqrMagnitude)
				{
					PrevSegment();
				}
				float num = 0f;
				float num2 = 0f;
				float num3 = float.PositiveInfinity;
				float num4 = float.PositiveInfinity;
				if (segmentIndex > 0)
				{
					num = VectorMath.ClosestPointOnLineFactor(path[segmentIndex - 1], path[segmentIndex], point);
					num3 = (Vector3.Lerp(path[segmentIndex - 1], path[segmentIndex], num) - point).sqrMagnitude;
				}
				if (segmentIndex < path.Count - 1)
				{
					num2 = VectorMath.ClosestPointOnLineFactor(path[segmentIndex], path[segmentIndex + 1], point);
					num4 = (Vector3.Lerp(path[segmentIndex], path[segmentIndex + 1], num2) - point).sqrMagnitude;
				}
				if (num3 < num4)
				{
					MoveToSegment(segmentIndex - 1, num);
				}
				else
				{
					MoveToSegment(segmentIndex, num2);
				}
			}
		}

		public void MoveToCircleIntersection2D(Vector3 circleCenter3D, float radius, IMovementPlane transform)
		{
			if (path != null)
			{
				while (segmentIndex < path.Count - 2 && VectorMath.ClosestPointOnLineFactor(path[segmentIndex], path[segmentIndex + 1], circleCenter3D) > 1f)
				{
					NextSegment();
				}
				Vector2 vector = transform.ToPlane(circleCenter3D);
				while (segmentIndex < path.Count - 2 && (transform.ToPlane(path[segmentIndex + 1]) - vector).sqrMagnitude <= radius * radius)
				{
					NextSegment();
				}
				float fractionAlongSegment = VectorMath.LineCircleIntersectionFactor(vector, transform.ToPlane(path[segmentIndex]), transform.ToPlane(path[segmentIndex + 1]), radius);
				MoveToSegment(segmentIndex, fractionAlongSegment);
			}
		}

		protected virtual void PrevSegment()
		{
			segmentIndex--;
			currentSegmentLength = (path[segmentIndex + 1] - path[segmentIndex]).magnitude;
			distanceToSegmentStart -= currentSegmentLength;
		}

		protected virtual void NextSegment()
		{
			segmentIndex++;
			distanceToSegmentStart += currentSegmentLength;
			currentSegmentLength = (path[segmentIndex + 1] - path[segmentIndex]).magnitude;
		}
	}
}
