using UnityEngine;

namespace Pathfinding
{
	public class FleePath : RandomPath
	{
		public static FleePath Construct(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback = null)
		{
			FleePath fleePath = PathPool.GetPath<FleePath>();
			fleePath.Setup(start, avoid, searchLength, callback);
			return fleePath;
		}

		protected void Setup(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback)
		{
			Setup(start, searchLength, callback);
			aim = avoid - start;
			aim *= 10f;
			aim = start - aim;
		}
	}
}
