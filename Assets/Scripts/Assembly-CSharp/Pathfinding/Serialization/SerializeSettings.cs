using System;

namespace Pathfinding.Serialization
{
	public class SerializeSettings
	{
		public bool nodes = true;

		[Obsolete("There is no support for pretty printing the json anymore")]
		public bool prettyPrint;

		public bool editorSettings;

		public static SerializeSettings Settings
		{
			get
			{
				SerializeSettings serializeSettings = new SerializeSettings();
				serializeSettings.nodes = false;
				return serializeSettings;
			}
		}
	}
}
