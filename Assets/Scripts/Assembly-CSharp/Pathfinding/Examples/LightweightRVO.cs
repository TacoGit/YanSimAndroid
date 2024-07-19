using System;
using System.Collections.Generic;
using Pathfinding.RVO;
using UnityEngine;

namespace Pathfinding.Examples
{
	[RequireComponent(typeof(MeshFilter))]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_lightweight_r_v_o.php")]
	public class LightweightRVO : MonoBehaviour
	{
		public enum RVOExampleType
		{
			Circle = 0,
			Line = 1,
			Point = 2,
			RandomStreams = 3,
			Crossing = 4
		}

		public int agentCount = 100;

		public float exampleScale = 100f;

		public RVOExampleType type;

		public float radius = 3f;

		public float maxSpeed = 2f;

		public float agentTimeHorizon = 10f;

		[HideInInspector]
		public float obstacleTimeHorizon = 10f;

		public int maxNeighbours = 10;

		public Vector3 renderingOffset = Vector3.up * 0.1f;

		public bool debug;

		private Mesh mesh;

		private Simulator sim;

		private List<IAgent> agents;

		private List<Vector3> goals;

		private List<Color> colors;

		private Vector3[] verts;

		private Vector2[] uv;

		private int[] tris;

		private Color[] meshColors;

		private Vector2[] interpolatedVelocities;

		private Vector2[] interpolatedRotations;

		public void Start()
		{
			mesh = new Mesh();
			RVOSimulator rVOSimulator = UnityEngine.Object.FindObjectOfType(typeof(RVOSimulator)) as RVOSimulator;
			if (rVOSimulator == null)
			{
				Debug.LogError("No RVOSimulator could be found in the scene. Please add a RVOSimulator component to any GameObject");
				return;
			}
			sim = rVOSimulator.GetSimulator();
			GetComponent<MeshFilter>().mesh = mesh;
			CreateAgents(agentCount);
		}

		public void OnGUI()
		{
			if (GUILayout.Button("2"))
			{
				CreateAgents(2);
			}
			if (GUILayout.Button("10"))
			{
				CreateAgents(10);
			}
			if (GUILayout.Button("100"))
			{
				CreateAgents(100);
			}
			if (GUILayout.Button("500"))
			{
				CreateAgents(500);
			}
			if (GUILayout.Button("1000"))
			{
				CreateAgents(1000);
			}
			if (GUILayout.Button("5000"))
			{
				CreateAgents(5000);
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("Random Streams"))
			{
				type = RVOExampleType.RandomStreams;
				CreateAgents((agents == null) ? 100 : agents.Count);
			}
			if (GUILayout.Button("Line"))
			{
				type = RVOExampleType.Line;
				CreateAgents((agents == null) ? 10 : Mathf.Min(agents.Count, 100));
			}
			if (GUILayout.Button("Circle"))
			{
				type = RVOExampleType.Circle;
				CreateAgents((agents == null) ? 100 : agents.Count);
			}
			if (GUILayout.Button("Point"))
			{
				type = RVOExampleType.Point;
				CreateAgents((agents == null) ? 100 : agents.Count);
			}
			if (GUILayout.Button("Crossing"))
			{
				type = RVOExampleType.Crossing;
				CreateAgents((agents == null) ? 100 : agents.Count);
			}
		}

		private float uniformDistance(float radius)
		{
			float num = UnityEngine.Random.value + UnityEngine.Random.value;
			if (num > 1f)
			{
				return radius * (2f - num);
			}
			return radius * num;
		}

