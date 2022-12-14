using System;
using System.Runtime.InteropServices;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		[Serializable]
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Settings
		{
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}

		[SerializeField]
		private DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		public DitheringModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}
	}
}
