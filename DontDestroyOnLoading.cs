using System;
using UnityEngine;

public class DontDestroyOnLoading : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}
