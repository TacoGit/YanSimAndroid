using UnityEngine;

namespace FIMSpace.FLook
{
	public class FLookAnimator_Demo_TriggeredMomentTarget : MonoBehaviour
	{
		public float timeOfLooking = 3f;

		public Vector3 offset = Vector3.up;

		public GameObject generatedFollow;

		private Renderer rend;

		private void Start()
		{
			rend = GetComponent<Renderer>();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				FLookAnimator component = other.gameObject.GetComponent<FLookAnimator>();
				if ((bool)component)
				{
					generatedFollow = component.SetMomentLookTarget(base.transform, offset, timeOfLooking);
				}
			}
		}

		private void Update()
		{
			if ((bool)generatedFollow)
			{
				rend.material.color = Color.Lerp(rend.material.color, Color.green, Time.deltaTime * 6f);
			}
			else
			{
				rend.material.color = Color.Lerp(rend.material.color, Color.gray, Time.deltaTime * 6f);
			}
		}
	}
}
