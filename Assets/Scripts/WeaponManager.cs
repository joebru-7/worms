using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	public IWeapon[] weapons;
	[SerializeField]
	int _selected;
	private IWeapon _currentWeapon;

	public int Selected { get => _selected; set => _selected = value%(weapons.Length); }

	// Start is called before the first frame update
	public void Init(ref IWeapon w)
	{
		if (Selected >= weapons.Length)
			throw new IndexOutOfRangeException("Selected weapon greater than numer of weapons");
		CreateWeapon(Selected);
		w = _currentWeapon;
	}

	void CreateWeapon(int num)
	{
		_currentWeapon = Instantiate(weapons[num],transform);
	}

	public void SwitchWeapon(ref IWeapon w)
	{
		Destroy(_currentWeapon.gameObject);
		Selected++;
		CreateWeapon(Selected);
		w = _currentWeapon;
	}
}
