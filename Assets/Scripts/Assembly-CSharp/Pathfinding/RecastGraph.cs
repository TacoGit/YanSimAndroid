using System;
using System.Collections.Generic;
using Pathfinding.Recast;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Pathfinding.Voxels;
using UnityEngine;

namespace Pathfinding
{
	[JsonOptIn]
	public class RecastGraph : NavmeshBase, IUpdatableGraph
	{
		public enum RelevantGraphSurfaceMode
		{
			DoNotRequire = 0,
			OnlyForCompletelyInsideTile = 1,
			RequireForAll = 2
		}

		[JsonMember]
		public float characterRadius = 1.5f;

		[JsonMember]
		public float contourMaxError = 2f;

		[JsonMember]
		public float cellSize = 0.5f;

		[JsonMember]
		public float walkableHeight = 2f;

		[JsonMember]
		public float walkableClimb = 0.5f;

		[JsonMember]
		public float maxSlope = 30f;

		[JsonMember]
		public float maxEdgeLength = 20f;

		[JsonMember]
		public float minRegionSize = 3f;

		[JsonMember]
		public int editorTileSize = 128;

		[JsonMember]
		public int tileSizeX = 128;

		[JsonMember]
		public int tileSizeZ = 128;

		[JsonMember]
		public bool useTiles = true;

		public bool scanEmptyGraph;

		[JsonMember]
		public RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		[JsonMember]
		public bool rasterizeColliders;

		[JsonMember]
		public bool rasterizeMeshes = true;

		[JsonMember]
		public bool rasterizeTerrain = true;

		[JsonMember]
		public bool rasterizeTrees = true;

		[JsonMember]
		public float colliderRasterizeDetail = 10f;

		[JsonMember]
		public LayerMask mask = -1;

		[JsonMember]
		public List<string> tagMask = new List<string>();

		[JsonMember]
		public int terrainSampleSize = 3;

		[JsonMember]
		public Vector3 rotation;

		[JsonMember]
		public Vector3 forcedBoundsCenter;

		private Voxelize globalVox;

		public const int BorderVertexMask = 1;

		public const int BorderVertexOffset = 31;

		private List<NavmeshTile> stagingTiles = new List<NavmeshTile>();

		protected override bool RecalculateNormals
		{
			get
			{
				return true;
			}
		}

		public override float TileWorldSizeX
		{
			get
			{
				return (float)tileSizeX * cellSize;
			}
		}

		public override float TileWorldSizeZ
		{
			get
			{
				return (float)tileSizeZ * cellSize;
			}
		}

		protected override float MaxTileConnectionEdgeDistance
		{
			get
			{
				return walkableClimb;
			}
		}

		[Obsolete("Obsolete since this is not accurate when the graph is rotated (rotation was not supported when this property was created)")]
		public Bounds forcedBounds
		{
			get
			{
				return new Bounds(forcedBoundsCenter, forcedBoundsSize);
			}
		}

		private float CellHeight
		{
			get
			{
				return Mathf.Max(forcedBoundsSize.y / 64000f, 0.001f);
			}
		}

		private int CharacterRadiusInVoxels
		{
			get
			{
				return Mathf.CeilToInt(characterRadius / cellSize - 0.1f);
			}
		}

		private int TileBorderSizeInVoxels
		{
			get
			{
				return CharacterRadiusInVoxels + 3;
			}
		}

		private float TileBorderSizeInWorldUnits
		{
			get
			{
				return (float)TileBorderSizeInVoxels * cellSize;
			}
		}

		[Obsolete("Use node.ClosestPointOnNode instead")]
		public Vector3 ClosestPointOnNode(TriangleMeshNode node, Vector3 pos)
		{
			return node.ClosestPointOnNode(pos);
		}

		[Obsolete("Use node.ContainsPoint instead")]
		public bool ContainsPoint(TriangleMeshNode node, Vector3 pos)
		{
			return node.ContainsPoint((Int3)pos);
		}

		public void SnapForceBoundsToScene()
		{
			List<RasterizationMesh> list = CollectMeshes(new Bounds(Vector3.zero, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)));
			if (list.Count != 0)
			{
				Bounds bounds = list[0].bounds;
				for (int i = 1; i < list.Count; i++)
				{
					bounds.Encapsulate(list[i].bounds);
					list[i].Pool();
				}
				forcedBoundsCenter = bounds.center;
				forcedBoundsSize = bounds.size;
			}
		}

