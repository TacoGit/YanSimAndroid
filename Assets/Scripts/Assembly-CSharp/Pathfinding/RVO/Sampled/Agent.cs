using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.RVO.Sampled
{
	public class Agent : IAgent
	{
		internal struct VO
		{
			private Vector2 line1;

			private Vector2 line2;

			private Vector2 dir1;

			private Vector2 dir2;

			private Vector2 cutoffLine;

			private Vector2 cutoffDir;

			private Vector2 circleCenter;

			private bool colliding;

			private float radius;

			private float weightFactor;

			private float weightBonus;

			private Vector2 segmentStart;

			private Vector2 segmentEnd;

			private bool segment;

			public VO(Vector2 center, Vector2 offset, float radius, float inverseDt, float inverseDeltaTime)
			{
				weightFactor = 1f;
				weightBonus = 0f;
				circleCenter = center * inverseDt + offset;
				weightFactor = 4f * Mathf.Exp(0f - Sqr(center.sqrMagnitude / (radius * radius))) + 1f;
				if (center.magnitude < radius)
				{
					colliding = true;
					line1 = center.normalized * (center.magnitude - radius - 0.001f) * 0.3f * inverseDeltaTime;
					dir1 = new Vector2(line1.y, 0f - line1.x).normalized;
					line1 += offset;
					cutoffDir = Vector2.zero;
					cutoffLine = Vector2.zero;
					dir2 = Vector2.zero;
					line2 = Vector2.zero;
					this.radius = 0f;
				}
				else
				{
					colliding = false;
					center *= inverseDt;
					radius *= inverseDt;
					Vector2 vector = center + offset;
					float num = center.magnitude - radius + 0.001f;
					cutoffLine = center.normalized * num;
					cutoffDir = new Vector2(0f - cutoffLine.y, cutoffLine.x).normalized;
					cutoffLine += offset;
					float num2 = Mathf.Atan2(0f - center.y, 0f - center.x);
					float num3 = Mathf.Abs(Mathf.Acos(radius / center.magnitude));
					this.radius = radius;
					line1 = new Vector2(Mathf.Cos(num2 + num3), Mathf.Sin(num2 + num3));
					dir1 = new Vector2(line1.y, 0f - line1.x);
					line2 = new Vector2(Mathf.Cos(num2 - num3), Mathf.Sin(num2 - num3));
					dir2 = new Vector2(line2.y, 0f - line2.x);
					line1 = line1 * radius + vector;
					line2 = line2 * radius + vector;
				}
				segmentStart = Vector2.zero;
				segmentEnd = Vector2.zero;
				segment = false;
			}

			public static VO SegmentObstacle(Vector2 segmentStart, Vector2 segmentEnd, Vector2 offset, float radius, float inverseDt, float inverseDeltaTime)
			{
				VO result = default(VO);
				result.weightFactor = 1f;
				result.weightBonus = Mathf.Max(radius, 1f) * 40f;
				Vector3 vector = VectorMath.ClosestPointOnSegment(segmentStart, segmentEnd, Vector2.zero);
				if (vector.magnitude <= radius)
				{
					result.colliding = true;
					result.line1 = vector.normalized * (vector.magnitude - radius) * 0.3f * inverseDeltaTime;
					result.dir1 = new Vector2(result.line1.y, 0f - result.line1.x).normalized;
					result.line1 += offset;
					result.cutoffDir = Vector2.zero;
					result.cutoffLine = Vector2.zero;
					result.dir2 = Vector2.zero;
					result.line2 = Vector2.zero;
					result.radius = 0f;
					result.segmentStart = Vector2.zero;
					result.segmentEnd = Vector2.zero;
					result.segment = false;
				}
				else
				{
					result.colliding = false;
					segmentStart *= inverseDt;
					segmentEnd *= inverseDt;
					radius *= inverseDt;
					Vector2 vector2 = (result.cutoffDir = (segmentEnd - segmentStart).normalized);
					result.cutoffLine = segmentStart + new Vector2(0f - vector2.y, vector2.x) * radius;
					result.cutoffLine += offset;
					float sqrMagnitude = segmentStart.sqrMagnitude;
					Vector2 vector3 = -VectorMath.ComplexMultiply(segmentStart, new Vector2(radius, Mathf.Sqrt(Mathf.Max(0f, sqrMagnitude - radius * radius)))) / sqrMagnitude;
					float sqrMagnitude2 = segmentEnd.sqrMagnitude;
					Vector2 vector4 = -VectorMath.ComplexMultiply(segmentEnd, new Vector2(radius, 0f - Mathf.Sqrt(Mathf.Max(0f, sqrMagnitude2 - radius * radius)))) / sqrMagnitude2;
					result.line1 = segmentStart + vector3 * radius + offset;
					result.line2 = segmentEnd + vector4 * radius + offset;
					result.dir1 = new Vector2(vector3.y, 0f - vector3.x);
					result.dir2 = new Vector2(vector4.y, 0f - vector4.x);
					result.segmentStart = segmentStart;
					result.segmentEnd = segmentEnd;
					result.radius = radius;
					result.segment = true;
				}
				return result;
			}

			public static float SignedDistanceFromLine(Vector2 a, Vector2 dir, Vector2 p)
			{
				return (p.x - a.x) * dir.y - dir.x * (p.y - a.y);
			}

			public Vector2 ScaledGradient(Vector2 p, out float weight)
			{
				Vector2 result = Gradient(p, out weight);
				if (weight > 0f)
				{
					result *= 2f * weightFactor;
					weight *= 2f * weightFactor;
					weight += 1f + weightBonus;
				}
				return result;
			}

			public Vector2 Gradient(Vector2 p, out float weight)
			{
				if (colliding)
				{
					float num = SignedDistanceFromLine(line1, dir1, p);
					if (num >= 0f)
					{
						weight = num;
						return new Vector2(0f - dir1.y, dir1.x);
					}
					weight = 0f;
					return new Vector2(0f, 0f);
				}
				float num2 = SignedDistanceFromLine(cutoffLine, cutoffDir, p);
				if (num2 <= 0f)
				{
					weight = 0f;
					return Vector2.zero;
				}
				float num3 = SignedDistanceFromLine(line1, dir1, p);
				float num4 = SignedDistanceFromLine(line2, dir2, p);
				if (num3 >= 0f && num4 >= 0f)
				{
					Vector2 result;
					if (Vector2.Dot(p - line1, dir1) > 0f && Vector2.Dot(p - line2, dir2) < 0f)
					{
						if (!segment)
						{
							Vector2 v = p - circleCenter;
							float magnitude;
							result = VectorMath.Normalize(v, out magnitude);
							weight = radius - magnitude;
							return result;
						}
						if (num2 < radius)
						{
							Vector2 vector = VectorMath.ClosestPointOnSegment(segmentStart, segmentEnd, p);
							Vector2 v2 = p - vector;
							float magnitude2;
							result = VectorMath.Normalize(v2, out magnitude2);
							weight = radius - magnitude2;
							return result;
						}
					}
					if (segment && num2 < num3 && num2 < num4)
					{
						weight = num2;
						return new Vector2(0f - cutoffDir.y, cutoffDir.x);
					}
					if (num3 < num4)
					{
						weight = num3;
						result = new Vector2(0f - dir1.y, dir1.x);
					}
					else
					{
						weight = num4;
						result = new Vector2(0f - dir2.y, dir2.x);
					}
					return result;
				}
				weight = 0f;
				return Vector2.zero;
			}
		}

		internal class VOBuffer
		{
			public VO[] buffer;

			public int length;

			public VOBuffer(int n)
			{
				buffer = new VO[n];
				length = 0;
			}

			public void Clear()
			{
				length = 0;
			}

			public void Add(VO vo)
			{
				if (length >= buffer.Length)
				{
					VO[] array = new VO[buffer.Length * 2];
					buffer.CopyTo(array, 0);
					buffer = array;
				}
				buffer[length++] = vo;
			}
		}

		internal float radius;

		internal float height;

		internal float desiredSpeed;

		internal float maxSpeed;

		internal float agentTimeHorizon;

		internal float obstacleTimeHorizon;

		internal bool locked;

		private RVOLayer layer;

		private RVOLayer collidesWith;

		private int maxNeighbours;

		internal Vector2 position;

		private float elevationCoordinate;

		private Vector2 currentVelocity;

		private Vector2 desiredTargetPointInVelocitySpace;

		private Vector2 desiredVelocity;

		private Vector2 nextTargetPoint;

		private float nextDesiredSpeed;

		private float nextMaxSpeed;

		private Vector2 collisionNormal;

		private bool manuallyControlled;

		private bool debugDraw;

		internal Agent next;

		private float calculatedSpeed;

		private Vector2 calculatedTargetPoint;

		internal Simulator simulator;

		private List<Agent> neighbours = new List<Agent>();

		private List<float> neighbourDists = new List<float>();

		private List<ObstacleVertex> obstaclesBuffered = new List<ObstacleVertex>();

		private List<ObstacleVertex> obstacles = new List<ObstacleVertex>();

		private const float DesiredVelocityWeight = 0.1f;

		private const float WallWeight = 5f;

		public Vector2 Position { get; set; }

		public float ElevationCoordinate { get; set; }

		public Vector2 CalculatedTargetPoint { get; private set; }

		public float CalculatedSpeed { get; private set; }

		public bool Locked { get; set; }

		public float Radius { get; set; }

		public float Height { get; set; }

		public float AgentTimeHorizon { get; set; }

		public float ObstacleTimeHorizon { get; set; }

		public int MaxNeighbours { get; set; }

		public int NeighbourCount { get; private set; }

		public RVOLayer Layer { get; set; }

		public RVOLayer CollidesWith { get; set; }

		public bool DebugDraw
		{
			get
			{
				return debugDraw;
			}
			set
			{
				debugDraw = value && simulator != null && !simulator.Multithreading;
			}
		}

		public float Priority { get; set; }

		public Action PreCalculationCallback { private get; set; }

		public List<ObstacleVertex> NeighbourObstacles
		{
			get
			{
				return null;
			}
		}

		public Agent(Vector2 pos, float elevationCoordinate)
		{
			AgentTimeHorizon = 2f;
			ObstacleTimeHorizon = 2f;
			Height = 5f;
			Radius = 5f;
			MaxNeighbours = 10;
			Locked = false;
			Position = pos;
			ElevationCoordinate = elevationCoordinate;
			Layer = RVOLayer.DefaultAgent;
			CollidesWith = (RVOLayer)(-1);
			Priority = 0.5f;
			CalculatedTargetPoint = pos;
			CalculatedSpeed = 0f;
			SetTarget(pos, 0f, 0f);
		}

		public void SetTarget(Vector2 targetPoint, float desiredSpeed, float maxSpeed)
		{
			maxSpeed = Math.Max(maxSpeed, 0f);
			desiredSpeed = Math.Min(Math.Max(desiredSpeed, 0f), maxSpeed);
			nextTargetPoint = targetPoint;
			nextDesiredSpeed = desiredSpeed;
			nextMaxSpeed = maxSpeed;
		}

		public void SetCollisionNormal(Vector2 normal)
		{
			collisionNormal = normal;
		}

		public void ForceSetVelocity(Vector2 velocity)
		{
			Vector2 vector2 = (CalculatedTargetPoint = position + velocity * 1000f);
			nextTargetPoint = vector2;
			float num = (CalculatedSpeed = velocity.magnitude);
			nextDesiredSpeed = num;
			manuallyControlled = true;
		}

		public void BufferSwitch()
		{
			radius = Radius;
			height = Height;
			maxSpeed = nextMaxSpeed;
			desiredSpeed = nextDesiredSpeed;
			agentTimeHorizon = AgentTimeHorizon;
			obstacleTimeHorizon = ObstacleTimeHorizon;
			maxNeighbours = MaxNeighbours;
			locked = Locked && !manuallyControlled;
			position = Position;
			elevationCoordinate = ElevationCoordinate;
			collidesWith = CollidesWith;
			layer = Layer;
			if (locked)
			{
				desiredTargetPointInVelocitySpace = position;
				desiredVelocity = (currentVelocity = Vector2.zero);
				return;
			}
			desiredTargetPointInVelocitySpace = nextTargetPoint - position;
			currentVelocity = (CalculatedTargetPoint - position).normalized * CalculatedSpeed;
			desiredVelocity = desiredTargetPointInVelocitySpace.normalized * desiredSpeed;
			if (collisionNormal != Vector2.zero)
			{
				collisionNormal.Normalize();
				float num = Vector2.Dot(currentVelocity, collisionNormal);
				if (num < 0f)
				{
					currentVelocity -= collisionNormal * num;
				}
				collisionNormal = Vector2.zero;
			}
		}

		public void PreCalculation()
		{
			if (PreCalculationCallback != null)
			{
				PreCalculationCallback();
			}
		}

		public void PostCalculation()
		{
			if (!manuallyControlled)
			{
				CalculatedTargetPoint = calculatedTargetPoint;
				CalculatedSpeed = calculatedSpeed;
			}
			List<ObstacleVertex> list = obstaclesBuffered;
			obstaclesBuffered = obstacles;
			obstacles = list;
			manuallyControlled = false;
		}

		public void CalculateNeighbours()
		{
			neighbours.Clear();
			neighbourDists.Clear();
			if (MaxNeighbours > 0 && !locked)
			{
				simulator.Quadtree.Query(position, maxSpeed, agentTimeHorizon, radius, this);
			}
			NeighbourCount = neighbours.Count;
		}

		private static float Sqr(float x)
		{
			return x * x;
		}

		internal float InsertAgentNeighbour(Agent agent, float rangeSq)
		{
			if (this == agent || (agent.layer & collidesWith) == 0)
			{
				return rangeSq;
			}
			float sqrMagnitude = (agent.position - position).sqrMagnitude;
			if (sqrMagnitude < rangeSq)
			{
				if (neighbours.Count < maxNeighbours)
				{
					neighbours.Add(null);
					neighbourDists.Add(float.PositiveInfinity);
				}
				int num = neighbours.Count - 1;
				if (sqrMagnitude < neighbourDists[num])
				{
					while (num != 0 && sqrMagnitude < neighbourDists[num - 1])
					{
						neighbours[num] = neighbours[num - 1];
						neighbourDists[num] = neighbourDists[num - 1];
						num--;
					}
					neighbours[num] = agent;
					neighbourDists[num] = sqrMagnitude;
				}
				if (neighbours.Count == maxNeighbours)
				{
					rangeSq = neighbourDists[neighbourDists.Count - 1];
				}
			}
			return rangeSq;
		}

		private static Vector3 FromXZ(Vector2 p)
		{
			return new Vector3(p.x, 0f, p.y);
		}

		private static Vector2 ToXZ(Vector3 p)
		{
			return new Vector2(p.x, p.z);
		}

		private Vector2 To2D(Vector3 p, out float elevation)
		{
			if (simulator.movementPlane == MovementPlane.XY)
			{
				elevation = 0f - p.z;
				return new Vector2(p.x, p.y);
			}
			elevation = p.y;
			return new Vector2(p.x, p.z);
		}

		private static void DrawVO(Vector2 circleCenter, float radius, Vector2 origin)
		{
			float num = Mathf.Atan2((origin - circleCenter).y, (origin - circleCenter).x);
			float num2 = radius / (origin - circleCenter).magnitude;
			float num3 = ((!(num2 <= 1f)) ? 0f : Mathf.Abs(Mathf.Acos(num2)));
			Draw.Debug.CircleXZ(FromXZ(circleCenter), radius, Color.black, num - num3, num + num3);
			Vector2 p = new Vector2(Mathf.Cos(num - num3), Mathf.Sin(num - num3)) * radius;
			Vector2 p2 = new Vector2(Mathf.Cos(num + num3), Mathf.Sin(num + num3)) * radius;
			Vector2 p3 = -new Vector2(0f - p.y, p.x);
			Vector2 p4 = new Vector2(0f - p2.y, p2.x);
			p += circleCenter;
			p2 += circleCenter;
			Debug.DrawRay(FromXZ(p), FromXZ(p3).normalized * 100f, Color.black);
			Debug.DrawRay(FromXZ(p2), FromXZ(p4).normalized * 100f, Color.black);
		}

		internal void CalculateVelocity(Simulator.WorkerContext context)
		{
			if (manuallyControlled)
			{
				return;
			}
			if (locked)
			{
				calculatedSpeed = 0f;
				calculatedTargetPoint = position;
				return;
			}
			VOBuffer vos = context.vos;
			vos.Clear();
			GenerateObstacleVOs(vos);
			GenerateNeighbourAgentVOs(vos);
			if (!BiasDesiredVelocity(vos, ref desiredVelocity, ref desiredTargetPointInVelocitySpace, simulator.symmetryBreakingBias))
			{
				calculatedTargetPoint = desiredTargetPointInVelocitySpace + position;
				calculatedSpeed = desiredSpeed;
				if (DebugDraw)
				{
					Draw.Debug.CrossXZ(FromXZ(calculatedTargetPoint), Color.white);
				}
				return;
			}
			Vector2 zero = Vector2.zero;
			zero = GradientDescent(vos, currentVelocity, desiredVelocity);
			if (DebugDraw)
			{
				Draw.Debug.CrossXZ(FromXZ(zero + position), Color.white);
			}
			calculatedTargetPoint = position + zero;
			calculatedSpeed = Mathf.Min(zero.magnitude, maxSpeed);
		}

		private static Color Rainbow(float v)
		{
			Color result = new Color(v, 0f, 0f);
			if (result.r > 1f)
			{
				result.g = result.r - 1f;
				result.r = 1f;
			}
			if (result.g > 1f)
			{
				result.b = result.g - 1f;
				result.g = 1f;
			}
			return result;
		}

		private void GenerateObstacleVOs(VOBuffer vos)
		{
			float num = maxSpeed * obstacleTimeHorizon;
			for (int i = 0; i < simulator.obstacles.Count; i++)
			{
				ObstacleVertex obstacleVertex = simulator.obstacles[i];
				ObstacleVertex obstacleVertex2 = obstacleVertex;
				do
				{
					if (obstacleVertex2.ignore || (obstacleVertex2.layer & collidesWith) == 0)
					{
						obstacleVertex2 = obstacleVertex2.next;
						continue;
					}
					float elevation;
					Vector2 vector = To2D(obstacleVertex2.position, out elevation);
					float elevation2;
					Vector2 vector2 = To2D(obstacleVertex2.next.position, out elevation2);
					Vector2 normalized = (vector2 - vector).normalized;
					float num2 = VO.SignedDistanceFromLine(vector, normalized, position);
					if (num2 >= -0.01f && num2 < num)
					{
						float t = Vector2.Dot(position - vector, vector2 - vector) / (vector2 - vector).sqrMagnitude;
						float num3 = Mathf.Lerp(elevation, elevation2, t);
						float sqrMagnitude = (Vector2.Lerp(vector, vector2, t) - position).sqrMagnitude;
						if (sqrMagnitude < num * num && (simulator.movementPlane == MovementPlane.XY || (elevationCoordinate <= num3 + obstacleVertex2.height && elevationCoordinate + height >= num3)))
						{
							vos.Add(VO.SegmentObstacle(vector2 - position, vector - position, Vector2.zero, radius * 0.01f, 1f / ObstacleTimeHorizon, 1f / simulator.DeltaTime));
						}
					}
					obstacleVertex2 = obstacleVertex2.next;
				}
				while (obstacleVertex2 != obstacleVertex && obstacleVertex2 != null && obstacleVertex2.next != null);
			}
		}

		private void GenerateNeighbourAgentVOs(VOBuffer vos)
		{
			float num = 1f / agentTimeHorizon;
			Vector2 a = currentVelocity;
			for (int i = 0; i < neighbours.Count; i++)
			{
				Agent agent = neighbours[i];
				if (agent == this)
				{
					continue;
				}
				float num2 = Math.Min(elevationCoordinate + height, agent.elevationCoordinate + agent.height);
				float num3 = Math.Max(elevationCoordinate, agent.elevationCoordinate);
				if (!(num2 - num3 < 0f))
				{
					float num4 = radius + agent.radius;
					Vector2 vector = agent.position - position;
					float num5 = ((!agent.locked && !agent.manuallyControlled) ? ((!(agent.Priority > 1E-05f) && !(Priority > 1E-05f)) ? 0.5f : (agent.Priority / (Priority + agent.Priority))) : 1f);
					Vector2 b = Vector2.Lerp(agent.currentVelocity, agent.desiredVelocity, 2f * num5 - 1f);
					Vector2 vector2 = Vector2.Lerp(a, b, num5);
					vos.Add(new VO(vector, vector2, num4, num, 1f / simulator.DeltaTime));
					if (DebugDraw)
					{
						DrawVO(position + vector * num + vector2, num4 * num, position + vector2);
					}
				}
			}
		}

		private Vector2 GradientDescent(VOBuffer vos, Vector2 sampleAround1, Vector2 sampleAround2)
		{
			float score;
			Vector2 vector = Trace(vos, sampleAround1, out score);
			if (DebugDraw)
			{
				Draw.Debug.CrossXZ(FromXZ(vector + position), Color.yellow, 0.5f);
			}
			float score2;
			Vector2 vector2 = Trace(vos, sampleAround2, out score2);
			if (DebugDraw)
			{
				Draw.Debug.CrossXZ(FromXZ(vector2 + position), Color.magenta, 0.5f);
			}
			return (!(score < score2)) ? vector2 : vector;
		}

		private static bool BiasDesiredVelocity(VOBuffer vos, ref Vector2 desiredVelocity, ref Vector2 targetPointInVelocitySpace, float maxBiasRadians)
		{
			float magnitude = desiredVelocity.magnitude;
			float num = 0f;
			for (int i = 0; i < vos.length; i++)
			{
				float weight;
				vos.buffer[i].Gradient(desiredVelocity, out weight);
				num = Mathf.Max(num, weight);
			}
			bool result = num > 0f;
			if (magnitude < 0.001f)
			{
				return result;
			}
			float num2 = Mathf.Min(maxBiasRadians, num / magnitude);
			desiredVelocity += new Vector2(desiredVelocity.y, 0f - desiredVelocity.x) * num2;
			targetPointInVelocitySpace += new Vector2(targetPointInVelocitySpace.y, 0f - targetPointInVelocitySpace.x) * num2;
			return result;
		}

		private Vector2 EvaluateGradient(VOBuffer vos, Vector2 p, out float value)
		{
			Vector2 result = Vector2.zero;
			value = 0f;
			for (int i = 0; i < vos.length; i++)
			{
				float weight;
				Vector2 vector = vos.buffer[i].ScaledGradient(p, out weight);
				if (weight > value)
				{
					value = weight;
					result = vector;
				}
			}
			Vector2 vector2 = desiredVelocity - p;
			float magnitude = vector2.magnitude;
			if (magnitude > 0.0001f)
			{
				result += vector2 * (0.1f / magnitude);
				value += magnitude * 0.1f;
			}
			float sqrMagnitude = p.sqrMagnitude;
			if (sqrMagnitude > desiredSpeed * desiredSpeed)
			{
				float num = Mathf.Sqrt(sqrMagnitude);
				if (num > maxSpeed)
				{
					value += 3f * (num - maxSpeed);
					result -= 3f * (p / num);
				}
				float num2 = 0.2f;
				value += num2 * (num - desiredSpeed);
				result -= num2 * (p / num);
			}
			return result;
		}

		private Vector2 Trace(VOBuffer vos, Vector2 p, out float score)
		{
			float num = Mathf.Max(radius, 0.2f * desiredSpeed);
			float num2 = float.PositiveInfinity;
			Vector2 result = p;
			for (int i = 0; i < 50; i++)
			{
				float x = 1f - (float)i / 50f;
				x = Sqr(x) * num;
				float value;
				Vector2 vector = EvaluateGradient(vos, p, out value);
				if (value < num2)
				{
					num2 = value;
					result = p;
				}
				vector.Normalize();
				vector *= x;
				Vector2 vector2 = p;
				p += vector;
				if (DebugDraw)
				{
					Debug.DrawLine(FromXZ(vector2 + position), FromXZ(p + position), Rainbow((float)i * 0.1f) * new Color(1f, 1f, 1f, 1f));
				}
			}
			score = num2;
			return result;
		}
	}
}
