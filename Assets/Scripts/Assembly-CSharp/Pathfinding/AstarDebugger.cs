using System;
using System.Text;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/Pathfinding Debugger")]
	[ExecuteInEditMode]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_astar_debugger.php")]
	public class AstarDebugger : VersionedMonoBehaviour
	{
		private struct GraphPoint
		{
			public float fps;

			public float memory;

			public bool collectEvent;
		}

		private struct PathTypeDebug
		{
			private string name;

			private Func<int> getSize;

			private Func<int> getTotalCreated;

			public PathTypeDebug(string name, Func<int> getSize, Func<int> getTotalCreated)
			{
				this.name = name;
				this.getSize = getSize;
				this.getTotalCreated = getTotalCreated;
			}

			public void Print(StringBuilder text)
			{
				int num = getTotalCreated();
				if (num > 0)
				{
					text.Append("\n").Append(("  " + name).PadRight(25)).Append(getSize())
						.Append("/")
						.Append(num);
				}
			}
		}

		public int yOffset = 5;

		public bool show = true;

		public bool showInEditor;

		public bool showFPS;

		public bool showPathProfile;

		public bool showMemProfile;

		public bool showGraph;

		public int graphBufferSize = 200;

		public Font font;

		public int fontSize = 12;

		private StringBuilder text = new StringBuilder();

		private string cachedText;

		private float lastUpdate = -999f;

		private GraphPoint[] graph;

		private float delayedDeltaTime = 1f;

		private float lastCollect;

		private float lastCollectNum;

		private float delta;

		private float lastDeltaTime;

		private int allocRate;

		private int lastAllocMemory;

		private float lastAllocSet = -9999f;

		private int allocMem;

		private int collectAlloc;

		private int peakAlloc;

		private int fpsDropCounterSize = 200;

		private float[] fpsDrops;

		private Rect boxRect;

		private GUIStyle style;

		private Camera cam;

		private float graphWidth = 100f;

		private float graphHeight = 100f;

		private float graphOffset = 50f;

		private int maxVecPool;

		private int maxNodePool;

		private PathTypeDebug[] debugTypes = new PathTypeDebug[7]
		{
			new PathTypeDebug("ABPath", () => PathPool.GetSize(typeof(ABPath)), () => PathPool.GetTotalCreated(typeof(ABPath))),
			new PathTypeDebug("MultiTargetPath", () => PathPool.GetSize(typeof(MultiTargetPath)), () => PathPool.GetTotalCreated(typeof(MultiTargetPath))),
			new PathTypeDebug("RandomPath", () => PathPool.GetSize(typeof(RandomPath)), () => PathPool.GetTotalCreated(typeof(RandomPath))),
			new PathTypeDebug("FleePath", () => PathPool.GetSize(typeof(FleePath)), () => PathPool.GetTotalCreated(typeof(FleePath))),
			new PathTypeDebug("ConstantPath", () => PathPool.GetSize(typeof(ConstantPath)), () => PathPool.GetTotalCreated(typeof(ConstantPath))),
			new PathTypeDebug("FloodPath", () => PathPool.GetSize(typeof(FloodPath)), () => PathPool.GetTotalCreated(typeof(FloodPath))),
			new PathTypeDebug("FloodPathTracer", () => PathPool.GetSize(typeof(FloodPathTracer)), () => PathPool.GetTotalCreated(typeof(FloodPathTracer)))
		};

		public void Start()
		{
			base.useGUILayout = false;
			fpsDrops = new float[fpsDropCounterSize];
			cam = GetComponent<Camera>();
			if (cam == null)
			{
				cam = Camera.main;
			}
			graph = new GraphPoint[graphBufferSize];
			if (Time.unscaledDeltaTime > 0f)
			{
				for (int i = 0; i < fpsDrops.Length; i++)
				{
					fpsDrops[i] = 1f / Time.unscaledDeltaTime;
				}
			}
		}

		public void LateUpdate()
		{
			if (!show || (!Application.isPlaying && !showInEditor) || Time.unscaledDeltaTime <= 0.0001f)
			{
				return;
			}
			int num = GC.CollectionCount(0);
			if (lastCollectNum != (float)num)
			{
				lastCollectNum = num;
				delta = Time.realtimeSinceStartup - lastCollect;
				lastCollect = Time.realtimeSinceStartup;
				lastDeltaTime = Time.unscaledDeltaTime;
				collectAlloc = allocMem;
			}
			allocMem = (int)GC.GetTotalMemory(false);
			bool flag = allocMem < peakAlloc;
			peakAlloc = (flag ? peakAlloc : allocMem);
			if (Time.realtimeSinceStartup - lastAllocSet > 0.3f || !Application.isPlaying)
			{
				int num2 = allocMem - lastAllocMemory;
				lastAllocMemory = allocMem;
				lastAllocSet = Time.realtimeSinceStartup;
				delayedDeltaTime = Time.unscaledDeltaTime;
				if (num2 >= 0)
				{
					allocRate = num2;
				}
			}
			if (Application.isPlaying)
			{
				fpsDrops[Time.frameCount % fpsDrops.Length] = ((!(Time.unscaledDeltaTime > 1E-05f)) ? 0f : (1f / Time.unscaledDeltaTime));
				int num3 = Time.frameCount % graph.Length;
				graph[num3].fps = ((!(Time.unscaledDeltaTime < 1E-05f)) ? 0f : (1f / Time.unscaledDeltaTime));
				graph[num3].collectEvent = flag;
				graph[num3].memory = allocMem;
			}
			if (!Application.isPlaying || !(cam != null) || !showGraph)
			{
				return;
			}
			graphWidth = (float)cam.pixelWidth * 0.8f;
			float num4 = float.PositiveInfinity;
			float b = 0f;
			float num5 = float.PositiveInfinity;
			float b2 = 0f;
			for (int i = 0; i < graph.Length; i++)
			{
				num4 = Mathf.Min(graph[i].memory, num4);
				b = Mathf.Max(graph[i].memory, b);
				num5 = Mathf.Min(graph[i].fps, num5);
				b2 = Mathf.Max(graph[i].fps, b2);
			}
			int num6 = Time.frameCount % graph.Length;
			Matrix4x4 m = Matrix4x4.TRS(new Vector3(((float)cam.pixelWidth - graphWidth) / 2f, graphOffset, 1f), Quaternion.identity, new Vector3(graphWidth, graphHeight, 1f));
			for (int j = 0; j < graph.Length - 1; j++)
			{
				if (j != num6)
				{
					DrawGraphLine(j, m, (float)j / (float)graph.Length, (float)(j + 1) / (float)graph.Length, Mathf.InverseLerp(num4, b, graph[j].memory), Mathf.InverseLerp(num4, b, graph[j + 1].memory), Color.blue);
					DrawGraphLine(j, m, (float)j / (float)graph.Length, (float)(j + 1) / (float)graph.Length, Mathf.InverseLerp(num5, b2, graph[j].fps), Mathf.InverseLerp(num5, b2, graph[j + 1].fps), Color.green);
				}
			}
		}

		private void DrawGraphLine(int index, Matrix4x4 m, float x1, float x2, float y1, float y2, Color color)
		{
			Debug.DrawLine(cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new Vector3(x1, y1))), cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new Vector3(x2, y2))), color);
		}

		public void OnGUI()
		{
			if (!show || (!Application.isPlaying && !showInEditor))
			{
				return;
			}
			if (style == null)
			{
				style = new GUIStyle();
				style.normal.textColor = Color.white;
				style.padding = new RectOffset(5, 5, 5, 5);
			}
			if (Time.realtimeSinceStartup - lastUpdate > 0.5f || cachedText == null || !Application.isPlaying)
			{
				lastUpdate = Time.realtimeSinceStartup;
				boxRect = new Rect(5f, yOffset, 310f, 40f);
				text.Length = 0;
				text.AppendLine("A* Pathfinding Project Debugger");
				text.Append("A* Version: ").Append(AstarPath.Version.ToString());
				if (showMemProfile)
				{
					boxRect.height += 200f;
					text.AppendLine();
					text.AppendLine();
					text.Append("Currently allocated".PadRight(25));
					text.Append(((float)allocMem / 1000000f).ToString("0.0 MB"));
					text.AppendLine();
					text.Append("Peak allocated".PadRight(25));
					text.Append(((float)peakAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
					text.Append("Last collect peak".PadRight(25));
					text.Append(((float)collectAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
					text.Append("Allocation rate".PadRight(25));
					text.Append(((float)allocRate / 1000000f).ToString("0.0 MB")).AppendLine();
					text.Append("Collection frequency".PadRight(25));
					text.Append(delta.ToString("0.00"));
					text.Append("s\n");
					text.Append("Last collect fps".PadRight(25));
					text.Append((1f / lastDeltaTime).ToString("0.0 fps"));
					text.Append(" (");
					text.Append(lastDeltaTime.ToString("0.000 s"));
					text.Append(")");
				}
				if (showFPS)
				{
					text.AppendLine();
					text.AppendLine();
					float num = ((!(delayedDeltaTime > 1E-05f)) ? 0f : (1f / delayedDeltaTime));
					text.Append("FPS".PadRight(25)).Append(num.ToString("0.0 fps"));
					float num2 = float.PositiveInfinity;
					for (int i = 0; i < fpsDrops.Length; i++)
					{
						if (fpsDrops[i] < num2)
						{
							num2 = fpsDrops[i];
						}
					}
					text.AppendLine();
					text.Append(("Lowest fps (last " + fpsDrops.Length + ")").PadRight(25)).Append(num2.ToString("0.0"));
				}
				if (showPathProfile)
				{
					AstarPath active = AstarPath.active;
					text.AppendLine();
					if (active == null)
					{
						text.Append("\nNo AstarPath Object In The Scene");
					}
					else
					{
						if (ListPool<Vector3>.GetSize() > maxVecPool)
						{
							maxVecPool = ListPool<Vector3>.GetSize();
						}
						if (ListPool<GraphNode>.GetSize() > maxNodePool)
						{
							maxNodePool = ListPool<GraphNode>.GetSize();
						}
						text.Append("\nPool Sizes (size/total created)");
						for (int j = 0; j < debugTypes.Length; j++)
						{
							debugTypes[j].Print(text);
						}
					}
				}
				cachedText = text.ToString();
			}
			if (font != null)
			{
				style.font = font;
				style.fontSize = fontSize;
			}
			boxRect.height = style.CalcHeight(new GUIContent(cachedText), boxRect.width);
			GUI.Box(boxRect, string.Empty);
			GUI.Label(boxRect, cachedText, style);
			if (showGraph)
			{
				float num3 = float.PositiveInfinity;
				float num4 = 0f;
				float num5 = float.PositiveInfinity;
				float num6 = 0f;
				for (int k = 0; k < graph.Length; k++)
				{
					num3 = Mathf.Min(graph[k].memory, num3);
					num4 = Mathf.Max(graph[k].memory, num4);
					num5 = Mathf.Min(graph[k].fps, num5);
					num6 = Mathf.Max(graph[k].fps, num6);
				}
				GUI.color = Color.blue;
				float num7 = Mathf.RoundToInt(num4 / 100000f);
				GUI.Label(new Rect(5f, (float)Screen.height - AstarMath.MapTo(num3, num4, graphOffset, graphHeight + graphOffset, num7 * 1000f * 100f) - 10f, 100f, 20f), (num7 / 10f).ToString("0.0 MB"));
				num7 = Mathf.Round(num3 / 100000f);
				GUI.Label(new Rect(5f, (float)Screen.height - AstarMath.MapTo(num3, num4, graphOffset, graphHeight + graphOffset, num7 * 1000f * 100f) - 10f, 100f, 20f), (num7 / 10f).ToString("0.0 MB"));
				GUI.color = Color.green;
				num7 = Mathf.Round(num6);
				GUI.Label(new Rect(55f, (float)Screen.height - AstarMath.MapTo(num5, num6, graphOffset, graphHeight + graphOffset, num7) - 10f, 100f, 20f), num7.ToString("0 FPS"));
				num7 = Mathf.Round(num5);
				GUI.Label(new Rect(55f, (float)Screen.height - AstarMath.MapTo(num5, num6, graphOffset, graphHeight + graphOffset, num7) - 10f, 100f, 20f), num7.ToString("0 FPS"));
			}
		}
	}
}
