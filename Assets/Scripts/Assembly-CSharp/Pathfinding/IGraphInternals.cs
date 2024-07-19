using System.Collections.Generic;
using Pathfinding.Serialization;

namespace Pathfinding
{
	public interface IGraphInternals
	{
		string SerializedEditorSettings { get; set; }

		void OnDestroy();

		void DestroyAllNodes();

		IEnumerable<Progress> ScanInternal();

		void SerializeExtraInfo(GraphSerializationContext ctx);

		void DeserializeExtraInfo(GraphSerializationContext ctx);

		void PostDeserialization(GraphSerializationContext ctx);

		void DeserializeSettingsCompatibility(GraphSerializationContext ctx);
	}
}
