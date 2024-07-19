namespace Pathfinding
{
	public struct Connection
	{
		public GraphNode node;

		public uint cost;

		public byte shapeEdge;

		public Connection(GraphNode node, uint cost, byte shapeEdge = byte.MaxValue)
		{
			this.node = node;
			this.cost = cost;
			this.shapeEdge = shapeEdge;
		}

		public override int GetHashCode()
		{
			return node.GetHashCode() ^ (int)cost;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Connection connection = (Connection)obj;
			return connection.node == node && connection.cost == cost && connection.shapeEdge == shapeEdge;
		}
	}
}
