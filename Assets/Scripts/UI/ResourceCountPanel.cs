﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ResourceCountPanel : MonoBehaviour {

	public Sprite paperSprite;
	public Sprite energySprite;

	public Image image;
	public Text text;

	public void SetContents(ResourceType type, int count)
	{
		// Fill in the contents
		image.color = GetColor(type);
		image.sprite = GetSprite(type);
		text.text = count.ToString();
	}

	private Color GetColor(ResourceType resource)
	{
		switch(resource)
		{
		// TODO Store the heart's "pink" color somewhere
		case ResourceType.Energy: return new Color(1.0f, 0.564705882f, 0.984313725f, 1.0f);
		case ResourceType.Black: return Color.black;
		case ResourceType.White: return Color.white;
		case ResourceType.Red: return Color.red;
		case ResourceType.Blue: return Color.blue;
		default: throw new ArgumentException("Illegal resource: " + resource, "resource");
		}
	}

	private Sprite GetSprite(ResourceType resource)
	{
		switch(resource)
		{
		case ResourceType.Energy: return energySprite;
		default: return paperSprite;
		}
	}
}
