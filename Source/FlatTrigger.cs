using System;
using UnityEngine;

public class FlatTrigger : MonoBehaviour
{
	public GameObject RoomToOff;

	public AudioSource source;

	public GameObject[] ToDestroy;

	private void OnTriggerEnter(Collider other)
	{
		this.source.Play();
		if (other.tag == "Player")
		{
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = (GameObject)array[i];
				Light[] components = gameObject.GetComponents<Light>();
				for (int j = 0; j < components.Length; j++)
				{
					Light light = components[j];
					light.enabled = false;
				}
				Light[] componentsInChildren = gameObject.GetComponentsInChildren<Light>();
				for (int k = 0; k < componentsInChildren.Length; k++)
				{
					Light light2 = componentsInChildren[k];
					light2.enabled = false;
				}
				Light[] componentsInParent = gameObject.GetComponentsInParent<Light>();
				for (int l = 0; l < componentsInParent.Length; l++)
				{
					Light light3 = componentsInParent[l];
					light3.enabled = false;
				}
			}
			for (int m = 0; m < this.ToDestroy.Length; m++)
			{
				UnityEngine.Object.Destroy(this.ToDestroy[m]);
			}
			this.RoomToOff.SetActive(false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
