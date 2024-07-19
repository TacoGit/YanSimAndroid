using System;

namespace Pathfinding
{
	[Serializable]
	public abstract class MonoModifier : VersionedMonoBehaviour, IPathModifier
	{
		[NonSerialized]
		public Seeker seeker;

		public abstract int Order { get; }

		protected virtual void OnEnable()
		{
			seeker = GetComponent<Seeker>();
			if (seeker != null)
			{
				seeker.RegisterModifier(this);
			}
		}

		protected virtual void OnDisable()
		{
			if (seeker != null)
			{
				seeker.DeregisterModifier(this);
			}
		}

		public virtual void PreProcess(Path path)
		{
		}

		public abstract void Apply(Path path);
	}
}
