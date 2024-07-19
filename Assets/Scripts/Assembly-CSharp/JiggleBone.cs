using UnityEngine;

public class JiggleBone : MonoBehaviour
{
	public bool debugMode = true;

	private Vector3 dynamicPos = default(Vector3);

	public Vector3 boneAxis = new Vector3(0f, 0f, 1f);

	public float targetDistance = 2f;

	public float bStiffness = 0.1f;

	public float bMass = 0.9f;

	public float bDamping = 0.75f;

	public float bGravity = 0.75f;

	private Vector3 force = default(Vector3);

	private Vector3 acc = default(Vector3);

	private Vector3 vel = default(Vector3);

	public bool SquashAndStretch = true;

	public float sideStretch = 0.15f;

	public float frontStretch = 0.2f;

	private void Awake()
	{
		Vector3 vector = base.transform.position + base.transform.TransformDirection(boneAxis * targetDistance);
		dynamicPos = vector;
	}

	private void LateUpdate()
	{
		base.transform.rotation = default(Quaternion);
		Vector3 dir = base.transform.TransformDirection(boneAxis * targetDistance);
		Vector3 vector = base.transform.TransformDirection(new Vector3(0f, 1f, 0f));
		Vector3 vector2 = base.transform.position + base.transform.TransformDirection(boneAxis * targetDistance);
		force.x = (vector2.x - dynamicPos.x) * bStiffness;
		acc.x = force.x / bMass;
		vel.x += acc.x * (1f - bDamping);
		force.y = (vector2.y - dynamicPos.y) * bStiffness;
		force.y -= bGravity / 10f;
		acc.y = force.y / bMass;
		vel.y += acc.y * (1f - bDamping);
		force.z = (vector2.z - dynamicPos.z) * bStiffness;
		acc.z = force.z / bMass;
		vel.z += acc.z * (1f - bDamping);
		dynamicPos += vel + force;
		base.transform.LookAt(dynamicPos, vector);
		if (SquashAndStretch)
		{
			float magnitude = (dynamicPos - vector2).magnitude;
			float num = 1f + ((boneAxis.x != 0f) ? (magnitude * frontStretch) : ((0f - magnitude) * sideStretch));
			float num2 = 1f + ((boneAxis.y != 0f) ? (magnitude * frontStretch) : ((0f - magnitude) * sideStretch));
			float num3 = 1f + ((boneAxis.z != 0f) ? (magnitude * frontStretch) : ((0f - magnitude) * sideStretch));
		}
		if (debugMode)
		{
			Debug.DrawRay(base.transform.position, dir, Color.blue);
			Debug.DrawRay(base.transform.position, vector, Color.green);
			Debug.DrawRay(vector2, Vector3.up * 0.2f, Color.yellow);
			Debug.DrawRay(dynamicPos, Vector3.up * 0.2f, Color.red);
		}
	}
}
