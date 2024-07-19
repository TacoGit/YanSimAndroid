using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public abstract class NavGraph : IGraphInternals
	{
		public AstarPath active;

		[JsonMember]
		public Pathfinding.Util.Guid guid;

		[JsonMember]
		public uint initialPenalty;

		[JsonMember]
		public bool open;

		public uint graphIndex;

		[JsonMember]
		public string name;

		[JsonMember]
		public bool drawGizmos = true;

		[JsonMember]
		public bool infoScreenOpen;

		[JsonMember]
		private string serializedEditorSettings;

		[Obsolete("Use the transform field (only available on some graph types) instead", true)]
		public Matrix4x4 matrix = Matrix4x4.identity;

		[Obsolete("Use the transform field (only available on some graph types) instead", true)]
		public Matrix4x4 inverseMatrix = Matrix4x4.identity;

		string IGraphInternals.SerializedEditorSettings
		{
			get
			{
				return serializedEditorSettings;
			}
			set
			{
				serializedEditorSettings = value;
			}
		}

		internal bool exists
		{
			get
			{
				return active != null;
			}
		}

		public virtual int CountNodes()
		{
			int count = 0;
			GetNodes(delegate
			{
				count++;
			});
			return count;
		}

		public void GetNodes(Func<GraphNode, bool> action)
		{
			bool cont = true;
			GetNodes(delegate(GraphNode node)
			{
				if (cont)
				{
					cont &= action(node);
				}
			});
		}

		public abstract void GetNodes(Action<GraphNode> action);

		[Obsolete("Use the transform field (only available on some graph types) instead", true)]
		public void SetMatrix(Matrix4x4 m)
		{
			matrix = m;
			inverseMatrix = m.inverse;
		}

		[Obsolete("Use RelocateNodes(Matrix4x4) instead. To keep the same behavior you can call RelocateNodes(newMatrix * oldMatrix.inverse).")]
		public void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
		{
			RelocateNodes(newMatrix * oldMatrix.inverse);
		}

		public virtual void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			GetNodes(delegate(GraphNode node)
			{
				node.position = (Int3)deltaMatrix.MultiplyPoint((Vector3)node.position);
			});
		}

		public NNInfoInternal GetNearest(Vector3 position)
		{
			return GetNearest(position, NNConstraint.None);
		}

		public NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint)
		{
			return GetNearest(position, constraint, null);
		}

		public virtual NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
		{
			float maxDistSqr = ((constraint != null && !constraint.constrainDistance) ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistanceSqr);
			float minDist = float.PositiveInfinity;
			GraphNode minNode = null;
			float minConstDist = float.PositiveInfinity;
			GraphNode minConstNode = null;
			GetNodes(delegate(GraphNode node)
			{
				float sqrMagnitude = (position - (Vector3)node.position).sqrMagnitude;
				if (sqrMagnitude < minDist)
				{
					minDist = sqrMagnitude;
					minNode = node;
				}
				if (sqrMagnitude < minConstDist && sqrMagnitude < maxDistSqr && (constraint == null || constraint.Suitable(node)))
				{
					minConstDist = sqrMagnitude;
					minConstNode = node;
				}
			});
			NNInfoInternal result = new NNInfoInternal(minNode);
			result.constrainedNode = minConstNode;
			if (minConstNode != null)
			{
				result.constClampedPosition = (Vector3)minConstNode.position;
			}
			else if (minNode != null)
			{
				result.constrainedNode = minNode;
				result.constClampedPosition = (Vector3)minNode.position;
			}
			return result;
		}

		public virtual NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
		{
			return GetNearest(position, constraint);
		}

		protected virtual void OnDestroy()
		{
			DestroyAllNodes();
		}

		protected virtual void DestroyAllNodes()
		{
			GetNodes(delegate(GraphNode node)
			{
				node.Destroy();
			});
		}

		[Obsolete("Use AstarPath.Scan instead")]
		public void ScanGraph()
		{
			Scan();
		}

		public void Scan()
		{
			active.Scan(this);
		}

		protected abstract IEnumerable<Progress> ScanInternal();

		protected virtual void SerializeExtraInfo(GraphSerializationContext ctx)
		{
		}

		protected virtual void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
		}

		protected virtual void PostDeserialization(GraphSerializationContext ctx)
		{
		}

		protected virtual void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			guid = new Pathfinding.Util.Guid(ctx.reader.ReadBytes(16));
			initialPenalty = ctx.reader.ReadUInt32();
			open = ctx.reader.ReadBoolean();
			name = ctx.reader.ReadString();
			drawGizmos = ctx.reader.ReadBoolean();
			infoScreenOpen = ctx.reader.ReadBoolean();
		}

		public virtual void OnDrawGizmos(RetainedGizmos gizmos, bool drawNodes)
		{
			if (!drawNodes)
			{
				return;
			}
			RetainedGizmos.Hasher hasher = new RetainedGizmos.Hasher(active);
			GetNodes(delegate(GraphNode node)
			{
				hasher.HashNode(node);
			});
			if (!gizmos.Draw(hasher))
			{
				using (GraphGizmoHelper @object = gizmos.GetGizmoHelper(active, hasher))
				{
					GetNodes((Action<GraphNode>)@object.DrawConnections);
				}
			}
			if (active.showUnwalkableNodes)
			{
				DrawUnwalkableNodes(active.unwalkableNodeDebugSize);
			}
		}

		protected void DrawUnwalkableNodes(float size)
		{
			Gizmos.color = AstarColor.UnwalkableNode;
			GetNodes(delegate(GraphNode node)
			{
				if (!node.Walkable)
				{
					Gizmos.DrawCube((Vector3)node.position, Vector3.one * size);
				}
			});
		}

		void IGraphInternals.OnDestroy()
		{
			OnDestroy();
		}

		void IGraphInternals.DestroyAllNodes()
		{
			DestroyAllNodes();
		}

		IEnumerable<Progress> IGraphInternals.ScanInternal()
		{
			return ScanInternal();
		}

		void IGraphInternals.SerializeExtraInfo(GraphSerializationContext ctx)
		{
			SerializeExtraInfo(ctx);
		}

		void IGraphInternals.DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			DeserializeExtraInfo(ctx);
		}

		void IGraphInternals.PostDeserialization(GraphSerializationContext ctx)
		{
			PostDeserialization(ctx);
		}

		void IGraphInternals.DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			DeserializeSettingsCompatibility(ctx);
		}
	}
}
