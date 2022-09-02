using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

internal class AnimateTiledTexture : MonoBehaviour
{
	public int columns = 2;

	public int rows = 2;

	public float framesPerSecond = 10f;

	private int index;

	private void Start()
	{
		base.StartCoroutine(this.updateTiling());
		Vector2 value = new Vector2(1f / (float)this.columns, 1f / (float)this.rows);
		base.GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", value);
	}

	[DebuggerHidden]
	private IEnumerator updateTiling()
	{
		AnimateTiledTexture.<updateTiling>c__Iterator0 <updateTiling>c__Iterator = new AnimateTiledTexture.<updateTiling>c__Iterator0();
		<updateTiling>c__Iterator.$this = this;
		return <updateTiling>c__Iterator;
	}
}
