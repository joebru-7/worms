using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

	//[SerializeField] [range]
	private CharacterController _characterController;
	private Vector2 _moveValue;
	[SerializeField] public float moveSpeed;
	[SerializeField] public IWeapon weapon;

	// Start is called before the first frame update
	void Start()
	{
		_characterController = GetComponent<CharacterController>();
	}

	public void Move(InputAction.CallbackContext context)
	{
		
		_moveValue = context.ReadValue<Vector2>();
		//Debug.Log(_moveValue);
	}

	public void Shoot(InputAction.CallbackContext context)
	{
		if(weapon == null)
		{
			Debug.Log("Weapon Null");
			return;
		}

		var isPress = context.started;
		//Debug.Log(isPress);

		if (isPress)
		{
			weapon.Shoot();
			//Debug.Log("Pew");
		}
	}

	// Update is called once per frame
	void Update()
	{
		_characterController.Move(new Vector3(_moveValue.x, -.1f, _moveValue.y) * (moveSpeed * Time.deltaTime));
	
	}
}
