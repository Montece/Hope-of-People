using System;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
	public GameObject[] ToDestroy;

	public void Use()
	{
		GameObject[] toDestroy = this.ToDestroy;
		for (int i = 0; i < toDestroy.Length; i++)
		{
			GameObject gameObject = toDestroy[i];
			UnityEngine.Object.Destroy(gameObject.gameObject);
		}
	}
}
