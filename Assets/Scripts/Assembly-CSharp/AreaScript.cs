using System.Collections.Generic;
using UnityEngine;

public class AreaScript : MonoBehaviour
{
	[Header("Do not touch any of these values. They get updated at runtime.")]
	[Tooltip("The amount of students in this area.")]
	public int Population;

	[Tooltip("A list of students in this area.")]
	public List<StudentScript> Students;

	[Tooltip("This area's crowd. Students will go here.")]
	public List<StudentScript> Crowd;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Student"))
		{
			StudentScript component = other.GetComponent<StudentScript>();
			Students.Add(component);
			Population++;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Student"))
		{
			StudentScript component = other.GetComponent<StudentScript>();
			Students.Remove(component);
			Population--;
		}
	}
}
