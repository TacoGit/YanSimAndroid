using UnityEngine;

public class ObstacleDetectorScript : MonoBehaviour
{
	public YandereScript Yandere;

	public GameObject ControllerX;

	public GameObject KeyboardX;

	public Collider[] ObstacleArray;

	public int Obstacles;

	public bool Add;

	public int ID;

	private void Start()
	{
		ControllerX.SetActive(false);
		KeyboardX.SetActive(false);
	}
}
