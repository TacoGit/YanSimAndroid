using UnityEngine;

public class RiggedAttacher : MonoBehaviour
{
	public Transform BasePelvisRoot;

	public Transform AttachmentPelvisRoot;

	private void Start()
	{
		Attaching(BasePelvisRoot, AttachmentPelvisRoot);
	}

	private void Attaching(Transform Base, Transform Attachment)
	{
		Attachment.transform.SetParent(Base);
		Base.localEulerAngles = Vector3.zero;
		Base.localPosition = Vector3.zero;
		Transform[] componentsInChildren = Base.GetComponentsInChildren<Transform>();
		Transform[] componentsInChildren2 = Attachment.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren2;
		foreach (Transform transform in array)
		{
			Transform[] array2 = componentsInChildren;
			foreach (Transform transform2 in array2)
			{
				if (transform.name == transform2.name)
				{
					transform.SetParent(transform2);
					transform.localEulerAngles = Vector3.zero;
					transform.localPosition = Vector3.zero;
				}
			}
		}
	}
}
