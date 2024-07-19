using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Pathfinding;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
[AddComponentMenu("Pathfinding/Pathfinder")]
[HelpURL("http://arongranberg.com/astar/docs/class_astar_path.php")]
public class AstarPath : VersionedMonoBehaviour
{
	public enum AstarDistribution
	{
		WebsiteDownload = 0,
		AssetStore = 1
	}

	public static readonly Version Version = new Version(4, 1, 16);

	public static readonly AstarDistribution Distribution = AstarDistribution.WebsiteDownload;

	public static readonly string Branch = "master_Pro";

	[FormerlySerializedAs("astarData")]
	public AstarData data;

	public static AstarPath active;

	public bool showNavGraphs = true;

	public bool showUnwalkableNodes = true;

	public GraphDebugMode debugMode;

	public float debugFloor;

	public float debugRoof = 20000f;

	public bool manualDebugFloorRoof;

	public bool showSearchTree;

	public float unwalkableNodeDebugSize = 0.3f;

	public PathLog logPathResults = PathLog.Normal;

	public float maxNearestNodeDistance = 100f;

	public bool scanOnStartup = true;

	public bool fullGetNearestSearch;

	public bool prioritizeGraphs;

	public float prioritizeGraphsLimit = 1f;

	public AstarColor colorSettings;

	[SerializeField]
	protected string[] tagNames;

	public Heuristic heuristic = Heuristic.Euclidean;

	public float heuristicScale = 1f;

	public ThreadCount threadCount = ThreadCount.One;

	public float maxFrameTime = 1f;

	[Obsolete("Minimum area size is mostly obsolete since the limit has been raised significantly, and the edge cases are handled automatically")]
	public int minAreaSize;

	public bool batchGraphUpdates;

	public float graphUpdateBatchingInterval = 0.2f;

	[NonSerialized]
	public PathHandler debugPathData;

	[NonSerialized]
	public ushort debugPathID;

	private string inGameDebugPath;

	[NonSerialized]
	private bool isScanningBacking;

	public static Action OnAwakeSettings;

	public static OnGraphDelegate OnGraphPreScan;

	public static OnGraphDelegate OnGraphPostScan;

	public static OnPathDelegate OnPathPreSearch;

	public static OnPathDelegate OnPathPostSearch;

	public static OnScanDelegate OnPreScan;

	public static OnScanDelegate OnPostScan;

	public static OnScanDelegate OnLatePostScan;

	public static OnScanDelegate OnGraphsUpdated;

	public static Action On65KOverflow;

	[Obsolete]
	public Action OnGraphsWillBeUpdated;

	[Obsolete]
	public Action OnGraphsWillBeUpdated2;

	private readonly GraphUpdateProcessor graphUpdates;

	private readonly WorkItemProcessor workItems;

	private PathProcessor pathProcessor;

	private bool graphUpdateRoutineRunning;

	private bool graphUpdatesWorkItemAdded;

	private float lastGraphUpdate = -9999f;

	private PathProcessor.GraphUpdateLock workItemLock;

	internal readonly PathReturnQueue pathReturnQueue;

	public EuclideanEmbedding euclideanEmbedding = new EuclideanEmbedding();

	public bool showGraphs;

	private ushort nextFreePathID = 1;

	private RetainedGizmos gizmos = new RetainedGizmos();

	private static int waitForPathDepth = 0;

	[Obsolete]
	public Type[] graphTypes
	{
		get
		{
			return data.graphTypes;
		}
	}

	[Obsolete("The 'astarData' field has been renamed to 'data'")]
	public AstarData astarData
	{
		get
		{
			return data;
		}
	}

	public NavGraph[] graphs
	{
		get
		{
			if (data == null)
			{
				data = new AstarData();
			}
			return data.graphs;
		}
	}

	public float maxNearestNodeDistanceSqr
	{
		get
		{
			return maxNearestNodeDistance * maxNearestNodeDistance;
		}
	}

	[Obsolete("This field has been renamed to 'batchGraphUpdates'")]
	public bool limitGraphUpdates
	{
		get
		{
			return batchGraphUpdates;
		}
		set
		{
			batchGraphUpdates = value;
		}
	}

