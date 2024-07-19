using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
	public interface IAgent
	{
		Vector2 Position { get; set; }

		float ElevationCoordinate { get; set; }

		Vector2 CalculatedTargetPoint { get; }

		float CalculatedSpeed { get; }

		bool Locked { get; set; }

		float Radius { get; set; }

		float Height { get; set; }

		float AgentTimeHorizon { get; set; }

		float ObstacleTimeHorizon { get; set; }

		int MaxNeighbours { get; set; }

		int NeighbourCount { get; }

		RVOLayer Layer { get; set; }

		RVOLayer CollidesWith { get; set; }

		bool DebugDraw { get; set; }

		[Obsolete]
		List<ObstacleVertex> NeighbourObstacles { get; }

		float Priority { get; set; }

		Action PreCalculationCallback { set; }

		void SetTarget(Vector2 targetPoint, float desiredSpeed, float maxSpeed);

		void SetCollisionNormal(Vector2 normal);

		void ForceSetVelocity(Vector2 velocity);
	}
}
