using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class RichPath
	{
		private int currentPart;

		private readonly List<RichPathPart> parts = new List<RichPathPart>();

		public Seeker seeker;

		public ITransform transform;

		public Vector3 Endpoint { get; private set; }

		public bool CompletedAllParts
		{
			get
			{
				return currentPart >= parts.Count;
			}
		}

		public bool IsLastPart
		{
			get
			{
				return currentPart >= parts.Count - 1;
			}
		}

		public RichPath()
		{
			Clear();
		}

		public void Clear()
		{
			parts.Clear();
			currentPart = 0;
			Endpoint = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		public void Initialize(Seeker seeker, Path path, bool mergePartEndpoints, bool simplificationMode)
		{
			if (path.error)
			{
				throw new ArgumentException("Path has an error");
			}
			List<GraphNode> path2 = path.path;
			if (path2.Count == 0)
			{
				throw new ArgumentException("Path traverses no nodes");
			}
			this.seeker = seeker;
			for (int i = 0; i < parts.Count; i++)
			{
				RichFunnel obj = parts[i] as RichFunnel;
				RichSpecial obj2 = parts[i] as RichSpecial;
				if (obj != null)
				{
					ObjectPool<RichFunnel>.Release(ref obj);
				}
				else if (obj2 != null)
				{
					ObjectPool<RichSpecial>.Release(ref obj2);
				}
			}
			Clear();
			Endpoint = path.vectorPath[path.vectorPath.Count - 1];
			for (int j = 0; j < path2.Count; j++)
			{
				if (path2[j] is TriangleMeshNode)
				{
					NavmeshBase navmeshBase = AstarData.GetGraph(path2[j]) as NavmeshBase;
					if (navmeshBase == null)
					{
						throw new Exception("Found a TriangleMeshNode that was not in a NavmeshBase graph");
					}
					RichFunnel richFunnel = ObjectPool<RichFunnel>.Claim().Initialize(this, navmeshBase);
					richFunnel.funnelSimplification = simplificationMode;
					int num = j;
					for (uint graphIndex = path2[num].GraphIndex; j < path2.Count && (path2[j].GraphIndex == graphIndex || path2[j] is NodeLink3Node); j++)
					{
					}
					j--;
					if (num == 0)
					{
						richFunnel.exactStart = path.vectorPath[0];
					}
					else
					{
						richFunnel.exactStart = (Vector3)path2[(!mergePartEndpoints) ? num : (num - 1)].position;
					}
					if (j == path2.Count - 1)
					{
						richFunnel.exactEnd = path.vectorPath[path.vectorPath.Count - 1];
					}
					else
					{
						richFunnel.exactEnd = (Vector3)path2[(!mergePartEndpoints) ? j : (j + 1)].position;
					}
					richFunnel.BuildFunnelCorridor(path2, num, j);
					parts.Add(richFunnel);
				}
				else if (NodeLink2.GetNodeLink(path2[j]) != null)
				{
					NodeLink2 nodeLink = NodeLink2.GetNodeLink(path2[j]);
					int num2 = j;
					uint graphIndex2 = path2[num2].GraphIndex;
					for (j++; j < path2.Count && path2[j].GraphIndex == graphIndex2; j++)
					{
					}
					j--;
					if (j - num2 > 1)
					{
						throw new Exception("NodeLink2 path length greater than two (2) nodes. " + (j - num2));
					}
					if (j - num2 != 0)
					{
						RichSpecial item = ObjectPool<RichSpecial>.Claim().Initialize(nodeLink, path2[num2]);
						parts.Add(item);
					}
				}
			}
		}

		public void NextPart()
		{
			currentPart = Mathf.Min(currentPart + 1, parts.Count);
		}

		public RichPathPart GetCurrentPart()
		{
			if (parts.Count == 0)
			{
				return null;
			}
			return (currentPart >= parts.Count) ? parts[parts.Count - 1] : parts[currentPart];
		}
	}
}
