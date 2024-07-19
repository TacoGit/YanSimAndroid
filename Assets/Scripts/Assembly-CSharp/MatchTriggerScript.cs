using UnityEngine;

public class MatchTriggerScript : MonoBehaviour
{
	public PickUpScript PickUp;

	public StudentScript Student;

	public bool Fireball;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != 9)
		{
			return;
		}
		Student = other.gameObject.GetComponent<StudentScript>();
		if (Student == null)
		{
			GameObject gameObject = other.gameObject.transform.root.gameObject;
			Student = gameObject.GetComponent<StudentScript>();
		}
		if (Student != null && (Student.Gas || Fireball))
		{
			Student.Combust();
			if (PickUp != null && PickUp.Yandere.PickUp != null && PickUp.Yandere.PickUp == PickUp)
			{
				PickUp.Yandere.TargetStudent = Student;
				PickUp.Yandere.MurderousActionTimer = 1f;
			}
			if (Fireball)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
