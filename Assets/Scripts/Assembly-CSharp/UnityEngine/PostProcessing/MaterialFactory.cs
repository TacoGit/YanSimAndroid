using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	public sealed class MaterialFactory : IDisposable
	{
		private Dictionary<string, Material> m_Materials;

		public MaterialFactory()
		{
			m_Materials = new Dictionary<string, Material>();
		}

		public Material Get(string shaderName)
		{
			Material value;
			if (!m_Materials.TryGetValue(shaderName, out value))
			{
				Shader shader = Shader.Find(shaderName);
				if (shader == null)
				{
					throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));
				}
				Material material = new Material(shader);
				material.name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1));
				material.hideFlags = HideFlags.DontSave;
				value = material;
				m_Materials.Add(shaderName, value);
			}
			return value;
		}

		public void Dispose()
		{
			Dictionary<string, Material>.Enumerator enumerator = m_Materials.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Material value = enumerator.Current.Value;
				GraphicsUtils.Destroy(value);
			}
			m_Materials.Clear();
		}
	}
}
