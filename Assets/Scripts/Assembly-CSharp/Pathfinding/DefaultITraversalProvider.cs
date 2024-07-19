namespace Pathfinding
{
	public static class DefaultITraversalProvider
	{
		public static bool CanTraverse(Path path, GraphNode node)
		{
			return node.Walkable && ((path.enabledTags >> (int)node.Tag) & 1) != 0;
		}

		public static uint GetTraversalCost(Path path, GraphNode node)
		{
			return path.GetTagPenalty((int)node.Tag) + node.Penalty;
		}
	}
}
