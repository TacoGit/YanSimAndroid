using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_animation_link.php")]
	public class AnimationLink : NodeLink2
	{
		[Serializable]
		public class LinkClip
		{
			public AnimationClip clip;

			public Vector3 velocity;

			public int loopCount = 1;

			public string name
			{
				get
				{
					return (!(clip != null)) ? string.Empty : clip.name;
				}
			}
		}

		public string clip;

		public float animSpeed = 1f;

		public bool reverseAnim = true;

		public GameObject referenceMesh;

		public LinkClip[] sequence;

		public string boneRoot = "bn_COG_Root";

		private static Transform SearchRec(Transform tr, string name)
		{
			int childCount = tr.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = tr.GetChild(i);
				if (child.name == name)
				{
					return child;
				}
				Transform transform = SearchRec(child, name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		public void CalculateOffsets(List<Vector3> trace, out Vector3 endPosition)
		{
			endPosition = base.transform.position;
			if (referenceMesh == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(referenceMesh, base.transform.position, base.transform.rotation);
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			Transform transform = SearchRec(gameObject.transform, boneRoot);
			if (transform == null)
			{
				throw new Exception("Could not find root transform");
			}
			Animation animation = gameObject.GetComponent<Animation>();
			if (animation == null)
			{
				animation = gameObject.AddComponent<Animation>();
			}
			for (int i = 0; i < sequence.Length; i++)
			{
				animation.AddClip(sequence[i].clip, sequence[i].clip.name);
			}
			Vector3 vector = Vector3.zero;
			Vector3 position = base.transform.position;
			Vector3 vector2 = Vector3.zero;
			for (int j = 0; j < sequence.Length; j++)
			{
				LinkClip linkClip = sequence[j];
				if (linkClip == null)
				{
					endPosition = position;
					return;
				}
				animation[linkClip.clip.name].enabled = true;
				animation[linkClip.clip.name].weight = 1f;
				for (int k = 0; k < linkClip.loopCount; k++)
				{
					animation[linkClip.clip.name].normalizedTime = 0f;
					animation.Sample();
					Vector3 vector3 = transform.position - base.transform.position;
					if (j > 0)
					{
						position += vector - vector3;
					}
					else
					{
						vector2 = vector3;
					}
					for (int l = 0; l <= 20; l++)
					{
						float num = (float)l / 20f;
						animation[linkClip.clip.name].normalizedTime = num;
						animation.Sample();
						Vector3 item = position + (transform.position - base.transform.position) + linkClip.velocity * num * linkClip.clip.length;
						trace.Add(item);
					}
					position += linkClip.velocity * 1f * linkClip.clip.length;
					animation[linkClip.clip.name].normalizedTime = 1f;
					animation.Sample();
					Vector3 vector4 = transform.position - base.transform.position;
					vector = vector4;
				}
				animation[linkClip.clip.name].enabled = false;
				animation[linkClip.clip.name].weight = 0f;
			}
			position += vector - vector2;
			UnityEngine.Object.DestroyImmediate(gameObject);
			endPosition = position;
		}

		public override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();
			List<Vector3> list = ListPool<Vector3>.Claim();
			Vector3 endPosition = Vector3.zero;
			CalculateOffsets(list, out endPosition);
			Gizmos.color = Color.blue;
			for (int i = 0; i < list.Count - 1; i++)
			{
				Gizmos.DrawLine(list[i], list[i + 1]);
			}
		}
	}
}
