using System.Collections.Generic;
using Pathfinding.RVO;
using UnityEngine;

namespace Pathfinding.Examples
{
	[RequireComponent(typeof(RVOController))]
	[RequireComponent(typeof(Seeker))]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_r_v_o_example_agent.php")]
	public class RVOExampleAgent : MonoBehaviour
	{
		public float repathRate = 1f;

		private float nextRepath;

		private Vector3 target;

		private bool canSearchAgain = true;

		private RVOController controller;

		public float maxSpeed = 10f;

		private Path path;

		private List<Vector3> vectorPath;

		private int wp;

		public float moveNextDist = 1f;

		public float slowdownDistance = 1f;

		public LayerMask groundMask;

		private Seeker seeker;

		private MeshRenderer[] rends;

		public void Awake()
		{
			seeker = GetComponent<Seeker>();
			controller = GetComponent<RVOController>();
		}

		public void SetTarget(Vector3 target)
		{
			this.target = target;
			RecalculatePath();
		}

		public void SetColor(Color color)
		{
			if (rends == null)
			{
				rends = GetComponentsInChildren<MeshRenderer>();
			}
			MeshRenderer[] array = rends;
			foreach (MeshRenderer meshRenderer in array)
			{
				Color color2 = meshRenderer.material.GetColor("_TintColor");
				AnimationCurve curve = AnimationCurve.Linear(0f, color2.r, 1f, color.r);
				AnimationCurve curve2 = AnimationCurve.Linear(0f, color2.g, 1f, color.g);
				AnimationCurve curve3 = AnimationCurve.Linear(0f, color2.b, 1f, color.b);
				AnimationClip animationClip = new AnimationClip();
				animationClip.legacy = true;
				animationClip.SetCurve(string.Empty, typeof(Material), "_TintColor.r", curve);
				animationClip.SetCurve(string.Empty, typeof(Material), "_TintColor.g", curve2);
				animationClip.SetCurve(string.Empty, typeof(Material), "_TintColor.b", curve3);
				Animation animation = meshRenderer.gameObject.GetComponent<Animation>();
				if (animation == null)
				{
					animation = meshRenderer.gameObject.AddComponent<Animation>();
				}
				animationClip.wrapMode = WrapMode.Once;
				animation.AddClip(animationClip, "ColorAnim");
				animation.Play("ColorAnim");
			}
		}

		public void RecalculatePath()
		{
			canSearchAgain = false;
			nextRepath = Time.time + repathRate * (Random.value + 0.5f);
			seeker.StartPath(base.transform.position, target, OnPathComplete);
		}

		public void OnPathComplete(Path _p)
		{
			ABPath aBPath = _p as ABPath;
			canSearchAgain = true;
			if (path != null)
			{
				path.Release(this);
			}
			path = aBPath;
			aBPath.Claim(this);
			if (aBPath.error)
			{
				wp = 0;
				vectorPath = null;
				return;
			}
			Vector3 originalStartPoint = aBPath.originalStartPoint;
			Vector3 position = base.transform.position;
			originalStartPoint.y = position.y;
			float magnitude = (position - originalStartPoint).magnitude;
			wp = 0;
			vectorPath = aBPath.vectorPath;
			if (!(moveNextDist > 0f))
			{
				return;
			}
			for (float num = 0f; num <= magnitude; num += moveNextDist * 0.6f)
			{
				wp--;
				Vector3 vector = originalStartPoint + (position - originalStartPoint) * num;
				Vector3 vector2;
				do
				{
					wp++;
					vector2 = vectorPath[wp];
				}
				while (controller.To2D(vector - vector2).sqrMagnitude < moveNextDist * moveNextDist && wp != vectorPath.Count - 1);
			}
		}

		public void Update()
		{
			if (Time.time >= nextRepath && canSearchAgain)
			{
				RecalculatePath();
			}
			Vector3 position = base.transform.position;
			if (vectorPath != null && vectorPath.Count != 0)
			{
				while ((controller.To2D(position - vectorPath[wp]).sqrMagnitude < moveNextDist * moveNextDist && wp != vectorPath.Count - 1) || wp == 0)
				{
					wp++;
				}
				Vector3 vector = vectorPath[wp - 1];
				Vector3 vector2 = vectorPath[wp];
				float value = VectorMath.LineCircleIntersectionFactor(controller.To2D(base.transform.position), controller.To2D(vector), controller.To2D(vector2), moveNextDist);
				value = Mathf.Clamp01(value);
				Vector3 vector3 = Vector3.Lerp(vector, vector2, value);
				float num = controller.To2D(vector3 - position).magnitude + controller.To2D(vector3 - vector2).magnitude;
				for (int i = wp; i < vectorPath.Count - 1; i++)
				{
					num += controller.To2D(vectorPath[i + 1] - vectorPath[i]).magnitude;
				}
				Vector3 pos = (vector3 - position).normalized * num + position;
				float speed = Mathf.Clamp01(num / slowdownDistance) * maxSpeed;
				Debug.DrawLine(base.transform.position, vector3, Color.red);
				controller.SetTarget(pos, speed, maxSpeed);
			}
			else
			{
				controller.SetTarget(position, maxSpeed, maxSpeed);
			}
			Vector3 vector4 = controller.CalculateMovementDelta(Time.deltaTime);
			position += vector4;
			if (Time.deltaTime > 0f && vector4.magnitude / Time.deltaTime > 0.01f)
			{
				Quaternion rotation = base.transform.rotation;
				Quaternion b = Quaternion.LookRotation(vector4, controller.To3D(Vector2.zero, 1f));
				if (controller.movementPlane == MovementPlane.XY)
				{
					b *= Quaternion.Euler(-90f, 180f, 0f);
				}
				base.transform.rotation = Quaternion.Slerp(rotation, b, Time.deltaTime * 5f);
			}
			RaycastHit hitInfo;
			if (controller.movementPlane == MovementPlane.XZ && Physics.Raycast(position + Vector3.up, Vector3.down, out hitInfo, 2f, groundMask))
			{
				position.y = hitInfo.point.y;
			}
			base.transform.position = position;
		}
	}
}
