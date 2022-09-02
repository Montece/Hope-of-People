using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ceto
{
	[AddComponentMenu("Ceto/Overlays/AddFoamTrail")]
	public class AddFoamTrail : AddWaveOverlayBase
	{
		public enum ROTATION
		{
			NONE,
			RANDOM,
			RELATIVE
		}

		private readonly float MIN_MOVEMENT = 0.1f;

		private readonly float MAX_MOVEMENT = 100f;

		public Texture foamTexture;

		public bool textureFoam = true;

		public AddFoamTrail.ROTATION rotation = AddFoamTrail.ROTATION.RANDOM;

		public AnimationCurve timeLine = AddWaveOverlayBase.DefaultCurve();

		public float duration = 10f;

		public float size = 10f;

		public float spacing = 4f;

		public float expansion = 1f;

		public float momentum = 1f;

		public float spin = 10f;

		public bool mustBeBelowWater = true;

		[Range(0f, 2f)]
		public float alpha = 0.8f;

		[Range(0f, 1f)]
		public float jitter = 0.2f;

		private Vector3 m_lastPosition;

		private float m_remainingDistance;

		protected override void Start()
		{
			this.m_lastPosition = base.transform.position;
		}

		protected override void Update()
		{
			this.UpdateOverlays();
			this.AddFoam();
			this.RemoveOverlays();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_lastPosition = base.transform.position;
		}

		public override void Translate(Vector3 amount)
		{
			base.Translate(amount);
			this.m_lastPosition += amount;
		}

		private float Rotation()
		{
			AddFoamTrail.ROTATION rOTATION = this.rotation;
			if (rOTATION == AddFoamTrail.ROTATION.NONE)
			{
				return 0f;
			}
			if (rOTATION == AddFoamTrail.ROTATION.RANDOM)
			{
				return UnityEngine.Random.Range(0f, 360f);
			}
			if (rOTATION != AddFoamTrail.ROTATION.RELATIVE)
			{
				return 0f;
			}
			return base.transform.eulerAngles.y;
		}

		private void AddFoam()
		{
			if (this.duration <= 0f || Ocean.Instance == null)
			{
				this.m_lastPosition = base.transform.position;
				return;
			}
			this.spacing = Mathf.Max(1f, this.spacing);
			this.size = Mathf.Max(1f, this.size);
			Vector3 position = base.transform.position;
			float num = position.y;
			if (this.mustBeBelowWater)
			{
				num = Ocean.Instance.QueryWaves(position.x, position.z);
			}
			if (num < position.y)
			{
				this.m_lastPosition = position;
				return;
			}
			position.y = 0f;
			this.m_lastPosition.y = 0f;
			Vector3 vector = this.m_lastPosition - position;
			Vector3 normalized = vector.normalized;
			float num2 = vector.magnitude;
			if (num2 < this.MIN_MOVEMENT)
			{
				return;
			}
			num2 = Mathf.Min(this.MAX_MOVEMENT, num2);
			Vector3 vector2 = normalized * this.momentum;
			this.m_remainingDistance += num2;
			float num3 = 0f;
			while (this.m_remainingDistance > this.spacing)
			{
				Vector3 pos = position + normalized * num3;
				FoamOverlay foamOverlay = new FoamOverlay(pos, this.Rotation(), this.size, this.duration, this.foamTexture);
				foamOverlay.FoamTex.alpha = 0f;
				foamOverlay.FoamTex.textureFoam = this.textureFoam;
				foamOverlay.Momentum = vector2;
				foamOverlay.Spin = ((UnityEngine.Random.value <= 0.5f) ? this.spin : (-this.spin));
				foamOverlay.Expansion = this.expansion;
				if (this.jitter > 0f)
				{
					foamOverlay.Spin *= 1f + UnityEngine.Random.Range(-1f, 1f) * this.jitter;
					foamOverlay.Expansion *= 1f + UnityEngine.Random.Range(-1f, 1f) * this.jitter;
				}
				this.m_overlays.Add(foamOverlay);
				Ocean.Instance.OverlayManager.Add(foamOverlay);
				this.m_remainingDistance -= this.spacing;
				num3 += this.spacing;
			}
			this.m_lastPosition = position;
		}

		private void UpdateOverlays()
		{
			IEnumerator<WaveOverlay> enumerator = this.m_overlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				float normalizedAge = enumerator.Current.NormalizedAge;
				enumerator.Current.FoamTex.alpha = this.timeLine.Evaluate(normalizedAge) * this.alpha;
				enumerator.Current.FoamTex.textureFoam = this.textureFoam;
				enumerator.Current.UpdateOverlay();
			}
		}

		private void RemoveOverlays()
		{
			LinkedList<WaveOverlay> linkedList = new LinkedList<WaveOverlay>();
			IEnumerator<WaveOverlay> enumerator = this.m_overlays.GetEnumerator();
			while (enumerator.MoveNext())
			{
				WaveOverlay current = enumerator.Current;
				if (current.Age >= current.Duration)
				{
					linkedList.AddLast(current);
					current.Kill = true;
				}
			}
			LinkedList<WaveOverlay>.Enumerator enumerator2 = linkedList.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				this.m_overlays.Remove(enumerator2.Current);
			}
		}

		private void OnDrawGizmos()
		{
			if (!base.enabled)
			{
				return;
			}
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, 2f);
		}
	}
}
