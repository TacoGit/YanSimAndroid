using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_bezier_mover.php")]
	public class BezierMover : MonoBehaviour
	{
		public Transform[] points;

		public float speed = 1f;

		public float tiltAmount = 1f;

		private float time;

		private Vector3 Position(float t)
		{
			int num = points.Length;
			int num2 = Mathf.FloorToInt(t) % num;
			return AstarSplines.CatmullRom(points[(num2 - 1 + num) % num].position, points[num2].position, points[(num2 + 1) % num].position, points[(num2 + 2) % num].position, t - (float)Mathf.FloorToInt(t));
		}

		private void Update()
		{
			float num = time;
			float num2 = time + 1f;
			while (num2 - num > 0.0001f)
			{
				float num3 = (num + num2) / 2f;
				Vector3 vector = Position(num3);
				if ((vector - base.transform.position).sqrMagnitude > speed * Time.deltaTime * (speed * Time.deltaTime))
				{
					num2 = num3;
				}
				else
				{
					num = num3;
				}
			}
			time = (num + num2) / 2f;
			Vector3 vector2 = Position(time);
			Vector3 vector3 = Position(time + 0.001f);
			base.transform.position = vector2;
			Vector3 vector4 = Position(time + 0.15f);
			Vector3 vector5 = Position(time + 0.15f + 0.001f);
			Vector3 vector6 = ((vector5 - vector4).normalized - (vector3 - vector2).normalized) / (vector4 - vector2).magnitude;
			Vector3 upwards = new Vector3(0f, 1f / (tiltAmount + 1E-05f), 0f) + vector6;
			base.transform.rotation = Quaternion.LookRotation(vector3 - vector2, upwards);
		}

		private void OnDrawGizmos()
		{
			if (points.Length < 3)
			{
				return;
			}
			for (int i = 0; i < points.Length; i++)
			{
				if (points[i] == null)
				{
					return;
				}
			}
			Gizmos.color = Color.white;
			Vector3 from = Position(0f);
			for (int j = 0; j < points.Length; j++)
			{
				for (int k = 1; k <= 100; k++)
				{
					Vector3 vector = Position((float)j + (float)k / 100f);
					Gizmos.DrawLine(from, vector);
					from = vector;
				}
			}
		}
	}
}
