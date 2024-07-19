using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
	public abstract class RVOObstacle : VersionedMonoBehaviour
	{
		public enum ObstacleVertexWinding
		{
			KeepOut = 0,
			KeepIn = 1
		}

		public ObstacleVertexWinding obstacleMode;

		public RVOLayer layer = RVOLayer.DefaultObstacle;

		protected Simulator sim;

		private List<ObstacleVertex> addedObstacles;

		private List<Vector3[]> sourceObstacles;

		private bool gizmoDrawing;

		private List<Vector3[]> gizmoVerts;

		private ObstacleVertexWinding _obstacleMode;

		private Matrix4x4 prevUpdateMatrix;

		protected abstract bool ExecuteInEditor { get; }

		protected abstract bool LocalCoordinates { get; }

		protected abstract bool StaticObstacle { get; }

		protected abstract float Height { get; }

		protected abstract void CreateObstacles();

		protected abstract bool AreGizmosDirty();

		public void OnDrawGizmos()
		{
			OnDrawGizmos(false);
		}

		public void OnDrawGizmosSelected()
		{
			OnDrawGizmos(true);
		}

		public void OnDrawGizmos(bool selected)
		{
			gizmoDrawing = true;
			Gizmos.color = new Color(0.615f, 1f, 0.06f, (!selected) ? 0.7f : 1f);
			MovementPlane movementPlane = ((RVOSimulator.active != null) ? RVOSimulator.active.movementPlane : MovementPlane.XZ);
			Vector3 vector = ((movementPlane != 0) ? (-Vector3.forward) : Vector3.up);
			if (gizmoVerts == null || AreGizmosDirty() || _obstacleMode != obstacleMode)
			{
				_obstacleMode = obstacleMode;
				if (gizmoVerts == null)
				{
					gizmoVerts = new List<Vector3[]>();
				}
				else
				{
					gizmoVerts.Clear();
				}
				CreateObstacles();
			}
			Matrix4x4 matrix = GetMatrix();
			for (int i = 0; i < gizmoVerts.Count; i++)
			{
				Vector3[] array = gizmoVerts[i];
				int num = 0;
				int num2 = array.Length - 1;
				while (num < array.Length)
				{
					Gizmos.DrawLine(matrix.MultiplyPoint3x4(array[num]), matrix.MultiplyPoint3x4(array[num2]));
					num2 = num++;
				}
				if (!selected)
				{
					continue;
				}
				int num3 = 0;
				int num4 = array.Length - 1;
				while (num3 < array.Length)
				{
					Vector3 vector2 = matrix.MultiplyPoint3x4(array[num4]);
					Vector3 vector3 = matrix.MultiplyPoint3x4(array[num3]);
					if (movementPlane != MovementPlane.XY)
					{
						Gizmos.DrawLine(vector2 + vector * Height, vector3 + vector * Height);
						Gizmos.DrawLine(vector2, vector2 + vector * Height);
					}
					Vector3 vector4 = (vector2 + vector3) * 0.5f;
					Vector3 normalized = (vector3 - vector2).normalized;
					if (!(normalized == Vector3.zero))
					{
						Vector3 vector5 = Vector3.Cross(vector, normalized);
						Gizmos.DrawLine(vector4, vector4 + vector5);
						Gizmos.DrawLine(vector4 + vector5, vector4 + vector5 * 0.5f + normalized * 0.5f);
						Gizmos.DrawLine(vector4 + vector5, vector4 + vector5 * 0.5f - normalized * 0.5f);
					}
					num4 = num3++;
				}
			}
			gizmoDrawing = false;
		}

		protected virtual Matrix4x4 GetMatrix()
		{
			return (!LocalCoordinates) ? Matrix4x4.identity : base.transform.localToWorldMatrix;
		}

		public void OnDisable()
		{
			if (addedObstacles != null)
			{
				if (sim == null)
				{
					throw new Exception("This should not happen! Make sure you are not overriding the OnEnable function");
				}
				for (int i = 0; i < addedObstacles.Count; i++)
				{
					sim.RemoveObstacle(addedObstacles[i]);
				}
			}
		}

		public void OnEnable()
		{
			if (addedObstacles == null)
			{
				return;
			}
			if (sim == null)
			{
				throw new Exception("This should not happen! Make sure you are not overriding the OnDisable function");
			}
			for (int i = 0; i < addedObstacles.Count; i++)
			{
				ObstacleVertex obstacleVertex = addedObstacles[i];
				ObstacleVertex obstacleVertex2 = obstacleVertex;
				do
				{
					obstacleVertex.layer = layer;
					obstacleVertex = obstacleVertex.next;
				}
				while (obstacleVertex != obstacleVertex2);
				sim.AddObstacle(addedObstacles[i]);
			}
		}

		public void Start()
		{
			addedObstacles = new List<ObstacleVertex>();
			sourceObstacles = new List<Vector3[]>();
			prevUpdateMatrix = GetMatrix();
			CreateObstacles();
		}

		public void Update()
		{
			Matrix4x4 matrix = GetMatrix();
			if (matrix != prevUpdateMatrix)
			{
				for (int i = 0; i < addedObstacles.Count; i++)
				{
					sim.UpdateObstacle(addedObstacles[i], sourceObstacles[i], matrix);
				}
				prevUpdateMatrix = matrix;
			}
		}

		protected void FindSimulator()
		{
			if (RVOSimulator.active == null)
			{
				throw new InvalidOperationException("No RVOSimulator could be found in the scene. Please add one to any GameObject");
			}
			sim = RVOSimulator.active.GetSimulator();
		}

		protected void AddObstacle(Vector3[] vertices, float height)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException("Vertices Must Not Be Null");
			}
			if (height < 0f)
			{
				throw new ArgumentOutOfRangeException("Height must be non-negative");
			}
			if (vertices.Length < 2)
			{
				throw new ArgumentException("An obstacle must have at least two vertices");
			}
			if (sim == null)
			{
				FindSimulator();
			}
			if (gizmoDrawing)
			{
				Vector3[] array = new Vector3[vertices.Length];
				WindCorrectly(vertices);
				Array.Copy(vertices, array, vertices.Length);
				gizmoVerts.Add(array);
			}
			else if (vertices.Length == 2)
			{
				AddObstacleInternal(vertices, height);
			}
			else
			{
				WindCorrectly(vertices);
				AddObstacleInternal(vertices, height);
			}
		}

		private void AddObstacleInternal(Vector3[] vertices, float height)
		{
			addedObstacles.Add(sim.AddObstacle(vertices, height, GetMatrix(), layer));
			sourceObstacles.Add(vertices);
		}

		private void WindCorrectly(Vector3[] vertices)
		{
			int num = 0;
			float num2 = float.PositiveInfinity;
			Matrix4x4 matrix = GetMatrix();
			for (int i = 0; i < vertices.Length; i++)
			{
				float x = matrix.MultiplyPoint3x4(vertices[i]).x;
				if (x < num2)
				{
					num = i;
					num2 = x;
				}
			}
			Vector3 a = matrix.MultiplyPoint3x4(vertices[(num - 1 + vertices.Length) % vertices.Length]);
			Vector3 b = matrix.MultiplyPoint3x4(vertices[num]);
			Vector3 c = matrix.MultiplyPoint3x4(vertices[(num + 1) % vertices.Length]);
			MovementPlane movementPlane = ((sim != null) ? sim.movementPlane : (RVOSimulator.active ? RVOSimulator.active.movementPlane : MovementPlane.XZ));
			if (movementPlane == MovementPlane.XY)
			{
				a.z = a.y;
				b.z = b.y;
				c.z = c.y;
			}
			if (VectorMath.IsClockwiseXZ(a, b, c) != (obstacleMode == ObstacleVertexWinding.KeepIn))
			{
				Array.Reverse(vertices);
			}
		}
	}
}
