using UnityEngine;

namespace Pathfinding
{
	public abstract class VersionedMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver, IVersionedMonoBehaviourInternal
	{
		[SerializeField]
		[HideInInspector]
		private int version;

		protected virtual void Awake()
		{
			if (Application.isPlaying)
			{
				version = OnUpgradeSerializedData(int.MaxValue, true);
			}
		}

		private void Reset()
		{
			version = OnUpgradeSerializedData(int.MaxValue, true);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			version = OnUpgradeSerializedData(version, false);
		}

		protected virtual int OnUpgradeSerializedData(int version, bool unityThread)
		{
			return 1;
		}

		int IVersionedMonoBehaviourInternal.OnUpgradeSerializedData(int version, bool unityThread)
		{
			return OnUpgradeSerializedData(version, unityThread);
		}
	}
}
