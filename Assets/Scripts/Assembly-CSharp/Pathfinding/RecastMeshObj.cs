using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/Navmesh/RecastMeshObj")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_recast_mesh_obj.php")]
	public class RecastMeshObj : VersionedMonoBehaviour
	{
		protected static RecastBBTree tree = new RecastBBTree();

		protected static List<RecastMeshObj> dynamicMeshObjs = new List<RecastMeshObj>();

		[HideInInspector]
		public Bounds bounds;

		public bool dynamic = true;

		public int area;

		private bool _dynamic;

		private bool registered;

		public static void GetAllInBounds(List<RecastMeshObj> buffer, Bounds bounds)
		{
			if (!Application.isPlaying)
			{
				RecastMeshObj[] array = UnityEngine.Object.FindObjectsOfType(typeof(RecastMeshObj)) as RecastMeshObj[];
				for (int i = 0; i < array.Length; i++)
				{
					array[i].RecalculateBounds();
					if (array[i].GetBounds().Intersects(bounds))
					{
						buffer.Add(array[i]);
					}
				}
				return;
			}
			if (Time.timeSinceLevelLoad == 0f)
			{
				RecastMeshObj[] array2 = UnityEngine.Object.FindObjectsOfType(typeof(RecastMeshObj)) as RecastMeshObj[];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].Register();
				}
			}
			for (int k = 0; k < dynamicMeshObjs.Count; k++)
			{
				if (dynamicMeshObjs[k].GetBounds().Intersects(bounds))
				{
					buffer.Add(dynamicMeshObjs[k]);
				}
			}
			Rect rect = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
			tree.QueryInBounds(rect, buffer);
		}

		private void OnEnable()
		{
			Register();
		}

		private void Register()
		{
			if (!registered)
			{
				registered = true;
				area = Mathf.Clamp(area, -1, 33554432);
				Renderer component = GetComponent<Renderer>();
				Collider component2 = GetComponent<Collider>();
				if (component == null && component2 == null)
				{
					throw new Exception("A renderer or a collider should be attached to the GameObject");
				}
				MeshFilter component3 = GetComponent<MeshFilter>();
				if (component != null && component3 == null)
				{
					throw new Exception("A renderer was attached but no mesh filter");
				}
				bounds = ((!(component != null)) ? component2.bounds : component.bounds);
				_dynamic = dynamic;
				if (_dynamic)
				{
					dynamicMeshObjs.Add(this);
				}
				else
				{
					tree.Insert(this);
				}
			}
		}

		private void RecalculateBounds()
		{
			Renderer component = GetComponent<Renderer>();
			Collider collider = GetCollider();
			if (component == null && collider == null)
			{
				throw new Exception("A renderer or a collider should be attached to the GameObject");
			}
			MeshFilter component2 = GetComponent<MeshFilter>();
			if (component != null && component2 == null)
			{
				throw new Exception("A renderer was attached but no mesh filter");
			}
			bounds = ((!(component != null)) ? collider.bounds : component.bounds);
		}

		public Bounds GetBounds()
		{
			if (_dynamic)
			{
				RecalculateBounds();
			}
			return bounds;
		}

		public MeshFilter GetMeshFilter()
		{
			return GetComponent<MeshFilter>();
		}

		public Collider GetCollider()
		{
			return GetComponent<Collider>();
		}

		private void OnDisable()
		{
			registered = false;
			if (_dynamic)
			{
				dynamicMeshObjs.Remove(this);
			}
			else if (!tree.Remove(this))
			{
				throw new Exception("Could not remove RecastMeshObj from tree even though it should exist in it. Has the object moved without being marked as dynamic?");
			}
			_dynamic = dynamic;
		}
	}
}
