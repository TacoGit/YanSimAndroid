using System.Linq;
using UnityEngine;

namespace Pathfinding
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_target_mover.php")]
	public class TargetMover : MonoBehaviour
	{
		public LayerMask mask;

		public Transform target;

		private IAstarAI[] ais;

		public bool onlyOnDoubleClick;

		public bool use2D;

		private Camera cam;

		public void Start()
		{
			cam = Camera.main;
			ais = Object.FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();
			base.useGUILayout = false;
		}

		public void OnGUI()
		{
			if (onlyOnDoubleClick && cam != null && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
			{
				UpdateTargetPosition();
			}
		}

		private void Update()
		{
			if (!onlyOnDoubleClick && cam != null)
			{
				UpdateTargetPosition();
			}
		}

		public void UpdateTargetPosition()
		{
			Vector3 vector = Vector3.zero;
			bool flag = false;
			RaycastHit hitInfo;
			if (use2D)
			{
				vector = cam.ScreenToWorldPoint(Input.mousePosition);
				vector.z = 0f;
				flag = true;
			}
			else if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity, mask))
			{
				vector = hitInfo.point;
				flag = true;
			}
			if (!flag || !(vector != target.position))
			{
				return;
			}
			target.position = vector;
			if (!onlyOnDoubleClick)
			{
				return;
			}
			for (int i = 0; i < ais.Length; i++)
			{
				if (ais[i] != null)
				{
					ais[i].SearchPath();
				}
			}
		}
	}
}
