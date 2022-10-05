using System;
using System.Collections;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

	//
	private Vector2 _moveValue;
	private Vector2 _lockValue;

	public float moveSpeed;
	public float JumpStrength = 5;
	public float rotationSpeed = 1;
	public float gravity = 9.82f;
	private float _rotation = 0;
	private float _fallspeed = 0;

	public int hp = 100;

	private CharacterController _characterController;
	private Camera _camera;
	private PlayerInput _input;

	private WeaponManager _weaponManager;
	private IWeapon weapon;
	private bool _hasFiered;


	[SerializeField] private bool _active = false;
	public bool Active
	{
		get => _active;
		set
		{
			_active = value;
			UpdateActive();
		}

	}

	private void UpdateActive()
	{
		_camera.enabled = _active;
		_input.enabled = _active;
		if (_active)
		{
			_hasFiered = false;
		}
		else
		{
		}
	}

	public void Damage(int amt)
	{
		hp -= amt;
	}

	// Start is called before the first frame update
	void Start()
	{
		PlayerManager.Register(this);
		_characterController = GetComponent<CharacterController>();
		_camera = GetComponentInChildren<Camera>(true);

		_weaponManager = GetComponentInChildren<WeaponManager>();
		_weaponManager.Init(ref weapon);

		_input = GetComponent<PlayerInput>();
	}

	void Update()
	{
		if (!_characterController.isGrounded)
		{
			_fallspeed += gravity * Time.deltaTime;
			_characterController.Move(Vector3.down * (_fallspeed * Time.deltaTime));
		}
		else
		{
			_fallspeed = 0;
		}

		if (!_active)
			return;

		var newRotation = _moveValue.x * rotationSpeed * Time.deltaTime;
		_rotation += newRotation;

		transform.Rotate(Vector3.up, Mathf.Rad2Deg * newRotation);

		var moveVector = new Vector3(_moveValue.y * Mathf.Sin(_rotation), -.1f, _moveValue.y * Mathf.Cos(_rotation)) * (moveSpeed * Time.deltaTime);

		_characterController.Move(moveVector);

		weapon.Aim(_lockValue.y * Time.deltaTime);
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
		if (!_active || _hasFiered)
			return;
		if (weapon == null)
		{
			throw new NullReferenceException("Weapon is null");
		}

		var isPress = context.started;
		//Debug.Log(isPress);

		if (isPress)
		{
			weapon.Shoot();
			_hasFiered = true;

			Invoke(nameof(nextPlayer), 1);
		}
	}

	void nextPlayer()
	{
		PlayerManager.Next();
	}

	public void Switch(InputAction.CallbackContext context)
	{
		if (context.started && Active)
		{
			_weaponManager.SwitchWeapon(ref weapon);
		}
	}

	public void Jump(InputAction.CallbackContext context)
	{
		if (context.started && Active && _characterController.isGrounded)
		{
			_characterController.Move(new Vector3(0, 0.01f, 0));
			_fallspeed = -JumpStrength;
		}
	}
}
