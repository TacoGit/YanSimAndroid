using UnityEngine;

public class DoorOpenerScript : MonoBehaviour
{
	public StudentScript Student;

	public DoorScript Door;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 9)
		{
			Student = other.gameObject.GetComponent<StudentScript>();
			if (Student != null && !Student.Dying && !Door.Open && !Door.Locked)
			{
				Door.Student = Student;
				Door.OpenDoor();
			}
		}
	}
}
