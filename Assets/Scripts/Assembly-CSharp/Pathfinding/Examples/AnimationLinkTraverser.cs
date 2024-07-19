using System;
using System.Collections;
using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_animation_link_traverser.php")]
	public class AnimationLinkTraverser : VersionedMonoBehaviour
	{
		public Animation anim;

		private RichAI ai;

		private void OnEnable()
		{
			ai = GetComponent<RichAI>();
			if (ai != null)
			{
				RichAI richAI = ai;
				richAI.onTraverseOffMeshLink = (Func<RichSpecial, IEnumerator>)Delegate.Combine(richAI.onTraverseOffMeshLink, new Func<RichSpecial, IEnumerator>(TraverseOffMeshLink));
			}
		}

		private void OnDisable()
		{
			if (ai != null)
			{
				RichAI richAI = ai;
				richAI.onTraverseOffMeshLink = (Func<RichSpecial, IEnumerator>)Delegate.Remove(richAI.onTraverseOffMeshLink, new Func<RichSpecial, IEnumerator>(TraverseOffMeshLink));
			}
		}

		protected virtual IEnumerator TraverseOffMeshLink(RichSpecial rs)
		{
			AnimationLink link = rs.nodeLink as AnimationLink;
			if (link == null)
			{
				Debug.LogError("Unhandled RichSpecial");
				yield break;
			}
			while (true)
			{
				Quaternion origRotation = ai.rotation;
				Quaternion finalRotation = ai.SimulateRotationTowards(rs.first.forward, ai.rotationSpeed * Time.deltaTime);
				if (origRotation == finalRotation)
				{
					break;
				}
				ai.FinalizeMovement(ai.position, finalRotation);
				yield return null;
			}
			base.transform.parent.position = base.transform.position;
			base.transform.parent.rotation = base.transform.rotation;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			if (rs.reverse && link.reverseAnim)
			{
				anim[link.clip].speed = 0f - link.animSpeed;
				anim[link.clip].normalizedTime = 1f;
				anim.Play(link.clip);
				anim.Sample();
			}
			else
			{
				anim[link.clip].speed = link.animSpeed;
				anim.Rewind(link.clip);
				anim.Play(link.clip);
			}
			base.transform.parent.position -= base.transform.position - base.transform.parent.position;
			yield return new WaitForSeconds(Mathf.Abs(anim[link.clip].length / link.animSpeed));
		}
	}
}
