using System;
using System.Xml.Serialization;
using UnityEngine;

public class Buff
{
	public string Title;

	public int Id;

	[XmlIgnore]
	public Sprite Icon;

	public bool IsPositive;

	public bool IsInfinity;
}
