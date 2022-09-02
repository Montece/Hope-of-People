using System;
using UnityEngine;

public class LoadSkyFromFile : MonoBehaviour
{
	public TOD_Sky sky;

	public TextAsset textAsset;

	protected void Start()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		if (this.textAsset)
		{
			this.sky.LoadParameters(this.textAsset.text);
		}
	}
}
