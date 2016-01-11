﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Util;

// TODO refactor with the recipe list
public class LevelSelect : MonoBehaviour
{
	public GameObject buttonContainer;
	public DelegateButton buttonPrefab;

	public string[] levels;

	public Sprite finishedSprite;
	public Sprite unfinishedSprite;

	void Update()
	{
		gameObject.SetActive(true);
		buttonContainer.DestroyAllChildren();

		// Populate the UI with buttons corresponding with the recipes
		for (int i = 0; i < levels.Length; i++)
		{
			var level = levels[i];
			DelegateButton newButton = Instantiate(buttonPrefab);
			newButton.transform.SetParent(buttonContainer.transform, false);
			newButton.GetComponentInChildren<Text>().text = "Level " + (i+1);
			// Translate the new object by the index amount
			var transform = newButton.GetComponent<RectTransform>();
			transform.anchoredPosition += Vector2.down * i * transform.rect.height;

			// Set the component to do the hooked in action when clicked
			var createButton = newButton.GetComponent<DelegateButton>();
			createButton.Click += () => GameManager.game.LoadLevel(level);

			if (GameManager.game.GetCompletion(level))
			{
				newButton.image.sprite = finishedSprite;
			}
			else
			{
				newButton.image.sprite = unfinishedSprite;	
			}
		}
	}

	// Clear our game progress
	public void ClearProgress()
	{
		foreach (string level in levels)
		{
			GameManager.game.SetCompletion(level, false);
		}
		GameManager.game.Save();
	}

}
