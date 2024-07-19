using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_door_controller.php")]
	public class DoorController : MonoBehaviour
	{
		private bool open;

		public int opentag = 1;

		public int closedtag = 1;

		public bool updateGraphsWithGUO = true;

		public float yOffset = 5f;

		private Bounds bounds;

		public void Start()
		{
			bounds = GetComponent<Collider>().bounds;
			SetState(open);
		}

		private void OnGUI()
		{
			if (GUI.Button(new Rect(5f, yOffset, 100f, 22f), "Toggle Door"))
			{
				SetState(!open);
			}
		}

		public void SetState(bool open)
		{
			this.open = open;
			if (updateGraphsWithGUO)
			{
				GraphUpdateObject graphUpdateObject = new GraphUpdateObject(bounds);
				int num = ((!open) ? closedtag : opentag);
				if (num > 31)
				{
					Debug.LogError("tag > 31");
					return;
				}
				graphUpdateObject.modifyTag = true;
				graphUpdateObject.setTag = num;
				graphUpdateObject.updatePhysics = false;
				AstarPath.active.UpdateGraphs(graphUpdateObject);
			}
			if (open)
			{
				GetComponent<Animation>().Play("Open");
			}
			else
			{
				GetComponent<Animation>().Play("Close");
			}
		}
	}
}