		GraphUpdateThreading IUpdatableGraph.CanUpdateAsync(GraphUpdateObject o)
		{
			return (!o.updatePhysics) ? GraphUpdateThreading.SeparateThread : ((GraphUpdateThreading)7);
		}

		void IUpdatableGraph.UpdateAreaInit(GraphUpdateObject o)
		{
			if (o.updatePhysics)
			{
				RelevantGraphSurface.UpdateAllPositions();
				IntRect touchingTiles = GetTouchingTiles(o.bounds);
				Bounds tileBounds = GetTileBounds(touchingTiles);
				tileBounds.Expand(new Vector3(1f, 0f, 1f) * TileBorderSizeInWorldUnits * 2f);
				List<RasterizationMesh> inputMeshes = CollectMeshes(tileBounds);
				if (globalVox == null)
				{
					globalVox = new Voxelize(CellHeight, cellSize, walkableClimb, walkableHeight, maxSlope, maxEdgeLength);
				}
				globalVox.inputMeshes = inputMeshes;
			}
		}

		void IUpdatableGraph.UpdateArea(GraphUpdateObject guo)
		{
			IntRect touchingTiles = GetTouchingTiles(guo.bounds);
			if (!guo.updatePhysics)
			{
				for (int i = touchingTiles.ymin; i <= touchingTiles.ymax; i++)
				{
					for (int j = touchingTiles.xmin; j <= touchingTiles.xmax; j++)
					{
						NavmeshTile graph = tiles[i * tileXCount + j];
						NavMeshGraph.UpdateArea(guo, graph);
					}
				}
				return;
			}
			Voxelize voxelize = globalVox;
			if (voxelize == null)
			{
				throw new InvalidOperationException("No Voxelizer object. UpdateAreaInit should have been called before this function.");
			}
			for (int k = touchingTiles.xmin; k <= touchingTiles.xmax; k++)
			{
				for (int l = touchingTiles.ymin; l <= touchingTiles.ymax; l++)
				{
					stagingTiles.Add(BuildTileMesh(voxelize, k, l));
				}
			}
			uint num = (uint)AstarPath.active.data.GetGraphIndex(this);
			for (int m = 0; m < stagingTiles.Count; m++)
			{
				NavmeshTile navmeshTile = stagingTiles[m];
				GraphNode[] nodes = navmeshTile.nodes;
				for (int n = 0; n < nodes.Length; n++)
				{
					nodes[n].GraphIndex = num;
				}
			}
			for (int num2 = 0; num2 < voxelize.inputMeshes.Count; num2++)
			{
				voxelize.inputMeshes[num2].Pool();
			}
			ListPool<RasterizationMesh>.Release(ref voxelize.inputMeshes);
		}

		void IUpdatableGraph.UpdateAreaPost(GraphUpdateObject guo)
		{
			for (int i = 0; i < stagingTiles.Count; i++)
			{
				NavmeshTile navmeshTile = stagingTiles[i];
				int num = navmeshTile.x + navmeshTile.z * tileXCount;
				NavmeshTile navmeshTile2 = tiles[num];
				for (int j = 0; j < navmeshTile2.nodes.Length; j++)
				{
					navmeshTile2.nodes[j].Destroy();
				}
				tiles[num] = navmeshTile;
			}
			for (int k = 0; k < stagingTiles.Count; k++)
			{
				NavmeshTile tile = stagingTiles[k];
				ConnectTileWithNeighbours(tile);
			}
			if (OnRecalculatedTiles != null)
			{
				OnRecalculatedTiles(stagingTiles.ToArray());
			}
			stagingTiles.Clear();
		}

		protected override IEnumerable<Progress> ScanInternal()
		{
			TriangleMeshNode.SetNavmeshHolder(AstarPath.active.data.GetGraphIndex(this), this);
			if (!Application.isPlaying)
			{
				RelevantGraphSurface.FindAllGraphSurfaces();
			}
			RelevantGraphSurface.UpdateAllPositions();
			foreach (Progress item in ScanAllTiles())
			{
				yield return item;
			}
		}

		public override GraphTransform CalculateTransform()
		{
			return new GraphTransform(Matrix4x4.TRS(forcedBoundsCenter, Quaternion.Euler(rotation), Vector3.one) * Matrix4x4.TRS(-forcedBoundsSize * 0.5f, Quaternion.identity, Vector3.one));
		}

