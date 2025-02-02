﻿using System;
using System.Collections.Generic;
using Util;

/// <summary>
/// Defines the static variables defining a creature
/// </summary>
public class CreatureDefinition
{
	public string Description { get; set; }
	public IDictionary<ResourceType, int> Recipe { get; set; }
	public bool NoEnergy { get; set; }
	public TerrainType[] AllowedTerrain { get; set; }
	public CreatureSpeed Speed { get; set; }
	public IAbilityDefinition Ability { get; set; }
	public bool IsEnemy { get; set; }

	// TODO make sure this can be updated in the editor
	private static IDictionary<CreatureType, CreatureDefinition> creatureList = null;

	public static CreatureDefinition ForType(CreatureType type)
	{
		if (creatureList == null)
		{
			creatureList = Serialization.DeserializeCreatureDefinitions();
		}
		return creatureList[type];
	}

	public IDictionary<ResourceType, int> RecipeWithEnergy()
	{
		return NoEnergy ? Recipe : Recipe.MultisetAdd(ResourceType.Energy, 1);
	}
}
