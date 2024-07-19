using Pathfinding;
using UnityEngine;

public class BloodCleanerScript : MonoBehaviour
{
	public Transform BloodParent;

	public PromptScript Prompt;

	public AIPath Pathfinding;

	public GameObject Lens;

	public UILabel Label;

	public float Distance;

	public float Blood;

	private void Start()
	{
		Physics.IgnoreLayerCollision(11, 15, true);
	}

	private void Update()
	{
		if (!(Blood < 100f) || BloodParent.childCount <= 0)
		{
			return;
		}
		Pathfinding.target = BloodParent.GetChild(0);
		Pathfinding.speed = 1f;
		if (Pathfinding.target.position.y < 4f)
		{
			Label.text = "1";
		}
		else if (Pathfinding.target.position.y < 8f)
		{
			Label.text = "2";
		}
		else if (Pathfinding.target.position.y < 12f)
		{
			Label.text = "3";
		}
		else
		{
			Label.text = "R";
		}
		if (!(Pathfinding.target != null))
		{
			return;
		}
		Distance = Vector3.Distance(base.transform.position, Pathfinding.target.position);
		if (Distance < 0.45f)
		{
			Pathfinding.speed = 0f;
			Transform child = BloodParent.GetChild(0);
			if (child.GetComponent("BloodPoolScript") != null)
			{
				child.localScale = new Vector3(child.localScale.x - Time.deltaTime, child.localScale.y - Time.deltaTime, child.localScale.z);
				Blood += Time.deltaTime;
				if (Blood >= 100f)
				{
					Lens.SetActive(true);
				}
				if (child.transform.localScale.x < 0.1f)
				{
					Object.Destroy(child.gameObject);
				}
			}
			else
			{
				Object.Destroy(child.gameObject);
			}
		}
		else
		{
			Pathfinding.speed = 1f;
		}
	}
}
