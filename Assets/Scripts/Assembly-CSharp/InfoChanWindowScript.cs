using UnityEngine;

public class InfoChanWindowScript : MonoBehaviour
{
	public Transform DropPoint;

	public GameObject[] Drops;

	public int[] ItemsToDrop;

	public int Orders;

	public int ID;

	public float Rotation;

	public float Timer;

	public bool Dropped;

	public bool Drop;

	public bool Open = true;

	public bool Test;

	private void Update()
	{
		if (Drop)
		{
			Rotation = Mathf.Lerp(Rotation, (!Drop) ? 0f : (-90f), Time.deltaTime * 10f);
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, Rotation, base.transform.localEulerAngles.z);
			Timer += Time.deltaTime;
			if (Timer > 1f)
			{
				if ((float)Orders > 0f)
				{
					Object.Instantiate(Drops[ItemsToDrop[Orders]], DropPoint.position, Quaternion.identity);
					Timer = 0f;
					Orders--;
				}
				else
				{
					Open = false;
					if (Timer > 3f)
					{
						base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, 0f, base.transform.localEulerAngles.z);
						Drop = false;
					}
				}
			}
		}
		if (Test)
		{
			DropObject();
		}
	}

	public void DropObject()
	{
		Rotation = 0f;
		Timer = 0f;
		Dropped = false;
		Test = false;
		Drop = true;
		Open = true;
	}
}
