using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

	//[SerializeField] [range]
	private CharacterController _characterController;
	private Vector2 _moveValue;
	private Vector2 _lockValue;

	private float _rotation = 0;
	[SerializeField] public float moveSpeed;
	public float rotationSpeed = 1;
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
	public void Look(InputAction.CallbackContext context)
	{
		_lockValue = context.ReadValue<Vector2>();
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
		var newRotation = _moveValue.x * rotationSpeed * Time.deltaTime;
		_rotation += newRotation;

		transform.Rotate(Vector3.up, Mathf.Rad2Deg*newRotation);

		var moveVector = new Vector3(_moveValue.y * Mathf.Sin(_rotation), -.1f, _moveValue.y * Mathf.Cos(_rotation)) * (moveSpeed * Time.deltaTime);
		
		_characterController.Move(moveVector);

		weapon.Aim(_lockValue.y * Time.deltaTime);

	}
}
