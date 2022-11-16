using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	[SerializeField]
	private IWeapon[] _Weapons;
	[SerializeField] 
	private IWeapon   _currentWeapon;

	private int _selected;
	private int Selected { get => _selected; set => _selected = value%(_Weapons.Length); }

	public void Init(ref IWeapon w)
	{
		if (Selected >= _Weapons.Length)
			throw new IndexOutOfRangeException("Selected weapon greater than number of weapons");
		CreateWeapon(Selected);
		w = _currentWeapon;
	}

	void CreateWeapon(int num)
	{
		_currentWeapon = Instantiate(_Weapons[num],transform);
	}

	public void SwitchWeapon(ref IWeapon w)
	{
		Destroy(_currentWeapon.gameObject);
		Selected++;
		CreateWeapon(Selected);
		w = _currentWeapon;
	}
}
