namespace Pathfinding
{
	public class PathNode
	{
		public GraphNode node;

		public PathNode parent;

		public ushort pathID;

		public ushort heapIndex = ushort.MaxValue;

		private uint flags;

		private const uint CostMask = 268435455u;

		private const int Flag1Offset = 28;

		private const uint Flag1Mask = 268435456u;

		private const int Flag2Offset = 29;

		private const uint Flag2Mask = 536870912u;

		private uint g;

		private uint h;

		public uint cost
		{
			get
			{
				return flags & 0xFFFFFFFu;
			}
			set
			{
				flags = (flags & 0xF0000000u) | value;
			}
		}

		public bool flag1
		{
			get
			{
				return (flags & 0x10000000) != 0;
			}
			set
			{
				flags = (flags & 0xEFFFFFFFu) | (value ? 268435456u : 0u);
			}
		}

		public bool flag2
		{
			get
			{
				return (flags & 0x20000000) != 0;
			}
			set
			{
				flags = (flags & 0xDFFFFFFFu) | (value ? 536870912u : 0u);
			}
		}

		public uint G
		{
			get
			{
				return g;
			}
			set
			{
				g = value;
			}
		}

		public uint H
		{
			get
			{
				return h;
			}
			set
			{
				h = value;
			}
		}

		public uint F
		{
			get
			{
				return g + h;
			}
		}

		public void UpdateG(Path path)
		{
			g = parent.g + cost + path.GetTraversalCost(node);
		}
	}
}
