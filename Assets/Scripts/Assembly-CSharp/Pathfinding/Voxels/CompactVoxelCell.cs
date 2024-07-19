namespace Pathfinding.Voxels
{
	public struct CompactVoxelCell
	{
		public uint index;

		public uint count;

		public CompactVoxelCell(uint i, uint c)
		{
			index = i;
			count = c;
		}
	}
}
