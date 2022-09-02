using System;
using UnityEngine;

public class TOD_Animation : MonoBehaviour
{
	[Tooltip("Wind direction in degrees.")]
	public float WindDegrees;

	[Tooltip("Speed of the wind that is acting on the clouds.")]
	public float WindSpeed = 1f;

	[Tooltip("Adjust the cloud coordinates when the sky dome moves.")]
	public bool WorldSpaceCloudUV = true;

	[Tooltip("Randomize the cloud coordinates at startup.")]
	public bool RandomInitialCloudUV = true;

	private TOD_Sky sky;

	internal Vector4 CloudUV
	{
		get;
		set;
	}

	internal Vector4 OffsetUV
	{
		get
		{
			if (!this.WorldSpaceCloudUV)
			{
				return Vector4.zero;
			}
			Vector3 position = base.transform.position;
			Vector3 lossyScale = base.transform.lossyScale;
			Vector3 point = new Vector3(position.x / lossyScale.x, 0f, position.z / lossyScale.z);
			point = Quaternion.Euler(0f, -base.transform.rotation.eulerAngles.y, 0f) * point;
			return new Vector4(point.x, point.z, point.x, point.z);
		}
	}

	private void AddUV(Vector4 uv)
	{
		this.CloudUV += uv;
		this.CloudUV = new Vector4(this.CloudUV.x % this.sky.Clouds.Scale1.x, this.CloudUV.y % this.sky.Clouds.Scale1.y, this.CloudUV.z % this.sky.Clouds.Scale2.x, this.CloudUV.w % this.sky.Clouds.Scale2.y);
	}

	protected void Start()
	{
		this.sky = base.GetComponent<TOD_Sky>();
		if (this.RandomInitialCloudUV)
		{
			this.AddUV(new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value) * 1000f);
		}
	}

	protected void Update()
	{
		Vector2 vector = new Vector2(Mathf.Cos(0.0174532924f * (this.WindDegrees + 15f)), Mathf.Sin(0.0174532924f * (this.WindDegrees + 15f)));
		Vector2 vector2 = new Vector2(Mathf.Cos(0.0174532924f * (this.WindDegrees - 15f)), Mathf.Sin(0.0174532924f * (this.WindDegrees - 15f)));
		Vector4 a = this.WindSpeed / 100f * new Vector4(vector.x, vector.y, vector2.x, vector2.y);
		this.AddUV(Time.deltaTime * a);
		this.sky.Components.Billboards.transform.Rotate(0f, Time.deltaTime * this.WindSpeed / 10f, 0f);
	}
}
