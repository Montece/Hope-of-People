using System;
using UnityEngine;

namespace Ceto
{
	public class FoamOverlay : WaveOverlay
	{
		public Vector3 Momentum
		{
			get;
			set;
		}

		public float Spin
		{
			get;
			set;
		}

		public float Expansion
		{
			get;
			set;
		}

		public float Size
		{
			get;
			set;
		}

		public FoamOverlay(Vector3 pos, float rotation, float size, float duration, Texture texture) : base(pos, rotation, new Vector2(size * 0.5f, size * 0.5f), duration)
		{
			this.Size = size;
			base.FoamTex.tex = texture;
		}

		public override void UpdateOverlay()
		{
			base.Position += this.Momentum * Time.deltaTime;
			this.Size += this.Expansion * Time.deltaTime;
			base.Rotation += this.Spin * Time.deltaTime;
			base.HalfSize = new Vector2(this.Size * 0.5f, this.Size * 0.5f);
			base.UpdateOverlay();
		}
	}
}
