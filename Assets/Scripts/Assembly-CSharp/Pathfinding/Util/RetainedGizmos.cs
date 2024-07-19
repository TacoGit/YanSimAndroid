using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	public class RetainedGizmos
	{
		public struct Hasher
		{
			private ulong hash;

			private bool includePathSearchInfo;

			private PathHandler debugData;

			public ulong Hash
			{
				get
				{
					return hash;
				}
			}

			public Hasher(AstarPath active)
			{
				hash = 0uL;
				debugData = active.debugPathData;
				includePathSearchInfo = debugData != null && (active.debugMode == GraphDebugMode.F || active.debugMode == GraphDebugMode.G || active.debugMode == GraphDebugMode.H || active.showSearchTree);
				AddHash((int)active.debugMode);
				AddHash(active.debugFloor.GetHashCode());
				AddHash(active.debugRoof.GetHashCode());
			}

			public void AddHash(int hash)
			{
				this.hash = (1572869 * this.hash) ^ (ulong)hash;
			}

			public void HashNode(GraphNode node)
			{
				AddHash(node.GetGizmoHashCode());
				if (includePathSearchInfo)
				{
					PathNode pathNode = debugData.GetPathNode(node.NodeIndex);
					AddHash(pathNode.pathID);
					AddHash((pathNode.pathID == debugData.PathID) ? 1 : 0);
					AddHash((int)pathNode.F);
				}
			}
		}

		public class Builder : IAstarPooledObject
		{
			private List<Vector3> lines = new List<Vector3>();

			private List<Color32> lineColors = new List<Color32>();

			private List<Mesh> meshes = new List<Mesh>();

			public void DrawMesh(RetainedGizmos gizmos, Vector3[] vertices, List<int> triangles, Color[] colors)
			{
				Mesh mesh = gizmos.GetMesh();
				mesh.vertices = vertices;
				mesh.SetTriangles(triangles, 0);
				mesh.colors = colors;
				mesh.UploadMeshData(true);
				meshes.Add(mesh);
			}

			public void DrawWireCube(GraphTransform tr, Bounds bounds, Color color)
			{
				Vector3 min = bounds.min;
				Vector3 max = bounds.max;
				DrawLine(tr.Transform(new Vector3(min.x, min.y, min.z)), tr.Transform(new Vector3(max.x, min.y, min.z)), color);
				DrawLine(tr.Transform(new Vector3(max.x, min.y, min.z)), tr.Transform(new Vector3(max.x, min.y, max.z)), color);
				DrawLine(tr.Transform(new Vector3(max.x, min.y, max.z)), tr.Transform(new Vector3(min.x, min.y, max.z)), color);
				DrawLine(tr.Transform(new Vector3(min.x, min.y, max.z)), tr.Transform(new Vector3(min.x, min.y, min.z)), color);
				DrawLine(tr.Transform(new Vector3(min.x, max.y, min.z)), tr.Transform(new Vector3(max.x, max.y, min.z)), color);
				DrawLine(tr.Transform(new Vector3(max.x, max.y, min.z)), tr.Transform(new Vector3(max.x, max.y, max.z)), color);
				DrawLine(tr.Transform(new Vector3(max.x, max.y, max.z)), tr.Transform(new Vector3(min.x, max.y, max.z)), color);
				DrawLine(tr.Transform(new Vector3(min.x, max.y, max.z)), tr.Transform(new Vector3(min.x, max.y, min.z)), color);
				DrawLine(tr.Transform(new Vector3(min.x, min.y, min.z)), tr.Transform(new Vector3(min.x, max.y, min.z)), color);
				DrawLine(tr.Transform(new Vector3(max.x, min.y, min.z)), tr.Transform(new Vector3(max.x, max.y, min.z)), color);
				DrawLine(tr.Transform(new Vector3(max.x, min.y, max.z)), tr.Transform(new Vector3(max.x, max.y, max.z)), color);
				DrawLine(tr.Transform(new Vector3(min.x, min.y, max.z)), tr.Transform(new Vector3(min.x, max.y, max.z)), color);
			}

			public void DrawLine(Vector3 start, Vector3 end, Color color)
			{
				lines.Add(start);
				lines.Add(end);
				Color32 item = color;
				lineColors.Add(item);
				lineColors.Add(item);
			}

			public void Submit(RetainedGizmos gizmos, Hasher hasher)
			{
				SubmitLines(gizmos, hasher.Hash);
				SubmitMeshes(gizmos, hasher.Hash);
			}

			private void SubmitMeshes(RetainedGizmos gizmos, ulong hash)
			{
				for (int i = 0; i < meshes.Count; i++)
				{
					gizmos.meshes.Add(new MeshWithHash
					{
						hash = hash,
						mesh = meshes[i],
						lines = false
					});
					gizmos.existingHashes.Add(hash);
				}
			}

			private void SubmitLines(RetainedGizmos gizmos, ulong hash)
			{
				int num = (lines.Count + 32766 - 1) / 32766;
				for (int i = 0; i < num; i++)
				{
					int num2 = 32766 * i;
					int num3 = Mathf.Min(num2 + 32766, lines.Count);
					int num4 = num3 - num2;
					List<Vector3> list = ListPool<Vector3>.Claim(num4 * 2);
					List<Color32> list2 = ListPool<Color32>.Claim(num4 * 2);
					List<Vector3> list3 = ListPool<Vector3>.Claim(num4 * 2);
					List<Vector2> list4 = ListPool<Vector2>.Claim(num4 * 2);
					List<int> list5 = ListPool<int>.Claim(num4 * 3);
					for (int j = num2; j < num3; j++)
					{
						Vector3 item = lines[j];
						list.Add(item);
						list.Add(item);
						Color32 item2 = lineColors[j];
						list2.Add(item2);
						list2.Add(item2);
						list4.Add(new Vector2(0f, 0f));
						list4.Add(new Vector2(1f, 0f));
					}
					for (int k = num2; k < num3; k += 2)
					{
						Vector3 item3 = lines[k + 1] - lines[k];
						list3.Add(item3);
						list3.Add(item3);
						list3.Add(item3);
						list3.Add(item3);
					}
					int num5 = 0;
					int num6 = 0;
					while (num5 < num4 * 3)
					{
						list5.Add(num6);
						list5.Add(num6 + 1);
						list5.Add(num6 + 2);
						list5.Add(num6 + 1);
						list5.Add(num6 + 3);
						list5.Add(num6 + 2);
						num5 += 6;
						num6 += 4;
					}
					Mesh mesh = gizmos.GetMesh();
					mesh.SetVertices(list);
					mesh.SetTriangles(list5, 0);
					mesh.SetColors(list2);
					mesh.SetNormals(list3);
					mesh.SetUVs(0, list4);
					mesh.UploadMeshData(true);
					ListPool<Vector3>.Release(ref list);
					ListPool<Color32>.Release(ref list2);
					ListPool<Vector3>.Release(ref list3);
					ListPool<Vector2>.Release(ref list4);
					ListPool<int>.Release(ref list5);
					gizmos.meshes.Add(new MeshWithHash
					{
						hash = hash,
						mesh = mesh,
						lines = true
					});
					gizmos.existingHashes.Add(hash);
				}
			}

			void IAstarPooledObject.OnEnterPool()
			{
				lines.Clear();
				lineColors.Clear();
				meshes.Clear();
			}
		}

		private struct MeshWithHash
		{
			public ulong hash;

			public Mesh mesh;

			public bool lines;
		}

		private List<MeshWithHash> meshes = new List<MeshWithHash>();

		private HashSet<ulong> usedHashes = new HashSet<ulong>();

		private HashSet<ulong> existingHashes = new HashSet<ulong>();

		private Stack<Mesh> cachedMeshes = new Stack<Mesh>();

		public Material surfaceMaterial;

		public Material lineMaterial;

		public GraphGizmoHelper GetSingleFrameGizmoHelper(AstarPath active)
		{
			Hasher hasher = default(Hasher);
			hasher.AddHash(Time.realtimeSinceStartup.GetHashCode());
			Draw(hasher);
			return GetGizmoHelper(active, hasher);
		}

		public GraphGizmoHelper GetGizmoHelper(AstarPath active, Hasher hasher)
		{
			GraphGizmoHelper graphGizmoHelper = ObjectPool<GraphGizmoHelper>.Claim();
			graphGizmoHelper.Init(active, hasher, this);
			return graphGizmoHelper;
		}

		private void PoolMesh(Mesh mesh)
		{
			mesh.Clear();
			cachedMeshes.Push(mesh);
		}

		private Mesh GetMesh()
		{
			if (cachedMeshes.Count > 0)
			{
				return cachedMeshes.Pop();
			}
			Mesh mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
			return mesh;
		}

		public bool HasCachedMesh(Hasher hasher)
		{
			return existingHashes.Contains(hasher.Hash);
		}

		public bool Draw(Hasher hasher)
		{
			usedHashes.Add(hasher.Hash);
			return HasCachedMesh(hasher);
		}

		public void DrawExisting()
		{
			for (int i = 0; i < meshes.Count; i++)
			{
				usedHashes.Add(meshes[i].hash);
			}
		}

		public void FinalizeDraw()
		{
			RemoveUnusedMeshes(meshes);
			Camera current = Camera.current;
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(current);
			if (surfaceMaterial == null || lineMaterial == null)
			{
				return;
			}
			for (int i = 0; i <= 1; i++)
			{
				Material material = ((i != 0) ? lineMaterial : surfaceMaterial);
				for (int j = 0; j < material.passCount; j++)
				{
					material.SetPass(j);
					for (int k = 0; k < meshes.Count; k++)
					{
						if (meshes[k].lines == (material == lineMaterial) && GeometryUtility.TestPlanesAABB(planes, meshes[k].mesh.bounds))
						{
							Graphics.DrawMeshNow(meshes[k].mesh, Matrix4x4.identity);
						}
					}
				}
			}
			usedHashes.Clear();
		}

		public void ClearCache()
		{
			usedHashes.Clear();
			RemoveUnusedMeshes(meshes);
			while (cachedMeshes.Count > 0)
			{
				Object.DestroyImmediate(cachedMeshes.Pop());
			}
		}

		private void RemoveUnusedMeshes(List<MeshWithHash> meshList)
		{
			int num = 0;
			int num2 = 0;
			while (num < meshList.Count)
			{
				if (num2 == meshList.Count)
				{
					num2--;
					meshList.RemoveAt(num2);
				}
				else if (usedHashes.Contains(meshList[num2].hash))
				{
					meshList[num] = meshList[num2];
					num++;
					num2++;
				}
				else
				{
					PoolMesh(meshList[num2].mesh);
					existingHashes.Remove(meshList[num2].hash);
					num2++;
				}
			}
		}
	}
}
