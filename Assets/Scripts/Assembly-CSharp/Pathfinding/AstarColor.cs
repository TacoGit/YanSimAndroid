using System;
using UnityEngine;

namespace Pathfinding
{
	[Serializable]
	public class AstarColor
	{
		public Color _NodeConnection;

		public Color _UnwalkableNode;

		public Color _BoundsHandles;

		public Color _ConnectionLowLerp;

		public Color _ConnectionHighLerp;

		public Color _MeshEdgeColor;

		public Color[] _AreaColors;

		public static Color NodeConnection = new Color(1f, 1f, 1f, 0.9f);

		public static Color UnwalkableNode = new Color(1f, 0f, 0f, 0.5f);

		public static Color BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);

		public static Color ConnectionLowLerp = new Color(0f, 1f, 0f, 0.5f);

		public static Color ConnectionHighLerp = new Color(1f, 0f, 0f, 0.5f);

		public static Color MeshEdgeColor = new Color(0f, 0f, 0f, 0.5f);

		private static Color[] AreaColors;

		public AstarColor()
		{
			_NodeConnection = new Color(1f, 1f, 1f, 0.9f);
			_UnwalkableNode = new Color(1f, 0f, 0f, 0.5f);
			_BoundsHandles = new Color(0.29f, 0.454f, 0.741f, 0.9f);
			_ConnectionLowLerp = new Color(0f, 1f, 0f, 0.5f);
			_ConnectionHighLerp = new Color(1f, 0f, 0f, 0.5f);
			_MeshEdgeColor = new Color(0f, 0f, 0f, 0.5f);
		}

		public static Color GetAreaColor(uint area)
		{
			if (AreaColors == null || area >= AreaColors.Length)
			{
				return AstarMath.IntToColor((int)area, 1f);
			}
			return AreaColors[area];
		}

		public void OnEnable()
		{
			NodeConnection = _NodeConnection;
			UnwalkableNode = _UnwalkableNode;
			BoundsHandles = _BoundsHandles;
			ConnectionLowLerp = _ConnectionLowLerp;
			ConnectionHighLerp = _ConnectionHighLerp;
			MeshEdgeColor = _MeshEdgeColor;
			AreaColors = _AreaColors;
		}
	}
}
