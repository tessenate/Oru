using System;
using System.Collections.Generic;

/// <summary>
/// Statically defines the creatures available in this game.
/// </summary>
public static class CreatureDefinitions
{
	public static readonly CreatureDefinition Crane = new CreatureDefinition
	{
		Type = CreatureType.Crane,
		Description = "The basic origami crane",
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 1} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Water }
	};
	
	public static readonly CreatureDefinition Turtle = new CreatureDefinition
	{
		Type = CreatureType.Turtle,
		Description = "A seabound unit that can carry resources",
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Green, 1} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Water },
		Ability = new CarryResourceAbility.Definition { Capacity = 5 }
	};

	public static readonly CreatureDefinition Horse = new CreatureDefinition
	{
		Type = CreatureType.Horse,
		Description = "A versatile land unit that can carry resources",
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Red, 4} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Rock },
		Ability = new CarryResourceAbility.Definition { Capacity = 5 }
	};

	public static readonly CreatureDefinition Elephant = new CreatureDefinition
	{
		Type = CreatureType.Elephant,
		Description = "A large unit that can uproot and move trees",
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 9} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land },
		Ability = new ChangeTerrainAbility.Definition
		{
			CarryType = TerrainType.Tree,
			LeaveType = TerrainType.Land
		}
	};

	public static readonly CreatureDefinition Crab = new CreatureDefinition
	{
		Type = CreatureType.Horse,
		Description = "A basic enemy creature",
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Red, 1} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land },
		Ability = new FightAbility.Definition { Attack = 5, Defense = 5 },
		IsEnemy = true
	};

	public static readonly CreatureDefinition Wolf = new CreatureDefinition
	{
		Type = CreatureType.Wolf,
		Description = "Can fight enemies",
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 4} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Rock },
		Ability = new FightAbility.Definition { Attack = 10, Defense = 5 }
	};

	public static CreatureDefinition ForType(CreatureType type)
	{
		switch(type)
		{
		case CreatureType.Crane: return Crane;
		case CreatureType.Turtle: return Turtle;
		case CreatureType.Horse: return Horse;
		case CreatureType.Elephant: return Elephant;
		case CreatureType.Crab: return Crab;
		case CreatureType.Wolf: return Wolf;
		default: throw new ArgumentException("Passed in an invalid creature type: " + type, "type");
		}
	}

}
