using System;

namespace UnityEngine.PostProcessing.Utilities
{
	[RequireComponent(typeof(PostProcessingController))]
	public class FocusPuller : MonoBehaviour
	{
		[SerializeField]
		private Transform _target;

		[SerializeField]
		private float _offset;

		[SerializeField]
		private float _speed = 10f;

		private PostProcessingController _controller;

		private float _velocity;

		public Transform target
		{
			get
			{
				return this._target;
			}
			set
			{
				this._target = value;
			}
		}

		public float offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		public float speed
		{
			get
			{
				return this._speed;
			}
			set
			{
				this._speed = Mathf.Max(0.01f, value);
			}
		}

		private void Start()
		{
			this._controller = base.GetComponent<PostProcessingController>();
		}

		private void OnValidate()
		{
			this.speed = this._speed;
		}

		private void Update()
		{
			if (this._target == null)
			{
				return;
			}
			float focusDistance = this._controller.depthOfField.focusDistance;
			float num = Vector3.Dot(this._target.position - base.transform.position, base.transform.forward);
			float deltaTime = Time.deltaTime;
			float num2 = this._velocity - (focusDistance - num) * this.speed * this.speed * deltaTime;
			float num3 = 1f + this.speed * deltaTime;
			this._velocity = num2 / (num3 * num3);
			float focusDistance2 = focusDistance + this._velocity * deltaTime;
			this._controller.depthOfField.focusDistance = focusDistance2;
		}
	}
}
