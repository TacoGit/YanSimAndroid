using System;
using System.Collections.Generic;
using System.Threading;
using Pathfinding.RVO.Sampled;
using UnityEngine;

namespace Pathfinding.RVO
{
	public class Simulator
	{
		internal class WorkerContext
		{
			public Agent.VOBuffer vos = new Agent.VOBuffer(16);

			public const int KeepCount = 3;

			public Vector2[] bestPos = new Vector2[3];

			public float[] bestSizes = new float[3];

			public float[] bestScores = new float[4];

			public Vector2[] samplePos = new Vector2[50];

			public float[] sampleSize = new float[50];
		}

		private class Worker
		{
			public int start;

			public int end;

			private readonly AutoResetEvent runFlag = new AutoResetEvent(false);

			private readonly ManualResetEvent waitFlag = new ManualResetEvent(true);

			private readonly Simulator simulator;

			private int task;

			private bool terminate;

			private WorkerContext context = new WorkerContext();

			public Worker(Simulator sim)
			{
				simulator = sim;
				Thread thread = new Thread(Run);
				thread.IsBackground = true;
				thread.Name = "RVO Simulator Thread";
				thread.Start();
			}

			public void Execute(int task)
			{
				this.task = task;
				waitFlag.Reset();
				runFlag.Set();
			}

			public void WaitOne()
			{
				if (!terminate)
				{
					waitFlag.WaitOne();
				}
			}

			public void Terminate()
			{
				WaitOne();
				terminate = true;
				Execute(-1);
			}

			public void Run()
			{
				runFlag.WaitOne();
				while (!terminate)
				{
					try
					{
						List<Agent> agents = simulator.GetAgents();
						if (task == 0)
						{
							for (int i = start; i < end; i++)
							{
								agents[i].CalculateNeighbours();
								agents[i].CalculateVelocity(context);
							}
						}
						else if (task == 1)
						{
							for (int j = start; j < end; j++)
							{
								agents[j].BufferSwitch();
							}
						}
						else
						{
							if (task != 2)
							{
								Debug.LogError("Invalid Task Number: " + task);
								throw new Exception("Invalid Task Number: " + task);
							}
							simulator.BuildQuadtree();
						}
					}
					catch (Exception message)
					{
						Debug.LogError(message);
					}
					waitFlag.Set();
					runFlag.WaitOne();
				}
			}
		}

		private readonly bool doubleBuffering = true;

		private float desiredDeltaTime = 0.05f;

		private readonly Worker[] workers;

		private List<Agent> agents;

		public List<ObstacleVertex> obstacles;

		private float deltaTime;

		private float lastStep = -99999f;

		private bool doUpdateObstacles;

		private bool doCleanObstacles;

		public float symmetryBreakingBias = 0.1f;

		public readonly MovementPlane movementPlane;

		private WorkerContext coroutineWorkerContext = new WorkerContext();

		public RVOQuadtree Quadtree { get; private set; }

		public float DeltaTime
		{
			get
			{
				return deltaTime;
			}
		}

		public bool Multithreading
		{
			get
			{
				return workers != null && workers.Length > 0;
			}
		}

		public float DesiredDeltaTime
		{
			get
			{
				return desiredDeltaTime;
			}
			set
			{
				desiredDeltaTime = Math.Max(value, 0f);
			}
		}

		public Simulator(int workers, bool doubleBuffering, MovementPlane movementPlane)
		{
			this.workers = new Worker[workers];
			this.doubleBuffering = doubleBuffering;
			DesiredDeltaTime = 1f;
			this.movementPlane = movementPlane;
			Quadtree = new RVOQuadtree();
			for (int i = 0; i < workers; i++)
			{
				this.workers[i] = new Worker(this);
			}
			agents = new List<Agent>();
			obstacles = new List<ObstacleVertex>();
		}

		public List<Agent> GetAgents()
		{
			return agents;
		}

		public List<ObstacleVertex> GetObstacles()
		{
			return obstacles;
		}

		public void ClearAgents()
		{
			BlockUntilSimulationStepIsDone();
			for (int i = 0; i < agents.Count; i++)
			{
				agents[i].simulator = null;
			}
			agents.Clear();
		}

		public void OnDestroy()
		{
			if (workers != null)
			{
				for (int i = 0; i < workers.Length; i++)
				{
					workers[i].Terminate();
				}
			}
		}

		~Simulator()
		{
			OnDestroy();
		}

		public IAgent AddAgent(IAgent agent)
		{
			if (agent == null)
			{
				throw new ArgumentNullException("Agent must not be null");
			}
			Agent agent2 = agent as Agent;
			if (agent2 == null)
			{
				throw new ArgumentException("The agent must be of type Agent. Agent was of type " + agent.GetType());
			}
			if (agent2.simulator != null && agent2.simulator == this)
			{
				throw new ArgumentException("The agent is already in the simulation");
			}
			if (agent2.simulator != null)
			{
				throw new ArgumentException("The agent is already added to another simulation");
			}
			agent2.simulator = this;
			BlockUntilSimulationStepIsDone();
			agents.Add(agent2);
			return agent;
		}

