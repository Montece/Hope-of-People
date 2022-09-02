using System;
using UnityEngine;

public class AnimatedTextures : MonoBehaviour
{
	public int colCount = 4;

	public int rowCount = 4;

	public int rowNumber;

	public int colNumber;

	public int totalCells = 4;

	public int fps = 10;

	private Vector2 offset;

	private void Update()
	{
		this.SetSpriteAnimation(this.colCount, this.rowCount, this.rowNumber, this.colNumber, this.totalCells, this.fps);
	}

	private void SetSpriteAnimation(int colCount, int rowCount, int rowNumber, int colNumber, int totalCells, int fps)
	{
		int num = (int)(Time.time * (float)fps);
		num %= totalCells;
		float x = 1f / (float)colCount;
		float y = 1f / (float)rowCount;
		Vector2 value = new Vector2(x, y);
		int num2 = num % colCount;
		int num3 = num / colCount;
		float x2 = (float)(num2 + colNumber) * value.x;
		float y2 = 1f - value.y - (float)(num3 + rowNumber) * value.y;
		Vector2 value2 = new Vector2(x2, y2);
		base.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", value2);
		base.GetComponent<Renderer>().material.SetTextureScale("_MainTex", value);
	}
}
