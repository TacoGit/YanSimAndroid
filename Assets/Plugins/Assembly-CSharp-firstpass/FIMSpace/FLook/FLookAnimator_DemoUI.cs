using UnityEngine;
using UnityEngine.UI;

namespace FIMSpace.FLook
{
	public class FLookAnimator_DemoUI : MonoBehaviour
	{
		public Toggle headSpine;

		public Slider rotationSpeed;

		public Slider animationBlend;

		public Toggle rotAsOffsets;

		public Toggle anchor;

		public Slider compensation;

		[Space(10f)]
		public FLookAnimator headLook;

		public FLookAnimator spineLook;

		private bool switchingComponents;

		private void Update()
		{
			if (switchingComponents)
			{
				return;
			}
			if (headSpine.isOn)
			{
				if (!headLook.enabled)
				{
					headLook.SwitchLooking(true);
					spineLook.SwitchLooking(false, 0.2f, SwitchingFinished);
					headLook.enabled = true;
					switchingComponents = true;
				}
			}
			else if (!spineLook.enabled)
			{
				spineLook.SwitchLooking(true);
				headLook.SwitchLooking(false, 0.2f, SwitchingFinished);
				spineLook.enabled = true;
				switchingComponents = true;
			}
			FLookAnimator fLookAnimator = ((!headSpine.isOn) ? spineLook : headLook);
			fLookAnimator.RotationSpeed = rotationSpeed.value;
			fLookAnimator.BlendToOriginal = animationBlend.value;
			fLookAnimator.AnimateWithSource = rotAsOffsets.isOn;
			fLookAnimator.AnchorReferencePoint = anchor.isOn;
			fLookAnimator.CompensationWeight = compensation.value;
		}

		private void SwitchingFinished()
		{
			switchingComponents = false;
			if (headSpine.isOn)
			{
				spineLook.enabled = false;
			}
			else
			{
				headLook.enabled = false;
			}
		}
	}
}
