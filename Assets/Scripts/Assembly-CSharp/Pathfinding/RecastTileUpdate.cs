using System;
using UnityEngine;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/Navmesh/RecastTileUpdate")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_recast_tile_update.php")]
	public class RecastTileUpdate : MonoBehaviour
	{
		public static event Action<Bounds> OnNeedUpdates;

		private void Start()
		{
			ScheduleUpdate();
		}

		private void OnDestroy()
		{
			ScheduleUpdate();
		}

		public void ScheduleUpdate()
		{
			Collider component = GetComponent<Collider>();
			if (component != null)
			{
				if (RecastTileUpdate.OnNeedUpdates != null)
				{
					RecastTileUpdate.OnNeedUpdates(component.bounds);
				}
			}
			else if (RecastTileUpdate.OnNeedUpdates != null)
			{
				RecastTileUpdate.OnNeedUpdates(new Bounds(base.transform.position, Vector3.zero));
			}
		}
	}
}
