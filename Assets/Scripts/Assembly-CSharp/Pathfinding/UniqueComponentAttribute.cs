using System;

namespace Pathfinding
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class UniqueComponentAttribute : Attribute
	{
		public string tag;
	}
}