	[Obsolete("This field has been renamed to 'graphUpdateBatchingInterval'")]
	public float maxGraphUpdateFreq
	{
		get
		{
			return graphUpdateBatchingInterval;
		}
		set
		{
			graphUpdateBatchingInterval = value;
		}
	}

	public float lastScanTime { get; private set; }

	public bool isScanning
	{
		get
		{
			return isScanningBacking;
		}
		private set
		{
			isScanningBacking = value;
		}
	}

	public int NumParallelThreads
	{
		get
		{
			return pathProcessor.NumThreads;
		}
	}

	public bool IsUsingMultithreading
	{
		get
		{
			return pathProcessor.IsUsingMultithreading;
		}
	}

	[Obsolete("Fixed grammar, use IsAnyGraphUpdateQueued instead")]
	public bool IsAnyGraphUpdatesQueued
	{
		get
		{
			return IsAnyGraphUpdateQueued;
		}
	}

	public bool IsAnyGraphUpdateQueued
	{
		get
		{
			return graphUpdates.IsAnyGraphUpdateQueued;
		}
	}

	public bool IsAnyGraphUpdateInProgress
	{
		get
		{
			return graphUpdates.IsAnyGraphUpdateInProgress;
		}
	}

	public bool IsAnyWorkItemInProgress
	{
		get
		{
			return workItems.workItemsInProgress;
		}
	}

	internal bool IsInsideWorkItem
	{
		get
		{
			return workItems.workItemsInProgressRightNow;
		}
	}

	private AstarPath()
	{
		pathReturnQueue = new PathReturnQueue(this);
		pathProcessor = new PathProcessor(this, pathReturnQueue, 1, false);
		workItems = new WorkItemProcessor(this);
		graphUpdates = new GraphUpdateProcessor(this);
		graphUpdates.OnGraphsUpdated += delegate
		{
			if (OnGraphsUpdated != null)
			{
				OnGraphsUpdated(this);
			}
		};
	}

	public string[] GetTagNames()
	{
		if (tagNames == null || tagNames.Length != 32)
		{
			tagNames = new string[32];
			for (int i = 0; i < tagNames.Length; i++)
			{
				tagNames[i] = string.Empty + i;
			}
			tagNames[0] = "Basic Ground";
		}
		return tagNames;
	}

	public static void FindAstarPath()
	{
		if (!Application.isPlaying)
		{
			if (active == null)
			{
				active = UnityEngine.Object.FindObjectOfType<AstarPath>();
			}
			if (active != null && (active.data.graphs == null || active.data.graphs.Length == 0))
			{
				active.data.DeserializeGraphs();
			}
		}
	}

	public static string[] FindTagNames()
	{
		FindAstarPath();
		return (active != null) ? active.GetTagNames() : new string[1] { "There is no AstarPath component in the scene" };
	}

	internal ushort GetNextPathID()
	{
		if (nextFreePathID == 0)
		{
			nextFreePathID++;
			if (On65KOverflow != null)
			{
				Action on65KOverflow = On65KOverflow;
				On65KOverflow = null;
				on65KOverflow();
			}
		}
		return nextFreePathID++;
	}

	private void RecalculateDebugLimits()
	{
		debugFloor = float.PositiveInfinity;
		debugRoof = float.NegativeInfinity;
		bool ignoreSearchTree = !showSearchTree || debugPathData == null;
		for (int i = 0; i < graphs.Length; i++)
		{
			if (graphs[i] == null || !graphs[i].drawGizmos)
			{
				continue;
			}
			graphs[i].GetNodes(delegate(GraphNode node)
			{
				if (ignoreSearchTree || GraphGizmoHelper.InSearchTree(node, debugPathData, debugPathID))
				{
					if (debugMode == GraphDebugMode.Penalty)
					{
						debugFloor = Mathf.Min(debugFloor, node.Penalty);
						debugRoof = Mathf.Max(debugRoof, node.Penalty);
					}
					else if (debugPathData != null)
					{
						PathNode pathNode = debugPathData.GetPathNode(node);
						switch (debugMode)
						{
						case GraphDebugMode.F:
							debugFloor = Mathf.Min(debugFloor, pathNode.F);
							debugRoof = Mathf.Max(debugRoof, pathNode.F);
							break;
						case GraphDebugMode.G:
							debugFloor = Mathf.Min(debugFloor, pathNode.G);
							debugRoof = Mathf.Max(debugRoof, pathNode.G);
							break;
						case GraphDebugMode.H:
							debugFloor = Mathf.Min(debugFloor, pathNode.H);
							debugRoof = Mathf.Max(debugRoof, pathNode.H);
							break;
						}
					}
				}
			});
		}
		if (float.IsInfinity(debugFloor))
		{
			debugFloor = 0f;
			debugRoof = 1f;
		}
		if (debugRoof - debugFloor < 1f)
		{
			debugRoof += 1f;
		}
	}

