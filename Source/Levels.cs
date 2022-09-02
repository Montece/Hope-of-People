using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{
	public string LevelName = string.Empty;

	private void OnCollisionEnter(Collision collision)
	{
		SceneManager.LoadScene(this.LevelName);
	}
}
