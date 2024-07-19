using UnityEngine;

public class SimpleDetectClickScript : MonoBehaviour
{
	public InventoryItemScript InventoryItem;

	public Collider MyCollider;

	public bool Clicked;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 100f) && hitInfo.collider == MyCollider)
			{
				Clicked = true;
			}
		}
	}
}
