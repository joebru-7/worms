using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public static class PlayerManager
{
	private static int currentPlayer = 0;
	private static List<PlayerController> playerControllers = new();

	public static void Register(PlayerController p) => playerControllers.Add(p);
	public static void Unregister(PlayerController p) => playerControllers.Remove(p);

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

	private Camera _camera;

	private int hp;

	[SerializeField] private bool _active = false;
	private bool _hasFiered;

	public bool Active { 
		get => _active;
		set {
			_active = value;
			UpdateActive();
		}

	}

	private void UpdateActive()
	{
		if (_active)
		{
			_camera.enabled = true;
			_hasFiered = false;
		}
		else
		{
			_camera.enabled = false;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		PlayerManager.Register(this);
		_characterController = GetComponent<CharacterController>();
		_camera = GetComponentInChildren<Camera>(true);
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
			Debug.Log("Weapon Null");
			return;
		}

		var isPress = context.started;
		//Debug.Log(isPress);

		if (isPress)
		{
			weapon.Shoot();
			//_hasFiered = true;
			//Debug.Log("Pew");
		}
	}



	// Update is called once per frame
	void Update()
	{
		if (!_active)
			return;

		var newRotation = _moveValue.x * rotationSpeed * Time.deltaTime;
		_rotation += newRotation;

		transform.Rotate(Vector3.up, Mathf.Rad2Deg*newRotation);

		var moveVector = new Vector3(_moveValue.y * Mathf.Sin(_rotation), -.1f, _moveValue.y * Mathf.Cos(_rotation)) * (moveSpeed * Time.deltaTime);
		
		_characterController.Move(moveVector);

		weapon.Aim(_lockValue.y * Time.deltaTime);

	}
}