		public void CreateAgents(int num)
		{
			agentCount = num;
			agents = new List<IAgent>(agentCount);
			goals = new List<Vector3>(agentCount);
			colors = new List<Color>(agentCount);
			sim.ClearAgents();
			if (type == RVOExampleType.Circle)
			{
				float num2 = Mathf.Sqrt((float)agentCount * radius * radius * 4f / (float)Math.PI) * exampleScale * 0.05f;
				for (int i = 0; i < agentCount; i++)
				{
					Vector3 vector = new Vector3(Mathf.Cos((float)i * (float)Math.PI * 2f / (float)agentCount), 0f, Mathf.Sin((float)i * (float)Math.PI * 2f / (float)agentCount)) * num2 * (1f + UnityEngine.Random.value * 0.01f);
					IAgent item = sim.AddAgent(new Vector2(vector.x, vector.z), vector.y);
					agents.Add(item);
					goals.Add(-vector);
					colors.Add(AstarMath.HSVToRGB((float)i * 360f / (float)agentCount, 0.8f, 0.6f));
				}
			}
			else if (type == RVOExampleType.Line)
			{
				for (int j = 0; j < agentCount; j++)
				{
					Vector3 vector2 = new Vector3((float)((j % 2 == 0) ? 1 : (-1)) * exampleScale, 0f, (float)(j / 2) * radius * 2.5f);
					IAgent item2 = sim.AddAgent(new Vector2(vector2.x, vector2.z), vector2.y);
					agents.Add(item2);
					goals.Add(new Vector3(0f - vector2.x, vector2.y, vector2.z));
					colors.Add((j % 2 != 0) ? Color.blue : Color.red);
				}
			}
			else if (type == RVOExampleType.Point)
			{
				for (int k = 0; k < agentCount; k++)
				{
					Vector3 vector3 = new Vector3(Mathf.Cos((float)k * (float)Math.PI * 2f / (float)agentCount), 0f, Mathf.Sin((float)k * (float)Math.PI * 2f / (float)agentCount)) * exampleScale;
					IAgent item3 = sim.AddAgent(new Vector2(vector3.x, vector3.z), vector3.y);
					agents.Add(item3);
					goals.Add(new Vector3(0f, vector3.y, 0f));
					colors.Add(AstarMath.HSVToRGB((float)k * 360f / (float)agentCount, 0.8f, 0.6f));
				}
			}
			else if (type == RVOExampleType.RandomStreams)
			{
				float num3 = Mathf.Sqrt((float)agentCount * radius * radius * 4f / (float)Math.PI) * exampleScale * 0.05f;
				for (int l = 0; l < agentCount; l++)
				{
					float f = UnityEngine.Random.value * (float)Math.PI * 2f;
					float num4 = UnityEngine.Random.value * (float)Math.PI * 2f;
					Vector3 vector4 = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f)) * uniformDistance(num3);
					IAgent item4 = sim.AddAgent(new Vector2(vector4.x, vector4.z), vector4.y);
					agents.Add(item4);
					goals.Add(new Vector3(Mathf.Cos(num4), 0f, Mathf.Sin(num4)) * uniformDistance(num3));
					colors.Add(AstarMath.HSVToRGB(num4 * 57.29578f, 0.8f, 0.6f));
				}
			}
			else if (type == RVOExampleType.Crossing)
			{
				float num5 = exampleScale * radius * 0.5f;
				int a = (int)Mathf.Sqrt((float)agentCount / 25f);
				a = Mathf.Max(a, 2);
				for (int m = 0; m < agentCount; m++)
				{
					float num6 = (float)(m % a) / (float)a * (float)Math.PI * 2f;
					float num7 = num5 * ((float)(m / (a * 10) + 1) + 0.3f * UnityEngine.Random.value);
					Vector3 vector5 = new Vector3(Mathf.Cos(num6), 0f, Mathf.Sin(num6)) * num7;
					IAgent agent = sim.AddAgent(new Vector2(vector5.x, vector5.z), vector5.y);
					agent.Priority = ((m % a != 0) ? 0.01f : 1f);
					agents.Add(agent);
					goals.Add(-vector5.normalized * num5 * 3f);
					colors.Add(AstarMath.HSVToRGB(num6 * 57.29578f, 0.8f, 0.6f));
				}
			}
			SetAgentSettings();
			verts = new Vector3[4 * agents.Count];
			uv = new Vector2[verts.Length];
			tris = new int[agents.Count * 2 * 3];
			meshColors = new Color[verts.Length];
		}

		private void SetAgentSettings()
		{
			for (int i = 0; i < agents.Count; i++)
			{
				IAgent agent = agents[i];
				agent.Radius = radius;
				agent.AgentTimeHorizon = agentTimeHorizon;
				agent.ObstacleTimeHorizon = obstacleTimeHorizon;
				agent.MaxNeighbours = maxNeighbours;
				agent.DebugDraw = i == 0 && debug;
			}
		}

		public void Update()
		{
			if (agents == null || mesh == null)
			{
				return;
			}
			if (agents.Count != goals.Count)
			{
				Debug.LogError("Agent count does not match goal count");
				return;
			}
			SetAgentSettings();
			if (interpolatedVelocities == null || interpolatedVelocities.Length < agents.Count)
			{
				Vector2[] array = new Vector2[agents.Count];
				Vector2[] array2 = new Vector2[agents.Count];
				if (interpolatedVelocities != null)
				{
					for (int i = 0; i < interpolatedVelocities.Length; i++)
					{
						array[i] = interpolatedVelocities[i];
					}
				}
				if (interpolatedRotations != null)
				{
					for (int j = 0; j < interpolatedRotations.Length; j++)
					{
						array2[j] = interpolatedRotations[j];
					}
				}
				interpolatedVelocities = array;
				interpolatedRotations = array2;
			}
			for (int k = 0; k < agents.Count; k++)
			{
				IAgent agent = agents[k];
				Vector2 position = agent.Position;
				Vector2 vector = Vector2.ClampMagnitude(agent.CalculatedTargetPoint - position, agent.CalculatedSpeed * Time.deltaTime);
				position += vector;
				agent.Position = position;
				agent.ElevationCoordinate = 0f;
				Vector2 vector2 = new Vector2(goals[k].x, goals[k].z);
				float magnitude = (vector2 - position).magnitude;
				agent.SetTarget(vector2, Mathf.Min(magnitude, maxSpeed), maxSpeed * 1.1f);
				interpolatedVelocities[k] += vector;
				if (interpolatedVelocities[k].magnitude > maxSpeed * 0.1f)
				{
					interpolatedVelocities[k] = Vector2.ClampMagnitude(interpolatedVelocities[k], maxSpeed * 0.1f);
					interpolatedRotations[k] = Vector2.Lerp(interpolatedRotations[k], interpolatedVelocities[k], agent.CalculatedSpeed * Time.deltaTime * 4f);
				}
				Vector3 vector3 = new Vector3(interpolatedRotations[k].x, 0f, interpolatedRotations[k].y).normalized * agent.Radius;
				if (vector3 == Vector3.zero)
				{
					vector3 = new Vector3(0f, 0f, agent.Radius);
				}
				Vector3 vector4 = Vector3.Cross(Vector3.up, vector3);
				Vector3 vector5 = new Vector3(agent.Position.x, agent.ElevationCoordinate, agent.Position.y) + renderingOffset;
				int num = 4 * k;
				int num2 = 6 * k;
				verts[num] = vector5 + vector3 - vector4;
				verts[num + 1] = vector5 + vector3 + vector4;
				verts[num + 2] = vector5 - vector3 + vector4;
				verts[num + 3] = vector5 - vector3 - vector4;
				uv[num] = new Vector2(0f, 1f);
				uv[num + 1] = new Vector2(1f, 1f);
				uv[num + 2] = new Vector2(1f, 0f);
				uv[num + 3] = new Vector2(0f, 0f);
				meshColors[num] = colors[k];
				meshColors[num + 1] = colors[k];
				meshColors[num + 2] = colors[k];
				meshColors[num + 3] = colors[k];
				tris[num2] = num;
				tris[num2 + 1] = num + 1;
				tris[num2 + 2] = num + 2;
				tris[num2 + 3] = num;
				tris[num2 + 4] = num + 2;
				tris[num2 + 5] = num + 3;
			}
			mesh.Clear();
			mesh.vertices = verts;
			mesh.uv = uv;
			mesh.colors = meshColors;
			mesh.triangles = tris;
			mesh.RecalculateNormals();
		}
	}
}
