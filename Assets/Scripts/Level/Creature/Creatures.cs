using UnityEngine;
using System;
using System.Collections.Generic;

public enum CreatureType
{
	Crane,
	Turtle,
	Horse,
	Elephant,
	Crab,
	Wolf,
	Flower,
	Beaver,
	Dolphin,
	Alligator,
	Lion,
	Camel
}

/// <summary>
/// Statically defines the creatures available in this game.
/// </summary>
public static class CreatureDefinitions
{
	private static Func<Creature, CreatureSpeed> FixedSpeed(CreatureSpeed speed)
	{
		return x => speed;
	}

	public static CreatureDefinition ForType(CreatureType type)
	{
		switch(type)
		{
		case CreatureType.Crane:
			return new CreatureDefinition
			{
				Description = "The basic origami crane",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 1} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Water },
				Speed = FixedSpeed(CreatureSpeed.Medium)
			};
		case CreatureType.Turtle:
			return new CreatureDefinition
			{
				Description = "A seabound unit that can carry resources",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Green, 1} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Water },
				Speed = x =>
				{
					switch(LevelManager.Terrain[x.Position])
					{
					case TerrainType.Water: return CreatureSpeed.Medium;
					case TerrainType.Land: return CreatureSpeed.Slow;
					default: throw new InvalidOperationException("Terrain not allowed");
					}
				},
				Ability = new CarryResourceAbility.Definition { Capacity = 5 }
			};
		case CreatureType.Horse:
			return new CreatureDefinition
			{
				Description = "A versatile land unit that can carry resources",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Red, 4} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Rock },
				Speed =  x =>
				{
					if (x.GetComponent<CarryResourceAbility>().Carrying.IsEmpty())
					{
						return CreatureSpeed.Fast;
					}
					else
					{
						return CreatureSpeed.Medium;
					}
				},
				Ability = new CarryResourceAbility.Definition { Capacity = 5 }
			};
		case CreatureType.Elephant:
			return new CreatureDefinition
			{
				Description = "A large unit that can uproot and move trees",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 9} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land },
				Speed = FixedSpeed(CreatureSpeed.Slow),
				Ability = new ChangeTerrainAbility.Definition
				{
					CarryType = TerrainType.Tree,
					LeaveType = TerrainType.Land
				}
			};
		case CreatureType.Crab:
			return new CreatureDefinition
			{
				Description = "A basic enemy creature",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Red, 1} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land },
				Speed = FixedSpeed(CreatureSpeed.Slow),
				Ability = new FightAbility.Definition { Attack = 10, Defense = 5 },
				IsEnemy = true
			};
		case CreatureType.Wolf:
			return new CreatureDefinition
			{
				Description = "Can fight enemies",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 4} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Rock },
				Speed = FixedSpeed(CreatureSpeed.Fast),
				Ability = new FightAbility.Definition { Attack = 10, Defense = 7 }
			};
		case CreatureType.Flower:
			return new CreatureDefinition
			{
				Description = "Can heal other creatures",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Yellow, 1} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land },
				Speed = FixedSpeed(CreatureSpeed.Idle),
				Ability = new HealAbility.Definition { HealPower = 1 }
			};
		case CreatureType.Beaver:
			return new CreatureDefinition
			{
				Description = "Can move land tiles",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Red, 1} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land },
				Speed =  x =>
				{
					if (x.GetComponent<ChangeTerrainAbility>().isCarrying)
					{
						return CreatureSpeed.Fast;
					}
					else
					{
						return CreatureSpeed.Slow;
					}
				},
				Ability = new ChangeTerrainAbility.Definition
				{
					CarryType = TerrainType.Land,
					LeaveType = TerrainType.Water
				}
			};
		case CreatureType.Dolphin:
			return new CreatureDefinition
			{
				Description = "A fast seabound fighter",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 9} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Water },
				Speed =  FixedSpeed(CreatureSpeed.Fast),
				Ability = new FightAbility.Definition { Attack = 15, Defense = 8 }
			};
		case CreatureType.Alligator:
			return new CreatureDefinition
			{
				Description = "A waterbound enemy creature",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Green, 4} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Water },
				Speed = FixedSpeed(CreatureSpeed.Medium),
				Ability = new FightAbility.Definition { Attack = 13, Defense = 7 },
				IsEnemy = true
			};
		case CreatureType.Lion:
			return new CreatureDefinition
			{
				Description = "The king of the jungle; your toughest enemy",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Yellow, 9} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Rock },
				Speed = FixedSpeed(CreatureSpeed.Medium),
				Ability = new FightAbility.Definition { Attack = 17, Defense = 10 },
				IsEnemy = true
			};
		case CreatureType.Camel:
			return new CreatureDefinition
			{
				Description = "A land unit that can carry a lot of stuff",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Yellow, 9} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land },
				Speed = FixedSpeed(CreatureSpeed.Slow),
				Ability = new CarryResourceAbility.Definition { Capacity = 10 }
			};
		default: throw new ArgumentException("Passed in an invalid creature type: " + type, "type");
		}
	}
}

[Serializable]
public class CreaturePrefabOptions
{
	public GameObject cranePrefab;
	public GameObject turtlePrefab;
	public GameObject horsePrefab;
	public GameObject elephantPrefab;
	public GameObject crabPrefab;
	public GameObject wolfPrefab;
	public GameObject flowerPrefab;
	public GameObject beaverPrefab;
	public GameObject dolphinPrefab;
	public GameObject alligatorPrefab;
	public GameObject lionPrefab;
	public GameObject camelPrefab;

	public GameObject PrefabFor (CreatureType creature)
	{
		switch (creature) {
		case CreatureType.Crane: return cranePrefab;
		case CreatureType.Turtle: return turtlePrefab;
		case CreatureType.Horse: return horsePrefab;
		case CreatureType.Elephant: return elephantPrefab;
		case CreatureType.Crab: return crabPrefab;
		case CreatureType.Wolf: return wolfPrefab;
		case CreatureType.Flower: return flowerPrefab;
		case CreatureType.Beaver: return beaverPrefab;
		case CreatureType.Dolphin: return dolphinPrefab;
		case CreatureType.Alligator: return alligatorPrefab;
		case CreatureType.Lion: return lionPrefab;
		case CreatureType.Camel: return camelPrefab;
		default: throw new ArgumentException("Illegal creature type: " + creature, "creature");
		}
	}
}
