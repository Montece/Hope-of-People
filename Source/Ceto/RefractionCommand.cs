using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ceto
{
	public class RefractionCommand : RefractionCommandBase
	{
		public Material m_copyDepthMat;

		public string GrabName
		{
			get;
			private set;
		}

		public string DepthName
		{
			get;
			private set;
		}

		public RefractionCommand(Shader copyDepth)
		{
			this.GrabName = Ocean.REFRACTION_GRAB_TEXTURE_NAME;
			this.DepthName = Ocean.DEPTH_GRAB_TEXTURE_NAME;
			this.m_copyDepthMat = new Material(copyDepth);
			this.m_data = new Dictionary<Camera, RefractionCommandBase.CommandData>();
		}

		public override CommandBuffer Create(Camera cam)
		{
			CommandBuffer commandBuffer = new CommandBuffer();
			commandBuffer.name = "Ceto DepthGrab Cmd: " + cam.name;
			RenderTextureFormat format;
			if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat))
			{
				format = RenderTextureFormat.RFloat;
			}
			else
			{
				format = RenderTextureFormat.RHalf;
			}
			int nameID = Shader.PropertyToID("Ceto_DepthCopyTexture");
			commandBuffer.GetTemporaryRT(nameID, cam.pixelWidth, cam.pixelHeight, 0, FilterMode.Point, format, RenderTextureReadWrite.Linear);
			commandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, nameID, this.m_copyDepthMat, 0);
			commandBuffer.SetGlobalTexture(this.DepthName, nameID);
			cam.AddCommandBuffer(base.Event, commandBuffer);
			RefractionCommandBase.CommandData commandData = new RefractionCommandBase.CommandData();
			commandData.command = commandBuffer;
			commandData.width = cam.pixelWidth;
			commandData.height = cam.pixelHeight;
			if (this.m_data.ContainsKey(cam))
			{
				this.m_data.Remove(cam);
			}
			this.m_data.Add(cam, commandData);
			return commandBuffer;
		}
	}
}
