using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_object_placer.php")]
	public class ObjectPlacer : MonoBehaviour
	{
		public GameObject go;

		public bool direct;

		public bool issueGUOs = true;

		private void Update()
		{
			if (Input.GetKeyDown("p"))
			{
				PlaceObject();
			}
			if (Input.GetKeyDown("r"))
			{
				RemoveObject();
			}
		}

		public void PlaceObject()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, float.PositiveInfinity))
			{
				return;
			}
			Vector3 point = hitInfo.point;
			GameObject gameObject = Object.Instantiate(go, point, Quaternion.identity);
			if (issueGUOs)
			{
				Bounds bounds = gameObject.GetComponent<Collider>().bounds;
				GraphUpdateObject ob = new GraphUpdateObject(bounds);
				AstarPath.active.UpdateGraphs(ob);
				if (direct)
				{
					AstarPath.active.FlushGraphUpdates();
				}
			}
		}

		public void RemoveObject()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, float.PositiveInfinity) || hitInfo.collider.isTrigger || hitInfo.transform.gameObject.name == "Ground")
			{
				return;
			}
			Bounds bounds = hitInfo.collider.bounds;
			Object.Destroy(hitInfo.collider);
			Object.Destroy(hitInfo.collider.gameObject);
			if (issueGUOs)
			{
				GraphUpdateObject ob = new GraphUpdateObject(bounds);
				AstarPath.active.UpdateGraphs(ob);
				if (direct)
				{
					AstarPath.active.FlushGraphUpdates();
				}
			}
		}
	}
}
