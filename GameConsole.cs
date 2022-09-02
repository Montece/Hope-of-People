using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameConsole : MonoBehaviour
{
	public KeyCode OpenCloseConsole = KeyCode.F1;

	public GameObject ConsoleField;

	public Text ConsoleText;

	public InputField ConsoleInput;

	private void Start()
	{
		Application.logMessageReceived += new Application.LogCallback(this.Application_logMessageReceived);
		this.ConsoleField.SetActive(false);
		this.ConsoleText.text = string.Empty;
	}

	private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
	{
		this.AddLog(condition + " " + stackTrace, ConsoleLogType.Game);
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.OpenCloseConsole))
		{
			this.ConsoleField.SetActive(!this.ConsoleField.activeSelf);
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			this.ButtonAddLog();
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			this.DoCommand("showitems");
		}
	}

	public void AddLog(object text, ConsoleLogType type)
	{
		if (type != ConsoleLogType.Game)
		{
			if (type == ConsoleLogType.User)
			{
				Text expr_52 = this.ConsoleText;
				string text2 = expr_52.text;
				expr_52.text = string.Concat(new object[]
				{
					text2,
					"[USER]: ",
					text,
					Environment.NewLine
				});
			}
		}
		else
		{
			Text expr_18 = this.ConsoleText;
			string text2 = expr_18.text;
			expr_18.text = string.Concat(new object[]
			{
				text2,
				"[GAME]: ",
				text,
				Environment.NewLine
			});
		}
	}

	public void ButtonAddLog()
	{
		if (this.ConsoleInput.text != string.Empty)
		{
			this.AddLog(this.ConsoleInput.text, ConsoleLogType.User);
			this.DoCommand(this.ConsoleInput.text);
			this.ConsoleInput.text = string.Empty;
		}
	}

	private void DoCommand(object text)
	{
		string text2 = text.ToString().ToLower();
		if (text2 != null)
		{
			if (text2 == "showitems")
			{
				base.GetComponent<CheatPanel>().Open();
				return;
			}
			if (text2 == "gotoworld")
			{
				SceneManager.LoadScene("World");
				return;
			}
		}
		this.AddLog("Unknown command!", ConsoleLogType.Game);
	}
}