		[Obsolete("Use AddAgent(Vector2,float) instead")]
		public IAgent AddAgent(Vector3 position)
		{
			return AddAgent(new Vector2(position.x, position.z), position.y);
		}

		public IAgent AddAgent(Vector2 position, float elevationCoordinate)
		{
			return AddAgent(new Agent(position, elevationCoordinate));
		}

		public void RemoveAgent(IAgent agent)
		{
			if (agent == null)
			{
				throw new ArgumentNullException("Agent must not be null");
			}
			Agent agent2 = agent as Agent;
			if (agent2 == null)
			{
				throw new ArgumentException("The agent must be of type Agent. Agent was of type " + agent.GetType());
			}
			if (agent2.simulator != this)
			{
				throw new ArgumentException("The agent is not added to this simulation");
			}
			BlockUntilSimulationStepIsDone();
			agent2.simulator = null;
			if (!agents.Remove(agent2))
			{
				throw new ArgumentException("Critical Bug! This should not happen. Please report this.");
			}
		}

		public ObstacleVertex AddObstacle(ObstacleVertex v)
		{
			if (v == null)
			{
				throw new ArgumentNullException("Obstacle must not be null");
			}
			BlockUntilSimulationStepIsDone();
			obstacles.Add(v);
			UpdateObstacles();
			return v;
		}

		public ObstacleVertex AddObstacle(Vector3[] vertices, float height, bool cycle = true)
		{
			return AddObstacle(vertices, height, Matrix4x4.identity, RVOLayer.DefaultObstacle, cycle);
		}

