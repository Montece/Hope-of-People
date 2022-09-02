using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public abstract class PostProcessingModel
	{
		[GetSet("enabled"), SerializeField]
		private bool m_Enabled;

		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		public abstract void Reset();

		public virtual void OnValidate()
		{
		}
	}
}
