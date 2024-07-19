using FIMSpace.Basics;
using UnityEngine;

namespace FIMSpace.Audio
{
	public class FBasic_FheelekFighter : FBasic_FheelekController
	{
		private AudioSource HitSource;

		public AudioClip SwingAudioClip;

		protected override void Start()
		{
			base.Start();
			fheelekAnimator.SetDefaultIdle("Idle Hammer");
			fheelekAnimator.SetDefaultRun("Idle Hammer");
			HitSource = base.gameObject.GetComponent<AudioSource>();
		}

		protected override void Update()
		{
			base.Update();
			if (Input.GetMouseButtonDown(0))
			{
				fheelekAnimator.PlayAnimationHoldUntilIdle("Attack Hammer", 0.15f);
			}
		}

		public void ESwing()
		{
			HitSource.PlayOneShot(SwingAudioClip, 0.9f);
		}

		public void EHit()
		{
		}
	}
}
