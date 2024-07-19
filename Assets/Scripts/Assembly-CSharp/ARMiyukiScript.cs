using UnityEngine;
// FIXED BY TANOS [x]
public class ARMiyukiScript : MonoBehaviour
{
	public Transform BulletSpawnPoint;

	public StudentScript MyStudent;

	public YandereScript Yandere;

	public GameObject Bullet;

	public Transform Enemy;

	public GameObject MagicalGirl;

	public bool Student;

	private void Start()
	{
		if (Enemy == null)
		{
			Enemy = MyStudent.StudentManager.MiyukiCat;
		}
	}

	private void Update()
	{
		if (!Student && Yandere.AR)
		{
			base.transform.LookAt(Enemy.position);
			if (Input.GetButtonDown("X"))
			{
				Shoot();
			}
		}
	}

	public void Shoot()
	{
		if (Enemy == null)
		{
			Enemy = MyStudent.StudentManager.MiyukiCat;
		}
		base.transform.LookAt(Enemy.position);
		Object.Instantiate(Bullet, BulletSpawnPoint.position, base.transform.rotation);
	}
}
