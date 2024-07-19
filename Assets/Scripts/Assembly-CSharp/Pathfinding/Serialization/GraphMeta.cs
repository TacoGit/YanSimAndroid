using System;
using System.Collections.Generic;
using Pathfinding.WindowsStore;

namespace Pathfinding.Serialization
{
	public class GraphMeta
	{
		public Version version;

		public int graphs;

		public List<string> guids;

		public List<string> typeNames;

		public Type GetGraphType(int index)
		{
			if (string.IsNullOrEmpty(typeNames[index]))
			{
				return null;
			}
			Type type = WindowsStoreCompatibility.GetTypeInfo(typeof(AstarPath)).Assembly.GetType(typeNames[index]);
			if (!object.Equals(type, null))
			{
				return type;
			}
			throw new Exception("No graph of type '" + typeNames[index] + "' could be created, type does not exist");
		}
	}
}
