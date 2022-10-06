using System;
using System.Collections.Generic;
using UnityEngine;


public static class PlayerManager
{
	private static int currentPlayer = 0;
	private static readonly List<PlayerController> playerControllers = new();

	public static void Register(PlayerController p)
	{
		playerControllers.Add(p);
		p.Active = (playerControllers.Count == 1);
	}

	public static void Unregister(PlayerController p)
	{
		p.Active = false;
		playerControllers.Remove(p);
	}

	public static void SetActive(PlayerController p)
	{
		int index = playerControllers.IndexOf(p);
		if (index != -1)
			throw new InvalidOperationException("player not managed");

		playerControllers[currentPlayer].Active = false;
		currentPlayer = index;
		playerControllers[currentPlayer].Active = true;

	}
	public static void Next()
	{
		playerControllers[currentPlayer].Active = false;
		currentPlayer = (currentPlayer +1 )%playerControllers.Count;
		playerControllers[currentPlayer].Active = true;
	}
}
