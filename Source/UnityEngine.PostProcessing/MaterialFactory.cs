using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	public sealed class MaterialFactory : IDisposable
	{
		private Dictionary<string, Material> m_Materials;

		public MaterialFactory()
		{
			this.m_Materials = new Dictionary<string, Material>();
		}

		public Material Get(string shaderName)
		{
			Material material;
			if (!this.m_Materials.TryGetValue(shaderName, out material))
			{
				Shader shader = Shader.Find(shaderName);
				if (shader == null)
				{
					throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));
				}
				material = new Material(shader)
				{
					name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
					hideFlags = HideFlags.DontSave
				};
				this.m_Materials.Add(shaderName, material);
			}
			return material;
		}

		public void Dispose()
		{
			Dictionary<string, Material>.Enumerator enumerator = this.m_Materials.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, Material> current = enumerator.Current;
				Material value = current.Value;
				GraphicsUtils.Destroy(value);
			}
			this.m_Materials.Clear();
		}
	}
}
