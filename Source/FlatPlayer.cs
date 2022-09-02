using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlatPlayer : MonoBehaviour
{
	public Text CrosshairTest;

	public float ArmRange = 3f;

	public float KickPower = 700f;

	public KeyCode UseButton = KeyCode.E;

	public Image Black;

	public Text Text;

	private bool Can = true;

	private RaycastHit hit;

	private Ray ray;

	private void Start()
	{
		this.Black.canvasRenderer.SetAlpha(0f);
		this.Text.canvasRenderer.SetAlpha(0f);
	}

	private void Update()
	{
		this.CrosshairTest.text = string.Empty;
		if (this.Black.canvasRenderer.GetAlpha() == 1f)
		{
			SceneManager.LoadScene("Island");
		}
		if (this.Can)
		{
			if (base.transform.position.y <= -10f)
			{
				this.StartGame();
			}
			this.ray = Camera.main.ScreenPointToRay(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2)));
			if (Physics.Raycast(this.ray, out this.hit, this.ArmRange) && this.hit.collider.GetComponent<Furniture>())
			{
				this.CrosshairTest.text = "Knock object  (" + this.UseButton + ")";
				if (Input.GetKeyDown(this.UseButton) && this.hit.collider.gameObject.GetComponent<Rigidbody>())
				{
					this.hit.collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(this.KickPower, this.hit.point, 2f);
				}
			}
		}
	}

	private void StartGame()
	{
		this.Can = false;
		this.Black.CrossFadeAlpha(1f, 3f, false);
		this.Text.CrossFadeAlpha(1f, 3f, false);
	}
}
