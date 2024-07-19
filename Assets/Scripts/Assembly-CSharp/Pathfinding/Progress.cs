using UnityEngine;

namespace Pathfinding
{
	public struct Progress
	{
		public readonly float progress;

		public readonly string description;

		public Progress(float progress, string description)
		{
			this.progress = progress;
			this.description = description;
		}

		public Progress MapTo(float min, float max, string prefix = null)
		{
			return new Progress(Mathf.Lerp(min, max, progress), prefix + description);
		}

		public override string ToString()
		{
			return progress.ToString("0.0") + " " + description;
		}
	}
}
