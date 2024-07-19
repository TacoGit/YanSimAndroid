namespace Pathfinding.Voxels
{
	public struct CompactVoxelSpan
	{
		public ushort y;

		public uint con;

		public uint h;

		public int reg;

		public CompactVoxelSpan(ushort bottom, uint height)
		{
			con = 24u;
			y = bottom;
			h = height;
			reg = 0;
		}

		public void SetConnection(int dir, uint value)
		{
			int num = dir * 6;
			con = (uint)((con & ~(63 << num)) | ((value & 0x3F) << num));
		}

		public int GetConnection(int dir)
		{
			return ((int)con >> dir * 6) & 0x3F;
		}
	}
}
