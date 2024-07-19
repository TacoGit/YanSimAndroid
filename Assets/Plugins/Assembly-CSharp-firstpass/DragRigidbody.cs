using System.Collections;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
	public float maxDistance = 100f;

	public float spring = 50f;

	public float damper = 5f;

	public float drag = 10f;

	public float angularDrag = 5f;

	public float distance = 0.2f;

	public bool attachToCenterOfMass;

	private SpringJoint springJoint;

	private void Update()
	{
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		Camera camera = FindCamera();
		RaycastHit hitInfo;
		if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hitInfo, maxDistance) && (bool)hitInfo.rigidbody && !hitInfo.rigidbody.isKinematic)
		{
			if (!springJoint)
			{
				GameObject gameObject = new GameObject("Rigidbody dragger");
				Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
				rigidbody.isKinematic = true;
				springJoint = gameObject.AddComponent<SpringJoint>();
			}
			springJoint.transform.position = hitInfo.point;
			if (attachToCenterOfMass)
			{
				Vector3 position = base.transform.TransformDirection(hitInfo.rigidbody.centerOfMass) + hitInfo.rigidbody.transform.position;
				position = springJoint.transform.InverseTransformPoint(position);
				springJoint.anchor = position;
			}
			else
			{
				springJoint.anchor = Vector3.zero;
			}
			springJoint.spring = spring;
			springJoint.damper = damper;
			springJoint.maxDistance = distance;
			springJoint.connectedBody = hitInfo.rigidbody;
			StartCoroutine(DragObject(hitInfo.distance));
		}
	}

	private IEnumerator DragObject(float distance)
	{
		float oldDrag = springJoint.connectedBody.drag;
		float oldAngularDrag = springJoint.connectedBody.angularDrag;
		springJoint.connectedBody.drag = drag;
		springJoint.connectedBody.angularDrag = angularDrag;
		Camera cam = FindCamera();
		while (Input.GetMouseButton(0))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			springJoint.transform.position = ray.GetPoint(distance);
			yield return null;
		}
		if ((bool)springJoint.connectedBody)
		{
			springJoint.connectedBody.drag = oldDrag;
			springJoint.connectedBody.angularDrag = oldAngularDrag;
			springJoint.connectedBody = null;
		}
	}

	private Camera FindCamera()
	{
		if ((bool)GetComponent<Camera>())
		{
			return GetComponent<Camera>();
		}
		return Camera.main;
	}
}
