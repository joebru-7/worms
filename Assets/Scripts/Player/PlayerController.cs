using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	//event readers
	private Vector2 _moveValue;
	private Vector2 _lockValue;

	//movement
	public  float MoveSpeed;
	public  float JumpStrength = 5;
	public  float RotationSpeed = 1;
	public  float gravity = 9.82f;
	private float _rotation = 0;
	private float _fallspeed = 0;

	//components
	private CharacterController _characterController;
	private Camera _camera;
	private AudioListener _audio;
	private PlayerInput _input;

	//weapons
	private WeaponManager _weaponManager;
	private IWeapon _weapon;
	private bool _hasFiered;
	
	//health
	public  int  Health = 100;
	private bool _dead = false;

	[SerializeField] private bool _active = false;
	public bool Active
	{
		get => _active;
		set
		{
			_active = value;
			_camera.enabled = _active;
			_input.enabled = _active;
			_audio.enabled = _active;
			if (_active)
			{
				_hasFiered = false;
			}
		}

	}

	public void Damage(int amt)
	{
		Health -= amt;
		if (Health <= 0 && !_dead)
		{
			_dead = true;
			PlayerManager.Unregister(this);
			transform.Rotate(90, 0, 0);

			if (PlayerManager.NumPlayers <= 1)
			{
				PlayerManager.GenerateWinScreen();
				if(PlayerManager.NumPlayers == 0)
				{
					_camera.enabled = true;
				}
			}
		}

	}


	void Start()
	{
		_characterController = GetComponent<CharacterController>();
		_camera = GetComponentInChildren<Camera>(true);
		_audio = GetComponentInChildren<AudioListener>(true);

		_weaponManager = GetComponentInChildren<WeaponManager>();
		_weaponManager.Init(ref _weapon);

		_input = GetComponent<PlayerInput>();

		PlayerManager.Register(this);
	}

	void Update()
	{
		//fall
		if (!_characterController.isGrounded)
		{
			_fallspeed += gravity * Time.deltaTime;
			_characterController.Move(Vector3.down * (_fallspeed * Time.deltaTime));
		}
		else
		{
			_fallspeed = 0;
		}

		// if not active player don't do anything more
		if (!_active)
			return;

		//rotate
		var newRotation = _moveValue.x * RotationSpeed * Time.deltaTime;
		_rotation += newRotation;
		transform.Rotate(Vector3.up, Mathf.Rad2Deg * newRotation);

		//move
		var moveVector = new Vector3(_moveValue.y * Mathf.Sin(_rotation), -.1f, _moveValue.y * Mathf.Cos(_rotation)) * (MoveSpeed * Time.deltaTime);
		_characterController.Move(moveVector);

		//aim
		_weapon.Aim(_lockValue.y * Time.deltaTime);
	}

	//Event handelers
	public void Move(InputAction.CallbackContext context)
	{
		_moveValue = context.ReadValue<Vector2>();
	}
	public void Look(InputAction.CallbackContext context)
	{
		_lockValue = context.ReadValue<Vector2>();
	}

	public void Shoot(InputAction.CallbackContext context)
	{
		if (!_active || _hasFiered)
			return;
		if (_weapon == null)
		{
			throw new NullReferenceException("Weapon is null");
		}

		var isPress = context.started;

		if (isPress)
		{
			_weapon.Shoot();
			_hasFiered = true;

			Invoke(nameof(NextPlayer), 1);
		}
	}

	void NextPlayer()
	{
		PlayerManager.Next();
	}

	public void Switch(InputAction.CallbackContext context)
	{
		if (context.started && Active)
		{
			_weaponManager.SwitchWeapon(ref _weapon);
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
