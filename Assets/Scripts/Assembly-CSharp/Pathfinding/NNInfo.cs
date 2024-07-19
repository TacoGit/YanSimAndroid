using System;
using UnityEngine;

namespace Pathfinding
{
	public struct NNInfo
	{
		public readonly GraphNode node;

		public readonly Vector3 position;

		[Obsolete("This field has been renamed to 'position'")]
		public Vector3 clampedPosition
		{
			get
			{
				return position;
			}
		}

		public NNInfo(NNInfoInternal internalInfo)
		{
			node = internalInfo.node;
			position = internalInfo.clampedPosition;
		}

		public static explicit operator Vector3(NNInfo ob)
		{
			return ob.position;
		}

		public static explicit operator GraphNode(NNInfo ob)
		{
			return ob.node;
		}
	}
}
