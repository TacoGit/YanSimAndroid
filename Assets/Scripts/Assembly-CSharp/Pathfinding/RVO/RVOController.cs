using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.RVO
{
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Controller")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_controller.php")]
	public class RVOController : VersionedMonoBehaviour
	{
		[Tooltip("Radius of the agent")]
		public float radius = 0.5f;

		[Tooltip("Height of the agent. In world units")]
		public float height = 2f;

		[Tooltip("A locked unit cannot move. Other units will still avoid it. But avoidance quality is not the best")]
		public bool locked;

		[Tooltip("Automatically set #locked to true when desired velocity is approximately zero")]
		public bool lockWhenNotMoving;

		[Tooltip("How far into the future to look for collisions with other agents (in seconds)")]
		public float agentTimeHorizon = 2f;

		[Tooltip("How far into the future to look for collisions with obstacles (in seconds)")]
		public float obstacleTimeHorizon = 2f;

		[Tooltip("Max number of other agents to take into account.\nA smaller value can reduce CPU load, a higher value can lead to better local avoidance quality.")]
		public int maxNeighbours = 10;

		public RVOLayer layer = RVOLayer.DefaultAgent;

		[EnumFlag]
		public RVOLayer collidesWith = (RVOLayer)(-1);

		[HideInInspector]
		[Obsolete]
		public float wallAvoidForce = 1f;

		[HideInInspector]
		[Obsolete]
		public float wallAvoidFalloff = 1f;

		[Tooltip("How strongly other agents will avoid this agent")]
		[Range(0f, 1f)]
		public float priority = 0.5f;

		[Tooltip("Center of the agent relative to the pivot point of this game object")]
		public float center = 1f;

		protected Transform tr;

		protected IAstarAI ai;

		public bool debug;

		private static readonly Color GizmoColor = new Color(0.9411765f, 71f / 85f, 0.11764706f);

		[Obsolete("This field is obsolete in version 4.0 and will not affect anything. Use the LegacyRVOController if you need the old behaviour")]
		public LayerMask mask
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		[Obsolete("This field is obsolete in version 4.0 and will not affect anything. Use the LegacyRVOController if you need the old behaviour")]
		public bool enableRotation
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		[Obsolete("This field is obsolete in version 4.0 and will not affect anything. Use the LegacyRVOController if you need the old behaviour")]
		public float rotationSpeed
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		[Obsolete("This field is obsolete in version 4.0 and will not affect anything. Use the LegacyRVOController if you need the old behaviour")]
		public float maxSpeed
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public MovementPlane movementPlane
		{
			get
			{
				if (simulator != null)
				{
					return simulator.movementPlane;
				}
				if ((bool)RVOSimulator.active)
				{
					return RVOSimulator.active.movementPlane;
				}
				return MovementPlane.XZ;
			}
		}

		public IAgent rvoAgent { get; private set; }

		public Simulator simulator { get; private set; }

		public Vector3 position
		{
			get
			{
				return To3D(rvoAgent.Position, rvoAgent.ElevationCoordinate);
			}
		}

		public Vector3 velocity
		{
			get
			{
				float num = ((!(Time.deltaTime > 0.0001f)) ? 0.02f : Time.deltaTime);
				return CalculateMovementDelta(num) / num;
			}
			set
			{
				rvoAgent.ForceSetVelocity(To2D(value));
			}
		}

		public Vector3 CalculateMovementDelta(float deltaTime)
		{
			if (rvoAgent == null)
			{
				return Vector3.zero;
			}
			return To3D(Vector2.ClampMagnitude(rvoAgent.CalculatedTargetPoint - To2D((ai == null) ? tr.position : ai.position), rvoAgent.CalculatedSpeed * deltaTime), 0f);
		}

		public Vector3 CalculateMovementDelta(Vector3 position, float deltaTime)
		{
			return To3D(Vector2.ClampMagnitude(rvoAgent.CalculatedTargetPoint - To2D(position), rvoAgent.CalculatedSpeed * deltaTime), 0f);
		}

		public void SetCollisionNormal(Vector3 normal)
		{
			rvoAgent.SetCollisionNormal(To2D(normal));
		}

		[Obsolete("Set the 'velocity' property instead")]
		public void ForceSetVelocity(Vector3 velocity)
		{
			this.velocity = velocity;
		}

		public Vector2 To2D(Vector3 p)
		{
			float elevation;
			return To2D(p, out elevation);
		}

		public Vector2 To2D(Vector3 p, out float elevation)
		{
			if (movementPlane == MovementPlane.XY)
			{
				elevation = 0f - p.z;
				return new Vector2(p.x, p.y);
			}
			elevation = p.y;
			return new Vector2(p.x, p.z);
		}

		public Vector3 To3D(Vector2 p, float elevationCoordinate)
		{
			if (movementPlane == MovementPlane.XY)
			{
				return new Vector3(p.x, p.y, 0f - elevationCoordinate);
			}
			return new Vector3(p.x, elevationCoordinate, p.y);
		}

		private void OnDisable()
		{
			if (simulator != null)
			{
				simulator.RemoveAgent(rvoAgent);
			}
		}

		private void OnEnable()
		{
			tr = base.transform;
			ai = GetComponent<IAstarAI>();
			if (RVOSimulator.active == null)
			{
				Debug.LogError("No RVOSimulator component found in the scene. Please add one.");
				base.enabled = false;
				return;
			}
			simulator = RVOSimulator.active.GetSimulator();
			if (rvoAgent != null)
			{
				simulator.AddAgent(rvoAgent);
				return;
			}
			rvoAgent = simulator.AddAgent(Vector2.zero, 0f);
			rvoAgent.PreCalculationCallback = UpdateAgentProperties;
		}

		protected void UpdateAgentProperties()
		{
			rvoAgent.Radius = Mathf.Max(0.001f, radius);
			rvoAgent.AgentTimeHorizon = agentTimeHorizon;
			rvoAgent.ObstacleTimeHorizon = obstacleTimeHorizon;
			rvoAgent.Locked = locked;
			rvoAgent.MaxNeighbours = maxNeighbours;
			rvoAgent.DebugDraw = debug;
			rvoAgent.Layer = layer;
			rvoAgent.CollidesWith = collidesWith;
			rvoAgent.Priority = priority;
			float elevation;
			rvoAgent.Position = To2D((ai == null) ? tr.position : ai.position, out elevation);
			if (movementPlane == MovementPlane.XZ)
			{
				rvoAgent.Height = height;
				rvoAgent.ElevationCoordinate = elevation + center - 0.5f * height;
			}
			else
			{
				rvoAgent.Height = 1f;
				rvoAgent.ElevationCoordinate = 0f;
			}
		}

		public void SetTarget(Vector3 pos, float speed, float maxSpeed)
		{
			if (simulator != null)
			{
				rvoAgent.SetTarget(To2D(pos), speed, maxSpeed);
				if (lockWhenNotMoving)
				{
					locked = speed < 0.001f;
				}
			}
		}

		public void Move(Vector3 vel)
		{
			if (simulator != null)
			{
				Vector2 vector = To2D(vel);
				float magnitude = vector.magnitude;
				rvoAgent.SetTarget(To2D((ai == null) ? tr.position : ai.position) + vector, magnitude, magnitude);
				if (lockWhenNotMoving)
				{
					locked = magnitude < 0.001f;
				}
			}
		}

		[Obsolete("Use transform.position instead, the RVOController can now handle that without any issues.")]
		public void Teleport(Vector3 pos)
		{
			tr.position = pos;
		}

		private void OnDrawGizmos()
		{
			Color color = GizmoColor * ((!locked) ? 1f : 0.5f);
			Vector3 vector = ((ai == null) ? base.transform.position : ai.position);
			if (movementPlane == MovementPlane.XY)
			{
				Draw.Gizmos.Cylinder(vector, Vector3.forward, 0f, radius, color);
			}
			else
			{
				Draw.Gizmos.Cylinder(vector + To3D(Vector2.zero, center - height * 0.5f), To3D(Vector2.zero, 1f), height, radius, color);
			}
		}
	}
}
