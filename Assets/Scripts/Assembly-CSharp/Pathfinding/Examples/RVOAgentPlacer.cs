using System;
using System.Collections;
using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_r_v_o_agent_placer.php")]
	public class RVOAgentPlacer : MonoBehaviour
	{
		public int agents = 100;

		public float ringSize = 100f;

		public LayerMask mask;

		public GameObject prefab;

		public Vector3 goalOffset;

		public float repathRate = 1f;

		private const float rad2Deg = 180f / (float)Math.PI;

		private IEnumerator Start()
		{
			yield return null;
			for (int i = 0; i < agents; i++)
			{
				float num = (float)i / (float)agents * (float)Math.PI * 2f;
				Vector3 vector = new Vector3((float)Math.Cos(num), 0f, (float)Math.Sin(num)) * ringSize;
				Vector3 target = -vector + goalOffset;
				GameObject gameObject = UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.Euler(0f, num + 180f, 0f));
				RVOExampleAgent component = gameObject.GetComponent<RVOExampleAgent>();
				if (component == null)
				{
					Debug.LogError("Prefab does not have an RVOExampleAgent component attached");
					break;
				}
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = vector;
				component.repathRate = repathRate;
				component.SetTarget(target);
				component.SetColor(GetColor(num));
			}
		}

		public Color GetColor(float angle)
		{
			return AstarMath.HSVToRGB(angle * (180f / (float)Math.PI), 0.8f, 0.6f);
		}
	}
}
