using Pathfinding.RVO;
using UnityEngine;

namespace Pathfinding.Examples
{
	[RequireComponent(typeof(RVOController))]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_manual_r_v_o_agent.php")]
	public class ManualRVOAgent : MonoBehaviour
	{
		private RVOController rvo;

		public float speed = 1f;

		private void Awake()
		{
			rvo = GetComponent<RVOController>();
		}

		private void Update()
		{
			float axis = Input.GetAxis("Horizontal");
			float axis2 = Input.GetAxis("Vertical");
			Vector3 vector = new Vector3(axis, 0f, axis2) * speed;
			rvo.velocity = vector;
			base.transform.position += vector * Time.deltaTime;
		}
	}
}
