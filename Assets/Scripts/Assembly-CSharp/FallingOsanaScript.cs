using UnityEngine;

public class FallingOsanaScript : MonoBehaviour
{
	public StudentScript Osana;

	public GameObject GroundImpact;

	private void Update()
	{
		if (base.transform.parent.position.y > 0f)
		{
			Osana.CharacterAnimation.Play(Osana.IdleAnim);
			base.transform.parent.position += new Vector3(0f, -1.0001f, 0f);
		}
		if (base.transform.parent.position.y < 0f)
		{
			base.transform.parent.position = new Vector3(base.transform.parent.position.x, 0f, base.transform.parent.position.z);
			Object.Instantiate(GroundImpact, base.transform.parent.position, Quaternion.identity);
		}
	}
}
