using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
	public AudioSource MusicSound;

	[SerializeField]
	private KeyCode SpeedUp = KeyCode.Space;

	[SerializeField]
	private KeyCode SpeedDown = KeyCode.C;

	[SerializeField]
	private KeyCode Forward = KeyCode.W;

	[SerializeField]
	private KeyCode Back = KeyCode.S;

	[SerializeField]
	private KeyCode Left = KeyCode.A;

	[SerializeField]
	private KeyCode Right = KeyCode.D;

	[SerializeField]
	private KeyCode TurnLeft = KeyCode.Q;

	[SerializeField]
	private KeyCode TurnRight = KeyCode.E;

	[SerializeField]
	private KeyCode MusicOffOn = KeyCode.M;

	private KeyCode[] keyCodes;

	public Action<PressedKeyCode[]> KeyPressed;

	private void Awake()
	{
		this.keyCodes = new KeyCode[]
		{
			this.SpeedUp,
			this.SpeedDown,
			this.Forward,
			this.Back,
			this.Left,
			this.Right,
			this.TurnLeft,
			this.TurnRight
		};
	}

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		List<PressedKeyCode> list = new List<PressedKeyCode>();
		for (int i = 0; i < this.keyCodes.Length; i++)
		{
			KeyCode key = this.keyCodes[i];
			if (Input.GetKey(key))
			{
				list.Add((PressedKeyCode)i);
			}
		}
		if (this.KeyPressed != null)
		{
			this.KeyPressed(list.ToArray());
		}
	}
}
