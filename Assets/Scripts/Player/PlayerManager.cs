using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PlayerManager
{
	private static int _currentPlayer = 0;
	private static readonly List<PlayerController> _playerControllers = new();

	public static int NumPlayers => _playerControllers.Count;

	public static void Register(PlayerController p)
	{
		_playerControllers.Add(p);
		p.Active = (_playerControllers.Count == 1);
	}

	public static void Unregister(PlayerController p)
	{
		p.Active = false;

		if(_playerControllers.IndexOf(p) <= _currentPlayer)
		{
			_currentPlayer--;
			if (_currentPlayer == -1)
				_currentPlayer = NumPlayers-2;
		}

		_playerControllers.Remove(p);

	}

	public static void SetActive(PlayerController p)
	{
		int index = _playerControllers.IndexOf(p);
		if (index != -1)
			throw new InvalidOperationException("player not managed");

		_playerControllers[_currentPlayer].Active = false;
		_currentPlayer = index;
		_playerControllers[_currentPlayer].Active = true;

	}
	public static void Next()
	{
		if (_playerControllers.Count == 0)
			return;

		_playerControllers[_currentPlayer].Active = false;
		_currentPlayer = (_currentPlayer +1 )%_playerControllers.Count;
		_playerControllers[_currentPlayer].Active = true;
	}

	public static void GenerateWinScreen()
	{
		UnityEngine.Object.Instantiate(Resources.Load("WinScreen"));
	}
}
