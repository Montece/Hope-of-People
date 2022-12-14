using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		[Serializable]
		public struct Settings
		{
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			[Range(0f, 1f), Tooltip("Blending factor.")]
			public float contribution;

			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}
		}

		[SerializeField]
		private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		public UserLutModel.Settings settings
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
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}
	}
}
