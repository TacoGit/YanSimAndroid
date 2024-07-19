using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_path_types_demo.php")]
	public class PathTypesDemo : MonoBehaviour
	{
		public enum DemoMode
		{
			ABPath = 0,
			MultiTargetPath = 1,
			RandomPath = 2,
			FleePath = 3,
			ConstantPath = 4,
			FloodPath = 5,
			FloodPathTracer = 6
		}

		public DemoMode activeDemo;

		public Transform start;

		public Transform end;

		public Vector3 pathOffset;

		public Material lineMat;

		public Material squareMat;

		public float lineWidth;

		public int searchLength = 1000;

		public int spread = 100;

		public float aimStrength;

		private Path lastPath;

		private FloodPath lastFloodPath;

		private List<GameObject> lastRender = new List<GameObject>();

		private List<Vector3> multipoints = new List<Vector3>();

		private void Update()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 vector = ray.origin + ray.direction * (ray.origin.y / (0f - ray.direction.y));
			end.position = vector;
			if (Input.GetMouseButtonUp(0))
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{
					multipoints.Add(vector);
				}
				if (Input.GetKey(KeyCode.LeftControl))
				{
					multipoints.Clear();
				}
				if (Input.mousePosition.x > 225f)
				{
					DemoPath();
				}
			}
			if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt) && (lastPath == null || lastPath.IsDone()))
			{
				DemoPath();
			}
		}

		public void OnGUI()
		{
			GUILayout.BeginArea(new Rect(5f, 5f, 220f, Screen.height - 10), string.Empty, "Box");
			switch (activeDemo)
			{
			case DemoMode.ABPath:
				GUILayout.Label("Basic path. Finds a path from point A to point B.");
				break;
			case DemoMode.MultiTargetPath:
				GUILayout.Label("Multi Target Path. Finds a path quickly from one point to many others in a single search.");
				break;
			case DemoMode.RandomPath:
				GUILayout.Label("Randomized Path. Finds a path with a specified length in a random direction or biased towards some point when using a larger aim strenggth.");
				break;
			case DemoMode.FleePath:
				GUILayout.Label("Flee Path. Tries to flee from a specified point. Remember to set Flee Strength!");
				break;
			case DemoMode.ConstantPath:
				GUILayout.Label("Finds all nodes which it costs less than some value to reach.");
				break;
			case DemoMode.FloodPath:
				GUILayout.Label("Searches the whole graph from a specific point. FloodPathTracer can then be used to quickly find a path to that point");
				break;
			case DemoMode.FloodPathTracer:
				GUILayout.Label("Traces a path to where the FloodPath started. Compare the claculation times for this path with ABPath!\nGreat for TD games");
				break;
			}
			GUILayout.Space(5f);
			GUILayout.Label("Note that the paths are rendered without ANY post-processing applied, so they might look a bit edgy");
			GUILayout.Space(5f);
			GUILayout.Label("Click anywhere to recalculate the path. Hold Alt to continuously recalculate the path while the mouse is pressed.");
			if (activeDemo == DemoMode.ConstantPath || activeDemo == DemoMode.RandomPath || activeDemo == DemoMode.FleePath)
			{
				GUILayout.Label("Search Distance (" + searchLength + ")");
				searchLength = Mathf.RoundToInt(GUILayout.HorizontalSlider(searchLength, 0f, 100000f));
			}
			if (activeDemo == DemoMode.RandomPath || activeDemo == DemoMode.FleePath)
			{
				GUILayout.Label("Spread (" + spread + ")");
				spread = Mathf.RoundToInt(GUILayout.HorizontalSlider(spread, 0f, 40000f));
				GUILayout.Label(((activeDemo != DemoMode.RandomPath) ? "Flee strength" : "Aim strength") + " (" + aimStrength + ")");
				aimStrength = GUILayout.HorizontalSlider(aimStrength, 0f, 1f);
			}
			if (activeDemo == DemoMode.MultiTargetPath)
			{
				GUILayout.Label("Hold shift and click to add new target points. Hold ctr and click to remove all target points");
			}
			if (GUILayout.Button("A to B path"))
			{
				activeDemo = DemoMode.ABPath;
			}
			if (GUILayout.Button("Multi Target Path"))
			{
				activeDemo = DemoMode.MultiTargetPath;
			}
			if (GUILayout.Button("Random Path"))
			{
				activeDemo = DemoMode.RandomPath;
			}
			if (GUILayout.Button("Flee path"))
			{
				activeDemo = DemoMode.FleePath;
			}
			if (GUILayout.Button("Constant Path"))
			{
				activeDemo = DemoMode.ConstantPath;
			}
			if (GUILayout.Button("Flood Path"))
			{
				activeDemo = DemoMode.FloodPath;
			}
			if (GUILayout.Button("Flood Path Tracer"))
			{
				activeDemo = DemoMode.FloodPathTracer;
			}
			GUILayout.EndArea();
		}

		public void OnPathComplete(Path p)
		{
			if (lastRender == null)
			{
				return;
			}
			ClearPrevious();
			if (!p.error)
			{
				GameObject gameObject = new GameObject("LineRenderer", typeof(LineRenderer));
				LineRenderer component = gameObject.GetComponent<LineRenderer>();
				component.sharedMaterial = lineMat;
				component.startWidth = lineWidth;
				component.endWidth = lineWidth;
				component.positionCount = p.vectorPath.Count;
				for (int i = 0; i < p.vectorPath.Count; i++)
				{
					component.SetPosition(i, p.vectorPath[i] + pathOffset);
				}
				lastRender.Add(gameObject);
			}
		}

		private void ClearPrevious()
		{
			for (int i = 0; i < lastRender.Count; i++)
			{
				Object.Destroy(lastRender[i]);
			}
			lastRender.Clear();
		}

		private void OnDestroy()
		{
			ClearPrevious();
			lastRender = null;
		}

		private void DemoPath()
		{
			Path path = null;
			switch (activeDemo)
			{
			case DemoMode.ABPath:
				path = ABPath.Construct(start.position, end.position, OnPathComplete);
				break;
			case DemoMode.MultiTargetPath:
				StartCoroutine(DemoMultiTargetPath());
				break;
			case DemoMode.ConstantPath:
				StartCoroutine(DemoConstantPath());
				break;
			case DemoMode.RandomPath:
			{
				RandomPath randomPath = RandomPath.Construct(start.position, searchLength, OnPathComplete);
				randomPath.spread = spread;
				randomPath.aimStrength = aimStrength;
				randomPath.aim = end.position;
				path = randomPath;
				break;
			}
			case DemoMode.FleePath:
			{
				FleePath fleePath = FleePath.Construct(start.position, end.position, searchLength, OnPathComplete);
				fleePath.aimStrength = aimStrength;
				fleePath.spread = spread;
				path = fleePath;
				break;
			}
			case DemoMode.FloodPath:
				path = (lastFloodPath = FloodPath.Construct(end.position));
				break;
			case DemoMode.FloodPathTracer:
				if (lastFloodPath != null)
				{
					FloodPathTracer floodPathTracer = FloodPathTracer.Construct(end.position, lastFloodPath, OnPathComplete);
					path = floodPathTracer;
				}
				break;
			}
			if (path != null)
			{
				AstarPath.StartPath(path);
				lastPath = path;
			}
		}

		private IEnumerator DemoMultiTargetPath()
		{
			MultiTargetPath mp = (MultiTargetPath)(lastPath = MultiTargetPath.Construct(multipoints.ToArray(), end.position, null));
			AstarPath.StartPath(mp);
			yield return StartCoroutine(mp.WaitForPath());
			List<GameObject> unused = new List<GameObject>(lastRender);
			lastRender.Clear();
			for (int i = 0; i < mp.vectorPaths.Length; i++)
			{
				if (mp.vectorPaths[i] != null)
				{
					List<Vector3> list = mp.vectorPaths[i];
					GameObject gameObject;
					if (unused.Count > i && unused[i].GetComponent<LineRenderer>() != null)
					{
						gameObject = unused[i];
						unused.RemoveAt(i);
					}
					else
					{
						gameObject = new GameObject("LineRenderer_" + i, typeof(LineRenderer));
					}
					LineRenderer component = gameObject.GetComponent<LineRenderer>();
					component.sharedMaterial = lineMat;
					component.startWidth = lineWidth;
					component.endWidth = lineWidth;
					component.positionCount = list.Count;
					for (int j = 0; j < list.Count; j++)
					{
						component.SetPosition(j, list[j] + pathOffset);
					}
					lastRender.Add(gameObject);
				}
			}
			for (int k = 0; k < unused.Count; k++)
			{
				Object.Destroy(unused[k]);
			}
		}

		public IEnumerator DemoConstantPath()
		{
			ConstantPath constPath = ConstantPath.Construct(end.position, searchLength);
			AstarPath.StartPath(constPath);
			lastPath = constPath;
			yield return StartCoroutine(constPath.WaitForPath());
			ClearPrevious();
			List<GraphNode> nodes = constPath.allNodes;
			Mesh mesh = new Mesh();
			List<Vector3> verts = new List<Vector3>();
			bool drawRaysInstead = false;
			for (int num = nodes.Count - 1; num >= 0; num--)
			{
				Vector3 vector = (Vector3)nodes[num].position + pathOffset;
				if (verts.Count == 65000 && !drawRaysInstead)
				{
					Debug.LogError("Too many nodes, rendering a mesh would throw 65K vertex error. Using Debug.DrawRay instead for the rest of the nodes");
					drawRaysInstead = true;
				}
				if (drawRaysInstead)
				{
					Debug.DrawRay(vector, Vector3.up, Color.blue);
				}
				else
				{
					GridGraph gridGraph = AstarData.GetGraph(nodes[num]) as GridGraph;
					float num2 = 1f;
					if (gridGraph != null)
					{
						num2 = gridGraph.nodeSize;
					}
					verts.Add(vector + new Vector3(-0.5f, 0f, -0.5f) * num2);
					verts.Add(vector + new Vector3(0.5f, 0f, -0.5f) * num2);
					verts.Add(vector + new Vector3(-0.5f, 0f, 0.5f) * num2);
					verts.Add(vector + new Vector3(0.5f, 0f, 0.5f) * num2);
				}
			}
			Vector3[] vs = verts.ToArray();
			int[] tris = new int[3 * vs.Length / 2];
			int i = 0;
			int num3 = 0;
			for (; i < vs.Length; i += 4)
			{
				tris[num3] = i;
				tris[num3 + 1] = i + 1;
				tris[num3 + 2] = i + 2;
				tris[num3 + 3] = i + 1;
				tris[num3 + 4] = i + 3;
				tris[num3 + 5] = i + 2;
				num3 += 6;
			}
			Vector2[] uv = new Vector2[vs.Length];
			for (int j = 0; j < uv.Length; j += 4)
			{
				uv[j] = new Vector2(0f, 0f);
				uv[j + 1] = new Vector2(1f, 0f);
				uv[j + 2] = new Vector2(0f, 1f);
				uv[j + 3] = new Vector2(1f, 1f);
			}
			mesh.vertices = vs;
			mesh.triangles = tris;
			mesh.uv = uv;
			mesh.RecalculateNormals();
			GameObject go = new GameObject("Mesh", typeof(MeshRenderer), typeof(MeshFilter));
			MeshFilter fi = go.GetComponent<MeshFilter>();
			fi.mesh = mesh;
			MeshRenderer re = go.GetComponent<MeshRenderer>();
			re.material = squareMat;
			lastRender.Add(go);
		}
	}
}
