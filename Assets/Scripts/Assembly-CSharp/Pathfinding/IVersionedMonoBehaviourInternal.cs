namespace Pathfinding
{
	public interface IVersionedMonoBehaviourInternal
	{
		int OnUpgradeSerializedData(int version, bool unityThread);
	}
}
