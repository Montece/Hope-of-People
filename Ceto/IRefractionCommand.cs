using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ceto
{
	public interface IRefractionCommand
	{
		REFRACTION_RESOLUTION Resolution
		{
			get;
			set;
		}

		CameraEvent Event
		{
			get;
			set;
		}

		CommandBuffer Create(Camera cam);

		void Remove(Camera cam);

		void RemoveAll();

		bool Matches(Camera cam);
	}
}