	private void OnDrawGizmos()
	{
		if (active == null)
		{
			active = this;
		}
		if (active != this || graphs == null || Event.current.type != EventType.Repaint)
		{
			return;
		}
		if (workItems.workItemsInProgress || isScanning)
		{
			gizmos.DrawExisting();
		}
		else
		{
			if (showNavGraphs && !manualDebugFloorRoof)
			{
				RecalculateDebugLimits();
			}
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null && graphs[i].drawGizmos)
				{
					graphs[i].OnDrawGizmos(gizmos, showNavGraphs);
				}
			}
			if (showNavGraphs)
			{
				euclideanEmbedding.OnDrawGizmos();
			}
		}
		gizmos.FinalizeDraw();
	}

	private void OnGUI()
	{
		if (logPathResults == PathLog.InGame && inGameDebugPath != string.Empty)
		{
			GUI.Label(new Rect(5f, 5f, 400f, 600f), inGameDebugPath);
		}
	}

	private void LogPathResults(Path path)
	{
		if (logPathResults != 0 && (path.error || logPathResults != PathLog.OnlyErrors))
		{
			string message = path.DebugString(logPathResults);
			if (logPathResults == PathLog.InGame)
			{
				inGameDebugPath = message;
			}
			else if (path.error)
			{
				UnityEngine.Debug.LogWarning(message);
			}
			else
			{
				UnityEngine.Debug.Log(message);
			}
		}
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			if (!isScanning)
			{
				PerformBlockingActions();
			}
			pathProcessor.TickNonMultithreaded();
			pathReturnQueue.ReturnPaths(true);
		}
	}

	private void PerformBlockingActions(bool force = false)
	{
		if (workItemLock.Held && pathProcessor.queue.AllReceiversBlocked)
		{
			pathReturnQueue.ReturnPaths(false);
			if (workItems.ProcessWorkItems(force))
			{
				workItemLock.Release();
			}
		}
	}

	[Obsolete("This method has been moved. Use the method on the context object that can be sent with work item delegates instead")]
	public void QueueWorkItemFloodFill()
	{
		throw new Exception("This method has been moved. Use the method on the context object that can be sent with work item delegates instead");
	}

	[Obsolete("This method has been moved. Use the method on the context object that can be sent with work item delegates instead")]
	public void EnsureValidFloodFill()
	{
		throw new Exception("This method has been moved. Use the method on the context object that can be sent with work item delegates instead");
	}

	public void AddWorkItem(Action callback)
	{
		AddWorkItem(new AstarWorkItem(callback));
	}

	public void AddWorkItem(Action<IWorkItemContext> callback)
	{
		AddWorkItem(new AstarWorkItem(callback));
	}

	public void AddWorkItem(AstarWorkItem item)
	{
		workItems.AddWorkItem(item);
		if (!workItemLock.Held)
		{
			workItemLock = PausePathfindingSoon();
		}
	}

	public void QueueGraphUpdates()
	{
		if (!graphUpdatesWorkItemAdded)
		{
			graphUpdatesWorkItemAdded = true;
			AstarWorkItem workItem = graphUpdates.GetWorkItem();
			AddWorkItem(new AstarWorkItem(delegate
			{
				graphUpdatesWorkItemAdded = false;
				lastGraphUpdate = Time.realtimeSinceStartup;
				workItem.init();
			}, workItem.update));
		}
	}

	private IEnumerator DelayedGraphUpdate()
	{
		graphUpdateRoutineRunning = true;
		yield return new WaitForSeconds(graphUpdateBatchingInterval - (Time.realtimeSinceStartup - lastGraphUpdate));
		QueueGraphUpdates();
		graphUpdateRoutineRunning = false;
	}

	public void UpdateGraphs(Bounds bounds, float delay)
	{
		UpdateGraphs(new GraphUpdateObject(bounds), delay);
	}

	public void UpdateGraphs(GraphUpdateObject ob, float delay)
	{
		StartCoroutine(UpdateGraphsInternal(ob, delay));
	}

	private IEnumerator UpdateGraphsInternal(GraphUpdateObject ob, float delay)
	{
		yield return new WaitForSeconds(delay);
		UpdateGraphs(ob);
	}

	public void UpdateGraphs(Bounds bounds)
	{
		UpdateGraphs(new GraphUpdateObject(bounds));
	}

	public void UpdateGraphs(GraphUpdateObject ob)
	{
		graphUpdates.AddToQueue(ob);
		if (batchGraphUpdates && Time.realtimeSinceStartup - lastGraphUpdate < graphUpdateBatchingInterval)
		{
			if (!graphUpdateRoutineRunning)
			{
				StartCoroutine(DelayedGraphUpdate());
			}
		}
		else
		{
			QueueGraphUpdates();
		}
	}

	public void FlushGraphUpdates()
	{
		if (IsAnyGraphUpdateQueued)
		{
			QueueGraphUpdates();
			FlushWorkItems();
		}
	}

	public void FlushWorkItems()
	{
		PathProcessor.GraphUpdateLock graphUpdateLock = PausePathfinding();
		PerformBlockingActions(true);
		graphUpdateLock.Release();
	}

	[Obsolete("Use FlushWorkItems() instead")]
	public void FlushWorkItems(bool unblockOnComplete, bool block)
	{
		PathProcessor.GraphUpdateLock graphUpdateLock = PausePathfinding();
		PerformBlockingActions(block);
		graphUpdateLock.Release();
	}

	[Obsolete("Use FlushWorkItems instead")]
	public void FlushThreadSafeCallbacks()
	{
		FlushWorkItems();
	}

	public static int CalculateThreadCount(ThreadCount count)
	{
		if (count == ThreadCount.AutomaticLowLoad || count == ThreadCount.AutomaticHighLoad)
		{
			int num = Mathf.Max(1, SystemInfo.processorCount);
			int num2 = SystemInfo.systemMemorySize;
			if (num2 <= 0)
			{
				UnityEngine.Debug.LogError("Machine reporting that is has <= 0 bytes of RAM. This is definitely not true, assuming 1 GiB");
				num2 = 1024;
			}
			if (num <= 1)
			{
				return 0;
			}
			if (num2 <= 512)
			{
				return 0;
			}
			if (count == ThreadCount.AutomaticHighLoad)
			{
				if (num2 <= 1024)
				{
					num = Math.Min(num, 2);
				}
			}
			else
			{
				num /= 2;
				num = Mathf.Max(1, num);
				if (num2 <= 1024)
				{
					num = Math.Min(num, 2);
				}
				num = Math.Min(num, 6);
			}
			return num;
		}
		return (int)count;
	}

	protected override void Awake()
	{
		base.Awake();
		active = this;
		if (UnityEngine.Object.FindObjectsOfType(typeof(AstarPath)).Length > 1)
		{
			UnityEngine.Debug.LogError("You should NOT have more than one AstarPath component in the scene at any time.\nThis can cause serious errors since the AstarPath component builds around a singleton pattern.");
		}
		base.useGUILayout = false;
		if (Application.isPlaying)
		{
			if (OnAwakeSettings != null)
			{
				OnAwakeSettings();
			}
			GraphModifier.FindAllModifiers();
			RelevantGraphSurface.FindAllGraphSurfaces();
			InitializePathProcessor();
			InitializeProfiler();
			ConfigureReferencesInternal();
			InitializeAstarData();
			FlushWorkItems();
			euclideanEmbedding.dirty = true;
			if (scanOnStartup && (!data.cacheStartup || data.file_cachedStartup == null))
			{
				Scan();
			}
		}
	}

	private void InitializePathProcessor()
	{
		int num = CalculateThreadCount(threadCount);
		if (!Application.isPlaying)
		{
			num = 0;
		}
		int processors = Mathf.Max(num, 1);
		bool flag = num > 0;
		pathProcessor = new PathProcessor(this, pathReturnQueue, processors, flag);
		pathProcessor.OnPathPreSearch += delegate(Path path)
		{
			OnPathDelegate onPathPreSearch = OnPathPreSearch;
			if (onPathPreSearch != null)
			{
				onPathPreSearch(path);
			}
		};
		pathProcessor.OnPathPostSearch += delegate(Path path)
		{
			LogPathResults(path);
			OnPathDelegate onPathPostSearch = OnPathPostSearch;
			if (onPathPostSearch != null)
			{
				onPathPostSearch(path);
			}
		};
		pathProcessor.OnQueueUnblocked += delegate
		{
			if (euclideanEmbedding.dirty)
			{
				euclideanEmbedding.RecalculateCosts();
			}
		};
		if (flag)
		{
			graphUpdates.EnableMultithreading();
		}
	}

	internal void VerifyIntegrity()
	{
		if (active != this)
		{
			throw new Exception("Singleton pattern broken. Make sure you only have one AstarPath object in the scene");
		}
		if (data == null)
		{
			throw new NullReferenceException("data is null... A* not set up correctly?");
		}
		if (data.graphs == null)
		{
			data.graphs = new NavGraph[0];
			data.UpdateShortcuts();
		}
	}

	public void ConfigureReferencesInternal()
	{
		active = this;
		data = data ?? new AstarData();
		colorSettings = colorSettings ?? new AstarColor();
		colorSettings.OnEnable();
	}

	private void InitializeProfiler()
	{
	}

	private void InitializeAstarData()
	{
		data.FindGraphTypes();
		data.Awake();
		data.UpdateShortcuts();
	}

	private void OnDisable()
	{
		gizmos.ClearCache();
	}

	private void OnDestroy()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (logPathResults == PathLog.Heavy)
		{
			UnityEngine.Debug.Log("+++ AstarPath Component Destroyed - Cleaning Up Pathfinding Data +++");
		}
		if (!(active != this))
		{
			PausePathfinding();
			euclideanEmbedding.dirty = false;
			FlushWorkItems();
			pathProcessor.queue.TerminateReceivers();
			if (logPathResults == PathLog.Heavy)
			{
				UnityEngine.Debug.Log("Processing Possible Work Items");
			}
			graphUpdates.DisableMultithreading();
			pathProcessor.JoinThreads();
			if (logPathResults == PathLog.Heavy)
			{
				UnityEngine.Debug.Log("Returning Paths");
			}
			pathReturnQueue.ReturnPaths(false);
			if (logPathResults == PathLog.Heavy)
			{
				UnityEngine.Debug.Log("Destroying Graphs");
			}
			data.OnDestroy();
			if (logPathResults == PathLog.Heavy)
			{
				UnityEngine.Debug.Log("Cleaning up variables");
			}
			OnAwakeSettings = null;
			OnGraphPreScan = null;
			OnGraphPostScan = null;
			OnPathPreSearch = null;
			OnPathPostSearch = null;
			OnPreScan = null;
			OnPostScan = null;
			OnLatePostScan = null;
			On65KOverflow = null;
			OnGraphsUpdated = null;
			active = null;
		}
	}

	public void FloodFill(GraphNode seed)
	{
		graphUpdates.FloodFill(seed);
	}

	public void FloodFill(GraphNode seed, uint area)
	{
		graphUpdates.FloodFill(seed, area);
	}

	[ContextMenu("Flood Fill Graphs")]
	public void FloodFill()
	{
		graphUpdates.FloodFill();
		workItems.OnFloodFill();
	}

	internal int GetNewNodeIndex()
	{
		return pathProcessor.GetNewNodeIndex();
	}

	internal void InitializeNode(GraphNode node)
	{
		pathProcessor.InitializeNode(node);
	}

	internal void DestroyNode(GraphNode node)
	{
		pathProcessor.DestroyNode(node);
	}

	[Obsolete("Use PausePathfinding instead. Make sure to call Release on the returned lock.", true)]
	public void BlockUntilPathQueueBlocked()
	{
	}

	public PathProcessor.GraphUpdateLock PausePathfinding()
	{
		return pathProcessor.PausePathfinding(true);
	}

	private PathProcessor.GraphUpdateLock PausePathfindingSoon()
	{
		return pathProcessor.PausePathfinding(false);
	}

	public void Scan(NavGraph graphToScan)
	{
		if (graphToScan == null)
		{
			throw new ArgumentNullException();
		}
		Scan(new NavGraph[1] { graphToScan });
	}

	public void Scan(NavGraph[] graphsToScan = null)
	{
		Progress progress = default(Progress);
		foreach (Progress item in ScanAsync(graphsToScan))
		{
			if (!(progress.description != item.description))
			{
			}
		}
	}

	[Obsolete("ScanLoop is now named ScanAsync and is an IEnumerable<Progress>. Use foreach to iterate over the progress insead")]
	public void ScanLoop(OnScanStatus statusCallback)
	{
		foreach (Progress item in ScanAsync())
		{
			statusCallback(item);
		}
	}

	public IEnumerable<Progress> ScanAsync(NavGraph graphToScan)
	{
		if (graphToScan == null)
		{
			throw new ArgumentNullException();
		}
		return ScanAsync(new NavGraph[1] { graphToScan });
	}

	public IEnumerable<Progress> ScanAsync(NavGraph[] graphsToScan = null)
	{
		if (graphsToScan == null)
		{
			graphsToScan = graphs;
		}
		if (graphsToScan == null)
		{
			yield break;
		}
		if (isScanning)
		{
			throw new InvalidOperationException("Another async scan is already running");
		}
		isScanning = true;
		VerifyIntegrity();
		PathProcessor.GraphUpdateLock graphUpdateLock = PausePathfinding();
		pathReturnQueue.ReturnPaths(false);
		if (!Application.isPlaying)
		{
			data.FindGraphTypes();
			GraphModifier.FindAllModifiers();
		}
		yield return new Progress(0.05f, "Pre processing graphs");
		if (OnPreScan != null)
		{
			OnPreScan(this);
		}
		GraphModifier.TriggerEvent(GraphModifier.EventType.PreScan);
		data.LockGraphStructure();
		Stopwatch watch = Stopwatch.StartNew();
		for (int j = 0; j < graphsToScan.Length; j++)
		{
			if (graphsToScan[j] != null)
			{
				((IGraphInternals)graphsToScan[j]).DestroyAllNodes();
			}
		}
		for (int i = 0; i < graphsToScan.Length; i++)
		{
			if (graphsToScan[i] == null)
			{
				continue;
			}
			float minp = Mathf.Lerp(0.1f, 0.8f, (float)i / (float)graphsToScan.Length);
			float maxp = Mathf.Lerp(0.1f, 0.8f, ((float)i + 0.95f) / (float)graphsToScan.Length);
			string progressDescriptionPrefix = "Scanning graph " + (i + 1) + " of " + graphsToScan.Length + " - ";
			IEnumerator<Progress> coroutine = ScanGraph(graphsToScan[i]).GetEnumerator();
			while (true)
			{
				try
				{
					if (!coroutine.MoveNext())
					{
						break;
					}
				}
				catch
				{
					isScanning = false;
					data.UnlockGraphStructure();
					graphUpdateLock.Release();
					throw;
				}
				yield return coroutine.Current.MapTo(minp, maxp, progressDescriptionPrefix);
			}
		}
		data.UnlockGraphStructure();
		yield return new Progress(0.8f, "Post processing graphs");
		if (OnPostScan != null)
		{
			OnPostScan(this);
		}
		GraphModifier.TriggerEvent(GraphModifier.EventType.PostScan);
		FlushWorkItems();
		yield return new Progress(0.9f, "Computing areas");
		FloodFill();
		yield return new Progress(0.95f, "Late post processing");
		isScanning = false;
		if (OnLatePostScan != null)
		{
			OnLatePostScan(this);
		}
		GraphModifier.TriggerEvent(GraphModifier.EventType.LatePostScan);
		euclideanEmbedding.dirty = true;
		euclideanEmbedding.RecalculatePivots();
		FlushWorkItems();
		graphUpdateLock.Release();
		watch.Stop();
		lastScanTime = (float)watch.Elapsed.TotalSeconds;
		GC.Collect();
		if (logPathResults != 0 && logPathResults != PathLog.OnlyErrors)
		{
			UnityEngine.Debug.Log("Scanning - Process took " + (lastScanTime * 1000f).ToString("0") + " ms to complete");
		}
	}

	private IEnumerable<Progress> ScanGraph(NavGraph graph)
	{
		if (OnGraphPreScan != null)
		{
			yield return new Progress(0f, "Pre processing");
			OnGraphPreScan(graph);
		}
		yield return new Progress(0f, string.Empty);
		foreach (Progress item in ((IGraphInternals)graph).ScanInternal())
		{
			yield return item.MapTo(0f, 0.95f);
		}
		yield return new Progress(0.95f, "Assigning graph indices");
		graph.GetNodes(delegate(GraphNode node)
		{
			node.GraphIndex = graph.graphIndex;
		});
		if (OnGraphPostScan != null)
		{
			yield return new Progress(0.99f, "Post processing");
			OnGraphPostScan(graph);
		}
	}

	[Obsolete("This method has been renamed to BlockUntilCalculated")]
	public static void WaitForPath(Path path)
	{
		BlockUntilCalculated(path);
	}

	public static void BlockUntilCalculated(Path path)
	{
		if (active == null)
		{
			throw new Exception("Pathfinding is not correctly initialized in this scene (yet?). AstarPath.active is null.\nDo not call this function in Awake");
		}
		if (path == null)
		{
			throw new ArgumentNullException("Path must not be null");
		}
		if (active.pathProcessor.queue.IsTerminating)
		{
			return;
		}
		if (path.PipelineState == PathState.Created)
		{
			throw new Exception("The specified path has not been started yet.");
		}
		waitForPathDepth++;
		if (waitForPathDepth == 5)
		{
			UnityEngine.Debug.LogError("You are calling the BlockUntilCalculated function recursively (maybe from a path callback). Please don't do this.");
		}
		if (path.PipelineState < PathState.ReturnQueue)
		{
			if (active.IsUsingMultithreading)
			{
				while (path.PipelineState < PathState.ReturnQueue)
				{
					if (active.pathProcessor.queue.IsTerminating)
					{
						waitForPathDepth--;
						throw new Exception("Pathfinding Threads seem to have crashed.");
					}
					Thread.Sleep(1);
					active.PerformBlockingActions(true);
				}
			}
			else
			{
				while (path.PipelineState < PathState.ReturnQueue)
				{
					if (active.pathProcessor.queue.IsEmpty && path.PipelineState != PathState.Processing)
					{
						waitForPathDepth--;
						throw new Exception(string.Concat("Critical error. Path Queue is empty but the path state is '", path.PipelineState, "'"));
					}
					active.pathProcessor.TickNonMultithreaded();
					active.PerformBlockingActions(true);
				}
			}
		}
		active.pathReturnQueue.ReturnPaths(false);
		waitForPathDepth--;
	}

	[Obsolete("The threadSafe parameter has been deprecated")]
	public static void RegisterSafeUpdate(Action callback, bool threadSafe)
	{
		RegisterSafeUpdate(callback);
	}

	[Obsolete("Use AddWorkItem(System.Action) instead. Note the slight change in behavior (mentioned in the documentation).")]
	public static void RegisterSafeUpdate(Action callback)
	{
		active.AddWorkItem(new AstarWorkItem(callback));
	}

	public static void StartPath(Path path, bool pushToFront = false)
	{
		AstarPath astarPath = active;
		if (object.ReferenceEquals(astarPath, null))
		{
			UnityEngine.Debug.LogError("There is no AstarPath object in the scene or it has not been initialized yet");
			return;
		}
		if (path.PipelineState != 0)
		{
			throw new Exception(string.Concat("The path has an invalid state. Expected ", PathState.Created, " found ", path.PipelineState, "\nMake sure you are not requesting the same path twice"));
		}
		if (astarPath.pathProcessor.queue.IsTerminating)
		{
			path.FailWithError("No new paths are accepted");
			return;
		}
		if (astarPath.graphs == null || astarPath.graphs.Length == 0)
		{
			UnityEngine.Debug.LogError("There are no graphs in the scene");
			path.FailWithError("There are no graphs in the scene");
			UnityEngine.Debug.LogError(path.errorLog);
			return;
		}
		path.Claim(astarPath);
		((IPathInternals)path).AdvanceState(PathState.PathQueue);
		if (pushToFront)
		{
			astarPath.pathProcessor.queue.PushFront(path);
		}
		else
		{
			astarPath.pathProcessor.queue.Push(path);
		}
		if (!Application.isPlaying)
		{
			BlockUntilCalculated(path);
		}
	}

	private void OnApplicationQuit()
	{
		OnDestroy();
		pathProcessor.AbortThreads();
	}

	public NNInfo GetNearest(Vector3 position)
	{
		return GetNearest(position, NNConstraint.None);
	}

	public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
	{
		return GetNearest(position, constraint, null);
	}

	public NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
	{
		NavGraph[] array = graphs;
		float num = float.PositiveInfinity;
		NNInfoInternal internalInfo = default(NNInfoInternal);
		int num2 = -1;
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				NavGraph navGraph = array[i];
				if (navGraph == null || !constraint.SuitableGraph(i, navGraph))
				{
					continue;
				}
				NNInfoInternal nNInfoInternal = ((!fullGetNearestSearch) ? navGraph.GetNearest(position, constraint) : navGraph.GetNearestForce(position, constraint));
				GraphNode node = nNInfoInternal.node;
				if (node != null)
				{
					float magnitude = (nNInfoInternal.clampedPosition - position).magnitude;
					if (prioritizeGraphs && magnitude < prioritizeGraphsLimit)
					{
						num = magnitude;
						internalInfo = nNInfoInternal;
						num2 = i;
						break;
					}
					if (magnitude < num)
					{
						num = magnitude;
						internalInfo = nNInfoInternal;
						num2 = i;
					}
				}
			}
		}
		if (num2 == -1)
		{
			return default(NNInfo);
		}
		if (internalInfo.constrainedNode != null)
		{
			internalInfo.node = internalInfo.constrainedNode;
			internalInfo.clampedPosition = internalInfo.constClampedPosition;
		}
		if (!fullGetNearestSearch && internalInfo.node != null && !constraint.Suitable(internalInfo.node))
		{
			NNInfoInternal nearestForce = array[num2].GetNearestForce(position, constraint);
			if (nearestForce.node != null)
			{
				internalInfo = nearestForce;
			}
		}
		if (!constraint.Suitable(internalInfo.node) || (constraint.constrainDistance && (internalInfo.clampedPosition - position).sqrMagnitude > maxNearestNodeDistanceSqr))
		{
			return default(NNInfo);
		}
		return new NNInfo(internalInfo);
	}

	public GraphNode GetNearest(Ray ray)
	{
		if (graphs == null)
		{
			return null;
		}
		float minDist = float.PositiveInfinity;
		GraphNode nearestNode = null;
		Vector3 lineDirection = ray.direction;
		Vector3 lineOrigin = ray.origin;
		for (int i = 0; i < graphs.Length; i++)
		{
			NavGraph navGraph = graphs[i];
			navGraph.GetNodes(delegate(GraphNode node)
			{
				Vector3 vector = (Vector3)node.position;
				Vector3 vector2 = lineOrigin + Vector3.Dot(vector - lineOrigin, lineDirection) * lineDirection;
				float num = Mathf.Abs(vector2.x - vector.x);
				num *= num;
				if (!(num > minDist))
				{
					num = Mathf.Abs(vector2.z - vector.z);
					num *= num;
					if (!(num > minDist))
					{
						float sqrMagnitude = (vector2 - vector).sqrMagnitude;
						if (sqrMagnitude < minDist)
						{
							minDist = sqrMagnitude;
							nearestNode = node;
						}
					}
				}
			});
		}
		return nearestNode;
	}
}
