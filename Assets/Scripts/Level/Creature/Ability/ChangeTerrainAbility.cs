﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;

public class ChangeTerrainAbility : AbstractCarryAbility, IAbility
{
	public TerrainType carryType;
	public TerrainType leaveType;

	private GameObject carriedObject;

	public class Definition : IAbilityDefinition
	{
		public TerrainType CarryType { get; set; }
		public TerrainType LeaveType { get; set; }

		public string Description()
		{
			return "Dig up " + CarryType;
		}

		public IAbility AddToCreature(Creature creature)
		{
			var ability = creature.gameObject.AddComponent<ChangeTerrainAbility>();
			ability.carryType = CarryType;
			ability.leaveType = LeaveType;
			return ability;
		}
	}

	public bool isCarrying;

	protected override void DoPickup(Coordinate target)
	{
		// Pick up or put down the terrain if we can
		if (CanUse(target))
		{
			isCarrying = !isCarrying;
			LevelManager.Terrain[target] = isCarrying ? leaveType : carryType;
			// Visually represent the carried tile
			if (isCarrying)
			{
				carriedObject = creature.gameObject.AddChild(
					ResourcesPathfinder.TerrainPrefab(carryType));
				var translate = creature.GetComponentInChildren<MeshRenderer>().bounds.size.y;
				carriedObject.transform.Translate(Vector3.up * translate);
				carriedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			}
			else
			{
				Destroy(carriedObject);
				carriedObject = null;
			}
		}
	}

	public override bool CanUse(Coordinate coordinate)
	{
		// Determine whether the creature should be picking up or putting down
		TerrainType initialType;
		TerrainType finalType;
		if (isCarrying)
		{
			initialType = leaveType;
			finalType = carryType;
		} else {
			initialType = carryType;
			finalType = leaveType;
		}
		
		return LevelManager.Terrain.Contains(coordinate)
			&& LevelManager.Terrain[coordinate] == initialType
			&& LevelManager.Resources[coordinate].IsEmpty()
			&& CanSetCoordinate(coordinate, finalType);
	}

	// Returns true if you can set the coordinate to the specified terrain type
	private bool CanSetCoordinate(Coordinate target, TerrainType terrain)
	{
		return !LevelManager.Creatures.CreatureList.Any(x => (x.Position == target || x.NextPosition == target)
			&& !x.Definition.AllowedTerrain.Contains(terrain));
	}

	public override string Description()
	{
		if (isCarrying)
		{
			return "Put down " + carryType;
		}
		else
		{
			return "Dig up " + carryType;
		}
	}
}
