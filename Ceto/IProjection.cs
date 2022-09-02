using System;
using UnityEngine;

namespace Ceto
{
	public interface IProjection
	{
		bool IsDouble
		{
			get;
		}

		void UpdateProjection(Camera cam, CameraData data, bool projectSceneView);
	}
}
