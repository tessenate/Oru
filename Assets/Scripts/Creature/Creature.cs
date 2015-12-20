﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A creature represents a single movable unit in the game, whether friendly or not.
/// </summary>
public class Creature : MonoBehaviour
{
	// The type of creature this is.
	public CreatureType creatureType;

	public Coordinate? Goal { get; set; }

	public Coordinate Position { get; private set; }

	public Action<Coordinate?> OnSetGoal;

	private CreatureController manager;
	private Coordinate nextPosition;

	private bool isMoving = false;
	
	public bool HasAbility()
	{
		return GetComponent<IAbility>() != null;
	}
	
	public IAbility Ability
	{
		get { return GetComponent<IAbility>(); }
	}
	
	// Convenience method to get the creature's definition
	public CreatureDefinition Definition
	{
		get { return Creatures.ForType(creatureType); }
	}

	void Start()
	{
		// Store our initial position
		nextPosition = Position = GameManager.gm.ToGridCoordinate(gameObject.transform.position);
	}

	void OnMouseDown()
	{
		GetComponentInParent<CreatureController>().OnSelect(this);
	}

	public void Update()
	{
		// Animate the creature moving
		var stepInterval = GameManager.gm.stepInterval;
		var cellSize = GameManager.gm.cellSize;

		var ratio = (Time.timeSinceLevelLoad % stepInterval) / stepInterval;
		var direction = nextPosition - Position;
		// TODO I'm sure we can factor this out
		var translation = new Vector3(direction.x, 0, direction.z) * ratio * cellSize;
		GameManager.gm.SetPosition(gameObject, Position);
		transform.position += translation;
	}

	public void Step()
	{
		Position = nextPosition;
		if (Goal != null && (!Position.Equals(Goal)))
		{
			nextPosition = NextCoordinate ();

			// Make our creature face the right direction
			if (nextPosition != Position)
			{
				var direction = nextPosition - Position;
				transform.Rotate(new Vector3(0, AngleFor(direction)) - transform.rotation.eulerAngles);
			}

			// Animate our creature if it has animation
			if (GetComponentInChildren<Animator>() && !isMoving)
			{
				isMoving = true;
				Debug.Log("Animating the creature moving.");
				GetComponentInChildren<Animator>().SetTrigger("StartMove");
			}
		}
		else
		{
			if (GetComponentInChildren<Animator>() && isMoving)
			{
				isMoving = false;
				Debug.Log("Animating the creature stopping.");
				GetComponentInChildren<Animator>().SetTrigger("StopMove");
			}
		}
	}

	// Returns true if this creature can reach the specified goal coordinate
	public bool CanReach(Coordinate goal)
	{
		var parents = DoBFS(Position, goal, IsValidCoordinate);
		return parents.Keys.Contains(goal);
	}

	private float AngleFor(Coordinate direction)
	{
		if (direction == Coordinate.up) { return -90; }
		if (direction == Coordinate.right) { return 0; }
		if (direction == Coordinate.down) { return 90; }
		if (direction == Coordinate.left) { return 180; }
		throw new ArgumentException("Given coordinate" + direction + " is not a direction", "direction");
	}

	private delegate bool CoordinatePredicate(Coordinate coordinate);
	private static IDictionary<Coordinate, Coordinate> DoBFS(Coordinate start, Coordinate end, CoordinatePredicate neighborPredicate)
	{
		// Initialize BFS data structures
		var distance = new Dictionary<Coordinate, int>();
		var parents = new Dictionary<Coordinate, Coordinate> ();
		var queue = new Queue<Coordinate> ();

		// Input our start coordinate
		distance [start] = 0;
		queue.Enqueue (start);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			var neighbors = Coordinate.cardinals.Select(x => x + current).ToList();
			foreach (Coordinate neighbor in neighbors)
			{
				if (!distance.ContainsKey(neighbor) && neighborPredicate(neighbor))
				{
					distance[neighbor] = distance[current] + 1;
					parents[neighbor] = current;
					queue.Enqueue(neighbor);
				}
			}
			// If we've already found the last coordinate, we do not need to keep searching.
			if (neighbors.Contains(end))
			{
				break;
			}
		}
		return parents;
	}

	// do a BFS and figure out the right path
	private Coordinate NextCoordinate()
	{
		// If the creature has no goal, just stick to the current position
		// TODO maybe make the creature's initial goal its position?
		if (Goal == null)
		{
			return Position;
		}
		var goal = Goal ?? Position;
		var parents = DoBFS(Position, goal, IsValidCoordinate);

		Coordinate next;
		// If we can reach the goal, then move the creature to the next step towards that goal
		if (parents.ContainsKey(goal))
		{
			next = goal;
			while (parents[next] != Position)
			{
				next = parents[next];
			}
			return next;
		}

		// Otherwise, do another BFS not accounting for terrain restrictions and try to move the creature there
		parents = DoBFS(Position, goal, GameManager.Terrain.Contains);
		// TODO assert that the level is fully connected
		next = goal;
		while (parents[next] != Position)
		{
			next = parents[next];
		}
		return IsValidCoordinate(next) ? next : Position;
	}

	// Returns true if we the creature is allowed to go to this coordinate
	private bool IsValidCoordinate(Coordinate coordinate)
	{
		return GameManager.Terrain.Contains(coordinate)
			&& Definition.AllowedTerrain.Contains(GameManager.Terrain[coordinate])
			&& !GameManager.Creatures.CreatureList.Any(x => x != this && x.Position == coordinate);
	}

}
