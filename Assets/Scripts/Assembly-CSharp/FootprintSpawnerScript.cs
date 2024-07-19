using UnityEngine;

public class FootprintSpawnerScript : MonoBehaviour
{
	public YandereScript Yandere;

	public GameObject BloodyFootprint;

	public AudioClip[] WalkFootsteps;

	public AudioClip[] RunFootsteps;

	public Transform BloodParent;

	public Collider GardenArea;

	public Collider PoolStairs;

	public Collider NEStairs;

	public Collider NWStairs;

	public Collider SEStairs;

	public Collider SWStairs;

	public bool Debugging;

	public bool CanSpawn;

	public bool FootUp;

	public float DownThreshold;

	public float UpThreshold;

	public float Height;

	public int Bloodiness;

	public int Collisions;

	private void Start()
	{
		GardenArea = GameObject.Find("GardenArea").GetComponent<Collider>();
		PoolStairs = GameObject.Find("PoolStairs").GetComponent<Collider>();
		NEStairs = GameObject.Find("NEStairs").GetComponent<Collider>();
		NWStairs = GameObject.Find("NWStairs").GetComponent<Collider>();
		SEStairs = GameObject.Find("SEStairs").GetComponent<Collider>();
		SWStairs = GameObject.Find("SWStairs").GetComponent<Collider>();
	}

	private void Update()
	{
		if (Debugging)
		{
			Debug.Log("UpThreshold: " + (Yandere.transform.position.y + UpThreshold) + " | DownThreshold: " + (Yandere.transform.position.y + DownThreshold) + " | CurrentHeight: " + base.transform.position.y);
		}
		CanSpawn = !GardenArea.bounds.Contains(base.transform.position) && !PoolStairs.bounds.Contains(base.transform.position) && !NEStairs.bounds.Contains(base.transform.position) && !NWStairs.bounds.Contains(base.transform.position) && !SEStairs.bounds.Contains(base.transform.position) && !SWStairs.bounds.Contains(base.transform.position);
		if (!FootUp)
		{
			if (base.transform.position.y > Yandere.transform.position.y + UpThreshold)
			{
				FootUp = true;
			}
		}
		else
		{
			if (!(base.transform.position.y < Yandere.transform.position.y + DownThreshold))
			{
				return;
			}
			if (Yandere.Stance.Current != StanceType.Crouching && Yandere.Stance.Current != StanceType.Crawling && Yandere.CanMove && !Yandere.NearSenpai && FootUp)
			{
				AudioSource component = GetComponent<AudioSource>();
				if (Input.GetButton("LB"))
				{
					component.clip = RunFootsteps[Random.Range(0, RunFootsteps.Length)];
					component.volume = 0.2f;
					component.Play();
				}
				else
				{
					component.clip = WalkFootsteps[Random.Range(0, WalkFootsteps.Length)];
					component.volume = 0.1f;
					component.Play();
				}
			}
			FootUp = false;
			if (CanSpawn && Bloodiness > 0)
			{
				if (base.transform.position.y > -1f && base.transform.position.y < 1f)
				{
					Height = 0f;
				}
				else if (base.transform.position.y > 3f && base.transform.position.y < 5f)
				{
					Height = 4f;
				}
				else if (base.transform.position.y > 7f && base.transform.position.y < 9f)
				{
					Height = 8f;
				}
				else if (base.transform.position.y > 11f && base.transform.position.y < 13f)
				{
					Height = 12f;
				}
				GameObject gameObject = Object.Instantiate(BloodyFootprint, new Vector3(base.transform.position.x, Height + 0.012f, base.transform.position.z), Quaternion.identity);
				gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, base.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
				gameObject.transform.GetChild(0).GetComponent<FootprintScript>().Yandere = Yandere;
				gameObject.transform.parent = BloodParent;
				Bloodiness--;
			}
		}
	}
}