		private void InitializeTileInfo()
		{
			int num = (int)(forcedBoundsSize.x / cellSize + 0.5f);
			int num2 = (int)(forcedBoundsSize.z / cellSize + 0.5f);
			if (!useTiles)
			{
				tileSizeX = num;
				tileSizeZ = num2;
			}
			else
			{
				tileSizeX = editorTileSize;
				tileSizeZ = editorTileSize;
			}
			tileXCount = (num + tileSizeX - 1) / tileSizeX;
			tileZCount = (num2 + tileSizeZ - 1) / tileSizeZ;
			if (tileXCount * tileZCount > 524288)
			{
				throw new Exception("Too many tiles (" + tileXCount * tileZCount + ") maximum is " + 524288 + "\nTry disabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* inspector.");
			}
			tiles = new NavmeshTile[tileXCount * tileZCount];
		}

		private List<RasterizationMesh>[] PutMeshesIntoTileBuckets(List<RasterizationMesh> meshes)
		{
			List<RasterizationMesh>[] array = new List<RasterizationMesh>[tiles.Length];
			Vector3 amount = new Vector3(1f, 0f, 1f) * TileBorderSizeInWorldUnits * 2f;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ListPool<RasterizationMesh>.Claim();
			}
			for (int j = 0; j < meshes.Count; j++)
			{
				RasterizationMesh rasterizationMesh = meshes[j];
				Bounds bounds = rasterizationMesh.bounds;
				bounds.Expand(amount);
				IntRect touchingTiles = GetTouchingTiles(bounds);
				for (int k = touchingTiles.ymin; k <= touchingTiles.ymax; k++)
				{
					for (int l = touchingTiles.xmin; l <= touchingTiles.xmax; l++)
					{
						array[l + k * tileXCount].Add(rasterizationMesh);
					}
				}
			}
			return array;
		}

		protected IEnumerable<Progress> ScanAllTiles()
		{
			transform = CalculateTransform();
			InitializeTileInfo();
			if (scanEmptyGraph)
			{
				FillWithEmptyTiles();
				yield break;
			}
			walkableClimb = Mathf.Min(walkableClimb, walkableHeight);
			yield return new Progress(0f, "Finding Meshes");
			Bounds bounds = transform.Transform(new Bounds(forcedBoundsSize * 0.5f, forcedBoundsSize));
			List<RasterizationMesh> meshes = CollectMeshes(bounds);
			List<RasterizationMesh>[] buckets = PutMeshesIntoTileBuckets(meshes);
			Queue<Int2> tileQueue = new Queue<Int2>();
			for (int i = 0; i < tileZCount; i++)
			{
				for (int j = 0; j < tileXCount; j++)
				{
					tileQueue.Enqueue(new Int2(j, i));
				}
			}
			ParallelWorkQueue<Int2> workQueue2 = new ParallelWorkQueue<Int2>(tileQueue);
			Voxelize[] voxelizers = new Voxelize[workQueue2.threadCount];
			for (int k = 0; k < voxelizers.Length; k++)
			{
				voxelizers[k] = new Voxelize(CellHeight, cellSize, walkableClimb, walkableHeight, maxSlope, maxEdgeLength);
			}
			workQueue2.action = delegate(Int2 tile, int threadIndex)
			{
				voxelizers[threadIndex].inputMeshes = buckets[tile.x + tile.y * tileXCount];
				tiles[tile.x + tile.y * tileXCount] = BuildTileMesh(voxelizers[threadIndex], tile.x, tile.y, threadIndex);
			};
			int timeoutMillis = (Application.isPlaying ? 1 : 200);
			foreach (int done2 in workQueue2.Run(timeoutMillis))
			{
				yield return new Progress(Mathf.Lerp(0.1f, 0.9f, (float)done2 / (float)tiles.Length), "Calculated Tiles: " + done2 + "/" + tiles.Length);
			}
			yield return new Progress(0.9f, "Assigning Graph Indices");
			uint graphIndex = (uint)AstarPath.active.data.GetGraphIndex(this);
			GetNodes(delegate(GraphNode node)
			{
				node.GraphIndex = graphIndex;
			});
			for (int coordinateSum = 0; coordinateSum <= 1; coordinateSum++)
			{
				int direction;
				for (direction = 0; direction <= 1; direction++)
				{
					for (int l = 0; l < tiles.Length; l++)
					{
						if ((tiles[l].x + tiles[l].z) % 2 == coordinateSum)
						{
							tileQueue.Enqueue(new Int2(tiles[l].x, tiles[l].z));
						}
					}
					workQueue2 = new ParallelWorkQueue<Int2>(tileQueue)
					{
						action = delegate(Int2 tile, int threadIndex)
						{
							if (direction == 0 && tile.x < tileXCount - 1)
							{
								ConnectTiles(tiles[tile.x + tile.y * tileXCount], tiles[tile.x + 1 + tile.y * tileXCount]);
							}
							if (direction == 1 && tile.y < tileZCount - 1)
							{
								ConnectTiles(tiles[tile.x + tile.y * tileXCount], tiles[tile.x + (tile.y + 1) * tileXCount]);
							}
						}
					};
					int numTilesInQueue = tileQueue.Count;
					foreach (int done in workQueue2.Run(timeoutMillis))
					{
						yield return new Progress(0.95f, "Connected Tiles " + (numTilesInQueue - done) + "/" + numTilesInQueue + " (Phase " + (direction + 1 + 2 * coordinateSum) + " of 4)");
					}
				}
			}
			for (int m = 0; m < meshes.Count; m++)
			{
				meshes[m].Pool();
			}
			ListPool<RasterizationMesh>.Release(ref meshes);
			if (OnRecalculatedTiles != null)
			{
				OnRecalculatedTiles(tiles.Clone() as NavmeshTile[]);
			}
		}

		private List<RasterizationMesh> CollectMeshes(Bounds bounds)
		{
			List<RasterizationMesh> list = ListPool<RasterizationMesh>.Claim();
			RecastMeshGatherer recastMeshGatherer = new RecastMeshGatherer(bounds, terrainSampleSize, mask, tagMask, colliderRasterizeDetail);
			if (rasterizeMeshes)
			{
				recastMeshGatherer.CollectSceneMeshes(list);
			}
			recastMeshGatherer.CollectRecastMeshObjs(list);
			if (rasterizeTerrain)
			{
				float desiredChunkSize = cellSize * (float)Math.Max(tileSizeX, tileSizeZ);
				recastMeshGatherer.CollectTerrainMeshes(rasterizeTrees, desiredChunkSize, list);
			}
			if (rasterizeColliders)
			{
				recastMeshGatherer.CollectColliderMeshes(list);
			}
			if (list.Count == 0)
			{
				Debug.LogWarning("No MeshFilters were found contained in the layers specified by the 'mask' variables");
			}
			return list;
		}

		private Bounds CalculateTileBoundsWithBorder(int x, int z)
		{
			Bounds result = default(Bounds);
			result.SetMinMax(new Vector3((float)x * TileWorldSizeX, 0f, (float)z * TileWorldSizeZ), new Vector3((float)(x + 1) * TileWorldSizeX, forcedBoundsSize.y, (float)(z + 1) * TileWorldSizeZ));
			result.Expand(new Vector3(1f, 0f, 1f) * TileBorderSizeInWorldUnits * 2f);
			return result;
		}

		protected NavmeshTile BuildTileMesh(Voxelize vox, int x, int z, int threadIndex = 0)
		{
			vox.borderSize = TileBorderSizeInVoxels;
			vox.forcedBounds = CalculateTileBoundsWithBorder(x, z);
			vox.width = tileSizeX + vox.borderSize * 2;
			vox.depth = tileSizeZ + vox.borderSize * 2;
			if (!useTiles && relevantGraphSurfaceMode == RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile)
			{
				vox.relevantGraphSurfaceMode = RelevantGraphSurfaceMode.RequireForAll;
			}
			else
			{
				vox.relevantGraphSurfaceMode = relevantGraphSurfaceMode;
			}
			vox.minRegionSize = Mathf.RoundToInt(minRegionSize / (cellSize * cellSize));
			vox.Init();
			vox.VoxelizeInput(transform, CalculateTileBoundsWithBorder(x, z));
			vox.FilterLedges(vox.voxelWalkableHeight, vox.voxelWalkableClimb, vox.cellSize, vox.cellHeight);
			vox.FilterLowHeightSpans(vox.voxelWalkableHeight, vox.cellSize, vox.cellHeight);
			vox.BuildCompactField();
			vox.BuildVoxelConnections();
			vox.ErodeWalkableArea(CharacterRadiusInVoxels);
			vox.BuildDistanceField();
			vox.BuildRegions();
			VoxelContourSet cset = new VoxelContourSet();
			vox.BuildContours(contourMaxError, 1, cset, 5);
			VoxelMesh mesh;
			vox.BuildPolyMesh(cset, 3, out mesh);
			for (int i = 0; i < mesh.verts.Length; i++)
			{
				mesh.verts[i] *= 1000;
			}
			vox.transformVoxel2Graph.Transform(mesh.verts);
			return CreateTile(vox, mesh, x, z, threadIndex);
		}

		private NavmeshTile CreateTile(Voxelize vox, VoxelMesh mesh, int x, int z, int threadIndex)
		{
			if (mesh.tris == null)
			{
				throw new ArgumentNullException("mesh.tris");
			}
			if (mesh.verts == null)
			{
				throw new ArgumentNullException("mesh.verts");
			}
			if (mesh.tris.Length % 3 != 0)
			{
				throw new ArgumentException("Indices array's length must be a multiple of 3 (mesh.tris)");
			}
			if (mesh.verts.Length >= 4095)
			{
				if (tileXCount * tileZCount == 1)
				{
					throw new ArgumentException("Too many vertices per tile (more than " + 4095 + ").\n<b>Try enabling tiling in the recast graph settings.</b>\n");
				}
				throw new ArgumentException("Too many vertices per tile (more than " + 4095 + ").\n<b>Try reducing tile size or enabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector</b>");
			}
			NavmeshTile navmeshTile = new NavmeshTile();
			navmeshTile.x = x;
			navmeshTile.z = z;
			navmeshTile.w = 1;
			navmeshTile.d = 1;
			navmeshTile.tris = mesh.tris;
			navmeshTile.bbTree = new BBTree();
			navmeshTile.graph = this;
			NavmeshTile navmeshTile2 = navmeshTile;
			navmeshTile2.vertsInGraphSpace = Utility.RemoveDuplicateVertices(mesh.verts, navmeshTile2.tris);
			navmeshTile2.verts = (Int3[])navmeshTile2.vertsInGraphSpace.Clone();
			transform.Transform(navmeshTile2.verts);
			uint num = (uint)(active.data.graphs.Length + threadIndex);
			if (num > 255)
			{
				throw new Exception("Graph limit reached. Multithreaded recast calculations cannot be done because a few scratch graph indices are required.");
			}
			TriangleMeshNode.SetNavmeshHolder((int)num, navmeshTile2);
			navmeshTile2.nodes = new TriangleMeshNode[navmeshTile2.tris.Length / 3];
			lock (active)
			{
				CreateNodes(navmeshTile2.nodes, navmeshTile2.tris, x + z * tileXCount, num);
			}
			navmeshTile2.bbTree.RebuildFrom(navmeshTile2.nodes);
			NavmeshBase.CreateNodeConnections(navmeshTile2.nodes);
			TriangleMeshNode.SetNavmeshHolder((int)num, null);
			return navmeshTile2;
		}

		protected override void DeserializeSettingsCompatibility(GraphSerializationContext ctx)
		{
			base.DeserializeSettingsCompatibility(ctx);
			characterRadius = ctx.reader.ReadSingle();
			contourMaxError = ctx.reader.ReadSingle();
			cellSize = ctx.reader.ReadSingle();
			ctx.reader.ReadSingle();
			walkableHeight = ctx.reader.ReadSingle();
			maxSlope = ctx.reader.ReadSingle();
			maxEdgeLength = ctx.reader.ReadSingle();
			editorTileSize = ctx.reader.ReadInt32();
			tileSizeX = ctx.reader.ReadInt32();
			nearestSearchOnlyXZ = ctx.reader.ReadBoolean();
			useTiles = ctx.reader.ReadBoolean();
			relevantGraphSurfaceMode = (RelevantGraphSurfaceMode)ctx.reader.ReadInt32();
			rasterizeColliders = ctx.reader.ReadBoolean();
			rasterizeMeshes = ctx.reader.ReadBoolean();
			rasterizeTerrain = ctx.reader.ReadBoolean();
			rasterizeTrees = ctx.reader.ReadBoolean();
			colliderRasterizeDetail = ctx.reader.ReadSingle();
			forcedBoundsCenter = ctx.DeserializeVector3();
			forcedBoundsSize = ctx.DeserializeVector3();
			mask = ctx.reader.ReadInt32();
			int num = ctx.reader.ReadInt32();
			tagMask = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				tagMask.Add(ctx.reader.ReadString());
			}
			showMeshOutline = ctx.reader.ReadBoolean();
			showNodeConnections = ctx.reader.ReadBoolean();
			terrainSampleSize = ctx.reader.ReadInt32();
			walkableClimb = ctx.DeserializeFloat(walkableClimb);
			minRegionSize = ctx.DeserializeFloat(minRegionSize);
			tileSizeZ = ctx.DeserializeInt(tileSizeX);
			showMeshSurface = ctx.reader.ReadBoolean();
		}
	}
}
