﻿using UnityEngine;
using System;
using System.Collections;

public class AudioController : MonoBehaviour {
	
	public SoundEffectOptions soundOptions;

	// Use this for initialization
	void Start ()
	{
		// Play sounds when the creature takes actions
		LevelManager.Creatures.CreatureCreated += (x, y) => PlaySound(soundOptions.createCreature);
		LevelManager.Creatures.CreatureDestroyed += (c, pos) => PlaySound(soundOptions.destroyCreature);
		LevelManager.Recipes.RecipesUpdated += (obj) => PlaySound(soundOptions.pickupRecipe);
		LevelManager.Goals.LevelCompleted += () => PlaySound(soundOptions.levelComplete);
		UXManager.State.Selector.Selected += x => PlaySound(soundOptions.selectCreature);
		UXManager.State.Selector.GoalSet += (x, y) => PlaySound(soundOptions.setCreatureGoal);
		UXManager.State.Selector.AbilityUsed += () => PlaySound(soundOptions.useAbility);
	}

	// Play the given sound
	void PlaySound(AudioClip clip)
	{
		if (clip)
		{
			// TODO perhaps refactor this?
			Camera.main.GetComponent<AudioSource>().PlayOneShot(clip);
		}
	}

}

[Serializable]
public class SoundEffectOptions
{
	public AudioClip destroyCreature;
	public AudioClip createCreature;
	public AudioClip selectCreature;
	public AudioClip pickupRecipe;
	public AudioClip setCreatureGoal;
	// TODO separate audio for different abilities
	public AudioClip useAbility;
	public AudioClip levelComplete;
}
