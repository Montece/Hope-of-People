using System;
using UnityEngine;

public class GrenadeInWorld : MonoBehaviour
{
	public GrenadeType Type;

	public float Delay = 5f;

	public GameObject PrefabOfAction;

	public AudioClip Sound;

	private AudioSource source;

	private GameManager manager;

	private void Start()
	{
		this.source = base.GetComponent<AudioSource>();
		this.manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		base.Invoke("DoIt", this.Delay);
	}

	private void DoIt()
	{
		this.source.clip = this.Sound;
		this.source.Play();
		switch (this.Type)
		{
		case GrenadeType.Standart:
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PrefabOfAction, base.transform.position, Quaternion.identity);
			gameObject.transform.position = base.transform.position;
			UnityEngine.Object.Destroy(base.gameObject);
			break;
		}
		case GrenadeType.Smoke:
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PrefabOfAction, base.transform.position, Quaternion.identity);
			gameObject.transform.position = base.transform.position;
			UnityEngine.Object.Destroy(base.gameObject);
			break;
		}
		case GrenadeType.Supply:
			this.manager.CallAirDrop(base.transform.position);
			UnityEngine.Object.Destroy(base.gameObject);
			break;
		}
	}
}
