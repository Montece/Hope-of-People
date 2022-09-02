using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ceto
{
	public class DepthData
	{
		public bool updated;

		public Camera cam;

		public CommandBuffer grabCmd;

		public CameraEvent cmdEvent;
	}
}