		public ObstacleVertex AddObstacle(Vector3[] vertices, float height, Matrix4x4 matrix, RVOLayer layer = RVOLayer.DefaultObstacle, bool cycle = true)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException("Vertices must not be null");
			}
			if (vertices.Length < 2)
			{
				throw new ArgumentException("Less than 2 vertices in an obstacle");
			}
			ObstacleVertex obstacleVertex = null;
			ObstacleVertex obstacleVertex2 = null;
			BlockUntilSimulationStepIsDone();
			for (int i = 0; i < vertices.Length; i++)
			{
				ObstacleVertex obstacleVertex3 = new ObstacleVertex();
				obstacleVertex3.prev = obstacleVertex2;
				obstacleVertex3.layer = layer;
				obstacleVertex3.height = height;
				ObstacleVertex obstacleVertex4 = obstacleVertex3;
				if (obstacleVertex == null)
				{
					obstacleVertex = obstacleVertex4;
				}
				else
				{
					obstacleVertex2.next = obstacleVertex4;
				}
				obstacleVertex2 = obstacleVertex4;
			}
			if (cycle)
			{
				obstacleVertex2.next = obstacleVertex;
				obstacleVertex.prev = obstacleVertex2;
			}
			UpdateObstacle(obstacleVertex, vertices, matrix);
			obstacles.Add(obstacleVertex);
			return obstacleVertex;
		}

		public ObstacleVertex AddObstacle(Vector3 a, Vector3 b, float height)
		{
			ObstacleVertex obstacleVertex = new ObstacleVertex();
			ObstacleVertex obstacleVertex2 = new ObstacleVertex();
			obstacleVertex.layer = RVOLayer.DefaultObstacle;
			obstacleVertex2.layer = RVOLayer.DefaultObstacle;
			obstacleVertex.prev = obstacleVertex2;
			obstacleVertex2.prev = obstacleVertex;
			obstacleVertex.next = obstacleVertex2;
			obstacleVertex2.next = obstacleVertex;
			obstacleVertex.position = a;
			obstacleVertex2.position = b;
			obstacleVertex.height = height;
			obstacleVertex2.height = height;
			obstacleVertex2.ignore = true;
			obstacleVertex.dir = new Vector2(b.x - a.x, b.z - a.z).normalized;
			obstacleVertex2.dir = -obstacleVertex.dir;
			BlockUntilSimulationStepIsDone();
			obstacles.Add(obstacleVertex);
			UpdateObstacles();
			return obstacleVertex;
		}

		public void UpdateObstacle(ObstacleVertex obstacle, Vector3[] vertices, Matrix4x4 matrix)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException("Vertices must not be null");
			}
			if (obstacle == null)
			{
				throw new ArgumentNullException("Obstacle must not be null");
			}
			if (vertices.Length < 2)
			{
				throw new ArgumentException("Less than 2 vertices in an obstacle");
			}
			bool flag = matrix == Matrix4x4.identity;
			BlockUntilSimulationStepIsDone();
			int num = 0;
			ObstacleVertex obstacleVertex = obstacle;
			do
			{
				if (num >= vertices.Length)
				{
					Debug.DrawLine(obstacleVertex.prev.position, obstacleVertex.position, Color.red);
					throw new ArgumentException("Obstacle has more vertices than supplied for updating (" + vertices.Length + " supplied)");
				}
				obstacleVertex.position = ((!flag) ? matrix.MultiplyPoint3x4(vertices[num]) : vertices[num]);
				obstacleVertex = obstacleVertex.next;
				num++;
			}
			while (obstacleVertex != obstacle && obstacleVertex != null);
			obstacleVertex = obstacle;
			do
			{
				if (obstacleVertex.next == null)
				{
					obstacleVertex.dir = Vector2.zero;
				}
				else
				{
					Vector3 vector = obstacleVertex.next.position - obstacleVertex.position;
					obstacleVertex.dir = new Vector2(vector.x, vector.z).normalized;
				}
				obstacleVertex = obstacleVertex.next;
			}
			while (obstacleVertex != obstacle && obstacleVertex != null);
			ScheduleCleanObstacles();
			UpdateObstacles();
		}

		private void ScheduleCleanObstacles()
		{
			doCleanObstacles = true;
		}

		private void CleanObstacles()
		{
		}

		public void RemoveObstacle(ObstacleVertex v)
		{
			if (v == null)
			{
				throw new ArgumentNullException("Vertex must not be null");
			}
			BlockUntilSimulationStepIsDone();
			obstacles.Remove(v);
			UpdateObstacles();
		}

		public void UpdateObstacles()
		{
			doUpdateObstacles = true;
		}

		private void BuildQuadtree()
		{
			Quadtree.Clear();
			if (agents.Count > 0)
			{
				Rect bounds = Rect.MinMaxRect(agents[0].position.x, agents[0].position.y, agents[0].position.x, agents[0].position.y);
				for (int i = 1; i < agents.Count; i++)
				{
					Vector2 position = agents[i].position;
					bounds = Rect.MinMaxRect(Mathf.Min(bounds.xMin, position.x), Mathf.Min(bounds.yMin, position.y), Mathf.Max(bounds.xMax, position.x), Mathf.Max(bounds.yMax, position.y));
				}
				Quadtree.SetBounds(bounds);
				for (int j = 0; j < agents.Count; j++)
				{
					Quadtree.Insert(agents[j]);
				}
			}
			Quadtree.CalculateSpeeds();
		}

		private void BlockUntilSimulationStepIsDone()
		{
			if (Multithreading && doubleBuffering)
			{
				for (int i = 0; i < workers.Length; i++)
				{
					workers[i].WaitOne();
				}
			}
		}

		private void PreCalculation()
		{
			for (int i = 0; i < agents.Count; i++)
			{
				agents[i].PreCalculation();
			}
		}

		private void CleanAndUpdateObstaclesIfNecessary()
		{
			if (doCleanObstacles)
			{
				CleanObstacles();
				doCleanObstacles = false;
				doUpdateObstacles = true;
			}
			if (doUpdateObstacles)
			{
				doUpdateObstacles = false;
			}
		}

		public void Update()
		{
			if (lastStep < 0f)
			{
				lastStep = Time.time;
				deltaTime = DesiredDeltaTime;
			}
			if (!(Time.time - lastStep >= DesiredDeltaTime))
			{
				return;
			}
			deltaTime = Time.time - lastStep;
			lastStep = Time.time;
			deltaTime = Math.Max(deltaTime, 0.0005f);
			if (Multithreading)
			{
				if (doubleBuffering)
				{
					for (int i = 0; i < workers.Length; i++)
					{
						workers[i].WaitOne();
					}
					for (int j = 0; j < agents.Count; j++)
					{
						agents[j].PostCalculation();
					}
				}
				PreCalculation();
				CleanAndUpdateObstaclesIfNecessary();
				BuildQuadtree();
				for (int k = 0; k < workers.Length; k++)
				{
					workers[k].start = k * agents.Count / workers.Length;
					workers[k].end = (k + 1) * agents.Count / workers.Length;
				}
				for (int l = 0; l < workers.Length; l++)
				{
					workers[l].Execute(1);
				}
				for (int m = 0; m < workers.Length; m++)
				{
					workers[m].WaitOne();
				}
				for (int n = 0; n < workers.Length; n++)
				{
					workers[n].Execute(0);
				}
				if (!doubleBuffering)
				{
					for (int num = 0; num < workers.Length; num++)
					{
						workers[num].WaitOne();
					}
					for (int num2 = 0; num2 < agents.Count; num2++)
					{
						agents[num2].PostCalculation();
					}
				}
			}
			else
			{
				PreCalculation();
				CleanAndUpdateObstaclesIfNecessary();
				BuildQuadtree();
				for (int num3 = 0; num3 < agents.Count; num3++)
				{
					agents[num3].BufferSwitch();
				}
				for (int num4 = 0; num4 < agents.Count; num4++)
				{
					agents[num4].CalculateNeighbours();
					agents[num4].CalculateVelocity(coroutineWorkerContext);
				}
				for (int num5 = 0; num5 < agents.Count; num5++)
				{
					agents[num5].PostCalculation();
				}
			}
		}
	}
}
