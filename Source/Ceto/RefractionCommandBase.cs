using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ceto
{
	public abstract class RefractionCommandBase : IRefractionCommand
	{
		public class CommandData
		{
			public CommandBuffer command;

			public int width;

			public int height;
		}

		protected Dictionary<Camera, RefractionCommandBase.CommandData> m_data;

		public CameraEvent Event
		{
			get;
			set;
		}

		public REFRACTION_RESOLUTION Resolution
		{
			get;
			set;
		}

		public RefractionCommandBase()
		{
			this.m_data = new Dictionary<Camera, RefractionCommandBase.CommandData>();
		}

		public abstract CommandBuffer Create(Camera cam);

		public virtual void Remove(Camera cam)
		{
			if (this.m_data.ContainsKey(cam))
			{
				RefractionCommandBase.CommandData commandData = this.m_data[cam];
				cam.RemoveCommandBuffer(this.Event, commandData.command);
				this.m_data.Remove(cam);
			}
		}

		public virtual void RemoveAll()
		{
			if (this.m_data.Count == 0)
			{
				return;
			}
			Dictionary<Camera, RefractionCommandBase.CommandData>.Enumerator enumerator = this.m_data.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<Camera, RefractionCommandBase.CommandData> current = enumerator.Current;
				Camera key = current.Key;
				KeyValuePair<Camera, RefractionCommandBase.CommandData> current2 = enumerator.Current;
				CommandBuffer command = current2.Value.command;
				key.RemoveCommandBuffer(this.Event, command);
			}
			this.m_data.Clear();
		}

		public virtual bool Matches(Camera cam)
		{
			if (!this.m_data.ContainsKey(cam))
			{
				return false;
			}
			RefractionCommandBase.CommandData commandData = this.m_data[cam];
			return commandData.width == cam.pixelWidth && commandData.height == cam.pixelHeight;
		}

		protected virtual int ResolutionToNumber(REFRACTION_RESOLUTION resolution)
		{
			switch (resolution)
			{
			case REFRACTION_RESOLUTION.FULL:
				return 1;
			case REFRACTION_RESOLUTION.HALF:
				return 2;
			case REFRACTION_RESOLUTION.QUARTER:
				return 4;
			default:
				return 2;
			}
		}
	}
}
