using System;
using UnityEngine;

namespace Pathfinding
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_dynamic_grid_obstacle.php")]
	public class DynamicGridObstacle : GraphModifier
	{
		private Collider coll;

		private Collider2D coll2D;

		private Transform tr;

		public float updateError = 1f;

		public float checkTime = 0.2f;

		private Bounds prevBounds;

		private Quaternion prevRotation;

		private bool prevEnabled;

		private float lastCheckTime = -9999f;

		private Bounds bounds
		{
			get
			{
				if (coll != null)
				{
					return coll.bounds;
				}
				Bounds result = coll2D.bounds;
				result.extents += new Vector3(0f, 0f, 10000f);
				return result;
			}
		}

		private bool colliderEnabled
		{
			get
			{
				return (!(coll != null)) ? coll2D.enabled : coll.enabled;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			coll = GetComponent<Collider>();
			coll2D = GetComponent<Collider2D>();
			tr = base.transform;
			if (coll == null && coll2D == null)
			{
				throw new Exception("A collider or 2D collider must be attached to the GameObject(" + base.gameObject.name + ") for the DynamicGridObstacle to work");
			}
			prevBounds = bounds;
			prevRotation = tr.rotation;
			prevEnabled = false;
		}

		public override void OnPostScan()
		{
			prevEnabled = colliderEnabled;
		}

		private void Update()
		{
			if (coll == null && coll2D == null)
			{
				Debug.LogError("Removed collider from DynamicGridObstacle", this);
				base.enabled = false;
			}
			else
			{
				if (AstarPath.active == null || AstarPath.active.isScanning || Time.realtimeSinceStartup - lastCheckTime < checkTime || !Application.isPlaying)
				{
					return;
				}
				lastCheckTime = Time.realtimeSinceStartup;
				if (colliderEnabled)
				{
					Bounds bounds = this.bounds;
					Quaternion rotation = tr.rotation;
					Vector3 vector = prevBounds.min - bounds.min;
					Vector3 vector2 = prevBounds.max - bounds.max;
					float magnitude = bounds.extents.magnitude;
					float num = magnitude * Quaternion.Angle(prevRotation, rotation) * ((float)Math.PI / 180f);
					if (vector.sqrMagnitude > updateError * updateError || vector2.sqrMagnitude > updateError * updateError || num > updateError || !prevEnabled)
					{
						DoUpdateGraphs();
					}
				}
				else if (prevEnabled)
				{
					DoUpdateGraphs();
				}
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (AstarPath.active != null && Application.isPlaying)
			{
				GraphUpdateObject ob = new GraphUpdateObject(prevBounds);
				AstarPath.active.UpdateGraphs(ob);
				prevEnabled = false;
			}
		}

		public void DoUpdateGraphs()
		{
			if (coll == null && coll2D == null)
			{
				return;
			}
			if (!colliderEnabled)
			{
				AstarPath.active.UpdateGraphs(prevBounds);
			}
			else
			{
				Bounds bounds = this.bounds;
				Bounds b = bounds;
				b.Encapsulate(prevBounds);
				if (BoundsVolume(b) < BoundsVolume(bounds) + BoundsVolume(prevBounds))
				{
					AstarPath.active.UpdateGraphs(b);
				}
				else
				{
					AstarPath.active.UpdateGraphs(prevBounds);
					AstarPath.active.UpdateGraphs(bounds);
				}
				prevBounds = bounds;
			}
			prevEnabled = colliderEnabled;
			prevRotation = tr.rotation;
			lastCheckTime = Time.realtimeSinceStartup;
		}

		private static float BoundsVolume(Bounds b)
		{
			return Math.Abs(b.size.x * b.size.y * b.size.z);
		}
	}
}
