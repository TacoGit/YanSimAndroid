using System;
using UnityEngine;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/Navmesh/RecastTileUpdateHandler")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_recast_tile_update_handler.php")]
	public class RecastTileUpdateHandler : MonoBehaviour
	{
		private RecastGraph graph;

		private bool[] dirtyTiles;

		private bool anyDirtyTiles;

		private float earliestDirty = float.NegativeInfinity;

		public float maxThrottlingDelay = 0.5f;

		public void SetGraph(RecastGraph graph)
		{
			this.graph = graph;
			if (graph != null)
			{
				dirtyTiles = new bool[graph.tileXCount * graph.tileZCount];
				anyDirtyTiles = false;
			}
		}

		public void ScheduleUpdate(Bounds bounds)
		{
			if (graph == null)
			{
				if (AstarPath.active != null)
				{
					SetGraph(AstarPath.active.data.recastGraph);
				}
				if (graph == null)
				{
					Debug.LogError("Received tile update request (from RecastTileUpdate), but no RecastGraph could be found to handle it");
					return;
				}
			}
			int num = Mathf.CeilToInt(graph.characterRadius / graph.cellSize);
			int num2 = num + 3;
			bounds.Expand(new Vector3(num2, 0f, num2) * graph.cellSize * 2f);
			IntRect touchingTiles = graph.GetTouchingTiles(bounds);
			if (touchingTiles.Width * touchingTiles.Height <= 0)
			{
				return;
			}
			if (!anyDirtyTiles)
			{
				earliestDirty = Time.time;
				anyDirtyTiles = true;
			}
			for (int i = touchingTiles.ymin; i <= touchingTiles.ymax; i++)
			{
				for (int j = touchingTiles.xmin; j <= touchingTiles.xmax; j++)
				{
					dirtyTiles[i * graph.tileXCount + j] = true;
				}
			}
		}

		private void OnEnable()
		{
			RecastTileUpdate.OnNeedUpdates += ScheduleUpdate;
		}

		private void OnDisable()
		{
			RecastTileUpdate.OnNeedUpdates -= ScheduleUpdate;
		}

		private void Update()
		{
			if (anyDirtyTiles && Time.time - earliestDirty >= maxThrottlingDelay && graph != null)
			{
				UpdateDirtyTiles();
			}
		}

		public void UpdateDirtyTiles()
		{
			if (graph == null)
			{
				new InvalidOperationException("No graph is set on this object");
			}
			if (graph.tileXCount * graph.tileZCount != dirtyTiles.Length)
			{
				Debug.LogError("Graph has changed dimensions. Clearing queued graph updates and resetting.");
				SetGraph(graph);
				return;
			}
			for (int i = 0; i < graph.tileZCount; i++)
			{
				for (int j = 0; j < graph.tileXCount; j++)
				{
					if (dirtyTiles[i * graph.tileXCount + j])
					{
						dirtyTiles[i * graph.tileXCount + j] = false;
						Bounds tileBounds = graph.GetTileBounds(j, i);
						tileBounds.extents *= 0.5f;
						GraphUpdateObject graphUpdateObject = new GraphUpdateObject(tileBounds);
						graphUpdateObject.nnConstraint.graphMask = 1 << (int)graph.graphIndex;
						AstarPath.active.UpdateGraphs(graphUpdateObject);
					}
				}
			}
			anyDirtyTiles = false;
		}
	}
}
