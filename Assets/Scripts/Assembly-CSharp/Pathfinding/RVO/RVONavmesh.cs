using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.RVO
{
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Navmesh")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_navmesh.php")]
	public class RVONavmesh : GraphModifier
	{
		public float wallHeight = 5f;

		private readonly List<ObstacleVertex> obstacles = new List<ObstacleVertex>();

		private Simulator lastSim;

		public override void OnPostCacheLoad()
		{
			OnLatePostScan();
		}

		public override void OnGraphsPostUpdate()
		{
			OnLatePostScan();
		}

		public override void OnLatePostScan()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			RemoveObstacles();
			NavGraph[] graphs = AstarPath.active.graphs;
			RVOSimulator active = RVOSimulator.active;
			if (active == null)
			{
				throw new NullReferenceException("No RVOSimulator could be found in the scene. Please add one to any GameObject");
			}
			lastSim = active.GetSimulator();
			for (int i = 0; i < graphs.Length; i++)
			{
				RecastGraph recastGraph = graphs[i] as RecastGraph;
				INavmesh navmesh = graphs[i] as INavmesh;
				GridGraph gridGraph = graphs[i] as GridGraph;
				if (recastGraph != null)
				{
					NavmeshTile[] tiles = recastGraph.GetTiles();
					foreach (NavmeshTile navmesh2 in tiles)
					{
						AddGraphObstacles(lastSim, navmesh2);
					}
				}
				else if (navmesh != null)
				{
					AddGraphObstacles(lastSim, navmesh);
				}
				else if (gridGraph != null)
				{
					AddGraphObstacles(lastSim, gridGraph);
				}
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			RemoveObstacles();
		}

		public void RemoveObstacles()
		{
			if (lastSim != null)
			{
				for (int i = 0; i < obstacles.Count; i++)
				{
					lastSim.RemoveObstacle(obstacles[i]);
				}
				lastSim = null;
			}
			obstacles.Clear();
		}

		private void AddGraphObstacles(Simulator sim, GridGraph grid)
		{
			bool reverse = Vector3.Dot(grid.transform.TransformVector(Vector3.up), (sim.movementPlane != MovementPlane.XY) ? Vector3.up : Vector3.back) > 0f;
			GraphUtilities.GetContours(grid, delegate(Vector3[] vertices)
			{
				if (reverse)
				{
					Array.Reverse(vertices);
				}
				obstacles.Add(sim.AddObstacle(vertices, wallHeight));
			}, wallHeight * 0.4f);
		}

		private void AddGraphObstacles(Simulator simulator, INavmesh navmesh)
		{
			GraphUtilities.GetContours(navmesh, delegate(List<Int3> vertices, bool cycle)
			{
				Vector3[] array = new Vector3[vertices.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (Vector3)vertices[i];
				}
				ListPool<Int3>.Release(vertices);
				obstacles.Add(simulator.AddObstacle(array, wallHeight, cycle));
			});
		}
	}
}
