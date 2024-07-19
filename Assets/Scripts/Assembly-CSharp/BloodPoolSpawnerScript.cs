using UnityEngine;
using UnityEngine.SceneManagement;

public class BloodPoolSpawnerScript : MonoBehaviour
{
	public RagdollScript Ragdoll;

	public GameObject LastBloodPool;

	public GameObject BloodPool;

	public Transform BloodParent;

	public Transform Hips;

	public Collider MyCollider;

	public Collider GardenArea;

	public Collider NEStairs;

	public Collider NWStairs;

	public Collider SEStairs;

	public Collider SWStairs;

	public Vector3[] Positions;

	public bool CanSpawn;

	public int PoolsSpawned;

	public int NearbyBlood;

	public float Height;

	public float Timer;

	public void Start()
	{
		if (SceneManager.GetActiveScene().name == "SchoolScene")
		{
			GardenArea = GameObject.Find("GardenArea").GetComponent<Collider>();
			NEStairs = GameObject.Find("NEStairs").GetComponent<Collider>();
			NWStairs = GameObject.Find("NWStairs").GetComponent<Collider>();
			SEStairs = GameObject.Find("SEStairs").GetComponent<Collider>();
			SWStairs = GameObject.Find("SWStairs").GetComponent<Collider>();
		}
		BloodParent = GameObject.Find("BloodParent").transform;
		Positions = new Vector3[5];
		Positions[0] = Vector3.zero;
		Positions[1] = new Vector3(0.5f, 0.012f, 0f);
		Positions[2] = new Vector3(-0.5f, 0.012f, 0f);
		Positions[3] = new Vector3(0f, 0.012f, 0.5f);
		Positions[4] = new Vector3(0f, 0.012f, -0.5f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "BloodPool(Clone)")
		{
			LastBloodPool = other.gameObject;
			NearbyBlood++;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == "BloodPool(Clone)")
		{
			NearbyBlood--;
		}
	}

	private void Update()
	{
		if (!MyCollider.enabled)
		{
			return;
		}
		if (Timer > 0f)
		{
			Timer -= Time.deltaTime;
		}
		SetHeight();
		Vector3 position = base.transform.position;
		if (SceneManager.GetActiveScene().name == "SchoolScene")
		{
			CanSpawn = !GardenArea.bounds.Contains(position) && !NEStairs.bounds.Contains(position) && !NWStairs.bounds.Contains(position) && !SEStairs.bounds.Contains(position) && !SWStairs.bounds.Contains(position);
		}
		else
		{
			CanSpawn = true;
		}
		if (!CanSpawn || !(position.y < Height + 1f / 3f))
		{
			return;
		}
		if (NearbyBlood > 0 && LastBloodPool == null)
		{
			NearbyBlood--;
		}
		if (NearbyBlood >= 1 || !(Timer <= 0f))
		{
			return;
		}
		Timer = 0.1f;
		if (PoolsSpawned < 10)
		{
			GameObject gameObject = Object.Instantiate(BloodPool, new Vector3(position.x, Height + 0.012f, position.z), Quaternion.identity);
			gameObject.transform.localEulerAngles = new Vector3(90f, Random.Range(0f, 360f), 0f);
			gameObject.transform.parent = BloodParent;
			PoolsSpawned++;
		}
		else if (PoolsSpawned < 20)
		{
			GameObject gameObject2 = Object.Instantiate(BloodPool, new Vector3(position.x, Height + 0.012f, position.z), Quaternion.identity);
			gameObject2.transform.localEulerAngles = new Vector3(90f, Random.Range(0f, 360f), 0f);
			gameObject2.transform.parent = BloodParent;
			PoolsSpawned++;
			gameObject2.GetComponent<BloodPoolScript>().TargetSize = 1f - (float)(PoolsSpawned - 10) * 0.1f;
			if (PoolsSpawned == 20)
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	public void SpawnBigPool()
	{
		SetHeight();
		Vector3 vector = new Vector3(Hips.position.x, Height + 0.012f, Hips.position.z);
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = Object.Instantiate(BloodPool, vector + Positions[i], Quaternion.identity);
			gameObject.transform.localEulerAngles = new Vector3(90f, Random.Range(0f, 360f), 0f);
			gameObject.transform.parent = BloodParent;
		}
	}

	private void SpawnRow(Transform Location)
	{
		Vector3 position = Location.position;
		Vector3 forward = Location.forward;
		GameObject gameObject = Object.Instantiate(BloodPool, position + forward * 2f, Quaternion.identity);
		gameObject.transform.localEulerAngles = new Vector3(90f, Random.Range(0f, 360f), 0f);
		gameObject.transform.parent = BloodParent;
		gameObject = Object.Instantiate(BloodPool, position + forward * 2.5f, Quaternion.identity);
		gameObject.transform.localEulerAngles = new Vector3(90f, Random.Range(0f, 360f), 0f);
		gameObject.transform.parent = BloodParent;
		gameObject = Object.Instantiate(BloodPool, position + forward * 3f, Quaternion.identity);
		gameObject.transform.localEulerAngles = new Vector3(90f, Random.Range(0f, 360f), 0f);
		gameObject.transform.parent = BloodParent;
	}

	public void SpawnPool(Transform Location)
	{
		GameObject gameObject = Object.Instantiate(BloodPool, Location.position + Location.forward, Quaternion.identity);
		gameObject.transform.localEulerAngles = new Vector3(90f, Random.Range(0f, 360f), 0f);
		gameObject.transform.parent = BloodParent;
	}

	private void SetHeight()
	{
		float y = base.transform.position.y;
		if (y < 4f)
		{
			Height = 0f;
		}
		else if (y < 8f)
		{
			Height = 4f;
		}
		else if (y < 12f)
		{
			Height = 8f;
		}
		else
		{
			Height = 12f;
		}
	}
}
