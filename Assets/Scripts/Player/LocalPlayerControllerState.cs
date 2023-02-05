using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


[System.Serializable]
public class LocalPlayerControllerState : MonoBehaviour
{
	private static LocalPlayerControllerState _instance;

	public static LocalPlayerControllerState Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<LocalPlayerControllerState>();
			}

			return _instance;
		}
	}

	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}

	}

	#region Properties
	public CharacterController playerController;
	public Camera playerCamera;
	public PlayerInput playerInput;
	private Vector2 rotation = Vector2.zero;
	private float currentGravity = -9.8f;
	private float currentSpeed = 3;
	private Vector3 currentMovement = Vector3.zero;
	private bool obstacleAbove;
	private bool sprinting = false;
	float turnSmoothVelocity;
	private bool canJump = true;
	private bool isCrouching = false;
	private float originalHeight, originalCameraY, originalFOV;

	public GameObject GunHolder;

	private Vector2 moveInput;
	private Vector2 lookInput;

	private bool grounded;
	#endregion

	#region input stuff
	private InputAction look;
	private InputAction move;
	private InputAction crouch;
	private InputAction sprint;
	private InputAction jump;

	#endregion

	#region Fields
	public float lookSensitivity = 3;
	public float playerMaxSpeed = 4.5f;
	public float playerMaxAirSpeed = 4.5f;
	public float playerAcceleration = 2f;
	public float airAcceleration = 1.5f;
	public float playerMaxCrouchSpeed = 2.5f;
	public float playerCrouchAcceleration = 1f;
	public float playerTerminalVelocity = -9.8f;
	public float playerMaxSprintSpeed = 6.5f;
	public float playerSprintAcceleration = 3f;
	public float playerFallSpeed = 0.5f;
	public float sprintEase = 0.25f;
	public float FOVEase = .8f;
	public float baseInputEase = 1f;
	public float fallingInputEase = .25f;
	public float turnSmoothness = .25f;
	public bool hasGravity = true;
	public float jumpSpeed = 3;
	public float jumpCooldown = 0.25f;
	public float sprintSpeed = 6;
	public float bobSpeed, bobHeight;
	public float sprintFOVKick = 1.5f;
	public float explosionEase = 1f;
	public float friction = .025f;
	public Vector3 playerCurrentVelocity = new Vector3(0, 0, 0);
	public Vector3 externalForce = Vector3.zero;
	public Animator anim;
	private float smoothedSpeed;


	public LayerMask jumpMask;
	#endregion



	void Start()
	{
		//psm.TPSGraphic.SetActive(false);

		playerController = gameObject.GetComponent<CharacterController>();
		currentGravity = playerTerminalVelocity;
		currentSpeed = playerMaxSpeed;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		originalHeight = playerController.height;

	}

	void Update()
	{

		lookSensitivity = SettingsManager.Instance.lookSensitivity;
		crouch = playerInput.actions["Crouch"];
		sprint = playerInput.actions["Sprint"];
		move = playerInput.actions["Move"];
		look = playerInput.actions["Look"];
		jump = playerInput.actions["Jump"];

		smoothedSpeed = Mathf.Lerp(smoothedSpeed, playerCurrentVelocity.normalized.magnitude, (.75f * Time.deltaTime) * 4f);

		anim.SetFloat("PlayerSpeed", smoothedSpeed);

		UpdateMoveInput(move.ReadValue<Vector2>());
		UpdateLookInput(look.ReadValue<Vector2>());

		if (jump.IsPressed())
		{
			Jump();
		}
		if (sprint.triggered)
		{
			sprinting = true;
		}
		if (!crouch.IsPressed())
		{
			isCrouching = false;
		}
		else
		{
			isCrouching = true;
		}

		MovePlayer();
		Look();

	}

	void FixedUpdate()
	{
		GroundCheck();
	}

	#region Input Methods

	public void UpdateMoveInput(Vector2 _move)
	{
		moveInput = _move;
	}

	public void UpdateLookInput(Vector2 _move)
	{
		lookInput = _move;
	}
	public Vector2 GetLookInput()
	{
		return lookInput;
	}



	#endregion


	#region Movement Methods
	private void Look()
	{

		rotation.y += lookInput.x * lookSensitivity * Time.deltaTime;
		rotation.x += lookInput.y * lookSensitivity * Time.deltaTime;
		rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
		gameObject.transform.eulerAngles = new Vector2(0, rotation.y);
		playerCamera.transform.localRotation = Quaternion.Euler(-rotation.x, 0f, 0f);

		//psm.UpdateAimRot(psm.vCam.transform.localRotation);
		//psm.UpdateFlashRot(psm.vCam.transform.localRotation);

		//psm.anim.SetFloat("MovX", lookInput.x * 3);
	}

	void Gravity()
	{
		playerCurrentVelocity.y += playerTerminalVelocity * Time.deltaTime;
		//playerCurrentVelocity.y = Mathf.Clamp(yVelocity.y, playerGravity, 2000f);
		if (grounded) playerCurrentVelocity.y = Mathf.Max(playerCurrentVelocity.y, 0);
	}
	void OnControllerColliderHit(ControllerColliderHit c)
	{
		if (!grounded)
		{

			playerCurrentVelocity *= .1f;
		}
	}
	private void GroundMove()
	{
		float deltaX;
		float deltaZ;

		float maxSpeedThisFrame = playerMaxSpeed;
		float accelerationThisFrame = playerAcceleration;
		Crouch();
		Sprint();

		if (sprinting)
		{
			maxSpeedThisFrame = playerMaxSprintSpeed;
			accelerationThisFrame = playerSprintAcceleration;
		}
		if (isCrouching)
		{
			maxSpeedThisFrame = playerMaxCrouchSpeed;
			accelerationThisFrame = playerCrouchAcceleration;
		}

		if (moveInput.x == 0 && moveInput.y == 0 && playerCurrentVelocity.x == 0 && playerCurrentVelocity.z == 0) return;



		// gives directions relative to where the player is facing
		Vector3 direction = (new Vector3(moveInput.x, moveInput.y));
		Vector3 move = (direction.x * gameObject.transform.right + direction.y * gameObject.transform.forward).normalized;
		playerCurrentVelocity += move * accelerationThisFrame;
		//frictino this time i swear
		Vector2 playerCurrentVelocityAsV2 = new Vector2(playerCurrentVelocity.x, playerCurrentVelocity.z);
		//need to represent the players velocity as a vector2 here so it doesn't touch vertical momentum
		playerCurrentVelocityAsV2 = Vector2.MoveTowards(playerCurrentVelocityAsV2, Vector2.zero, friction);
		if (Mathf.Abs(playerCurrentVelocityAsV2.magnitude) > playerMaxSpeed)
		{
			playerCurrentVelocityAsV2 = playerCurrentVelocityAsV2.normalized * maxSpeedThisFrame;
		}
		deltaX = playerCurrentVelocityAsV2.x - playerCurrentVelocity.x;
		deltaZ = playerCurrentVelocityAsV2.y - playerCurrentVelocity.z;

		playerCurrentVelocity.x += deltaX;
		playerCurrentVelocity.z += deltaZ;
		//animator stuff and other boring nonsense
		//psm.fpsAnim.SetFloat("Speed", direction.magnitude);
		//psm.anim.SetFloat("MovX", move.magnitude);


	}
	private void AirMove()
	{
		Vector3 playerCurrentVelocityTemp = playerCurrentVelocity;
		float deltaX;
		float deltaZ;


		float accelerationThisFrame = airAcceleration;
		Crouch();
		Sprint();

		// gives directions relative to where the player is facing
		Vector3 direction = (new Vector3(moveInput.x, moveInput.y));
		Vector3 move = (direction.x * gameObject.transform.right + direction.y * gameObject.transform.forward).normalized;
		playerCurrentVelocityTemp += move * accelerationThisFrame;
		Vector2 playerCurrentVelocityAsV2 = new(playerCurrentVelocityTemp.x, playerCurrentVelocityTemp.z);

		//need to represent the players velocity as a vector2 here so it doesn't touch vertical momentum
		// if (Mathf.Abs(playerCurrentVelocityAsV2.magnitude) > playerMaxAirSpeed)
		// {
		// 	playerCurrentVelocityAsV2 = playerCurrentVelocityAsV2.normalized * playerMaxAirSpeed;
		// }

		deltaX = playerCurrentVelocityAsV2.x - playerCurrentVelocity.x;
		deltaZ = playerCurrentVelocityAsV2.y - playerCurrentVelocity.z;
		if (playerCurrentVelocityAsV2.x < -playerMaxAirSpeed || playerCurrentVelocityAsV2.x > playerMaxAirSpeed)
		{
			deltaX = 0;
		}
		if (playerCurrentVelocityAsV2.y < -playerMaxAirSpeed || playerCurrentVelocityAsV2.y > playerMaxAirSpeed)
		{
			deltaZ = 0;
		}
		
		
		
		
		
		playerCurrentVelocity.x += deltaX;
		playerCurrentVelocity.z += deltaZ;
		//animator stuff and other boring nonsense
		//psm.fpsAnim.SetFloat("Speed", direction.magnitude);
		//psm.anim.SetFloat("MovX", move.magnitude);


	}
	private void MovePlayer()
	{

		if (hasGravity)
		{
			Gravity();
		}
		if (grounded)
		{
			GroundMove();
		}
		if (!grounded)
		{
			AirMove();
		}

		playerController.Move((playerCurrentVelocity) * Time.deltaTime);

	}

	public void Explode(Vector3 force)
	{
		playerCurrentVelocity += force;
	}
	public void Jump()
	{
		if (canJump)
		{
			playerCurrentVelocity.y = Mathf.Sqrt((jumpSpeed) * -3.0f * playerTerminalVelocity);

		}

	}

	public void SprintInput()
	{
		if (!isCrouching && moveInput.y > .1f)
		{
			sprinting = true;
		}
	}

	void OnDrawGizmos()
	{
		float radius = playerController.radius * 0.9f;
		Vector3 pos = playerController.transform.position - (new Vector3(0, (playerController.height - playerController.height / 1.5f), 0));
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(pos, radius);
	}

	private void GroundCheck()
	{
		float radius = playerController.radius * 0.9f;
		Vector3 pos = playerController.transform.position - (new Vector3(0, (playerController.height - playerController.height / 1.5f), 0));
		if (Physics.CheckSphere(pos, radius, ~jumpMask))
		{
			grounded = true;
			canJump = true;
		}
		else
		{
			grounded = false;
			canJump = false;
		}
		if (Physics.CheckSphere(pos, radius, LayerMask.GetMask("MovingPlatform")))
		{
			Collider[] c = Physics.OverlapSphere(pos, radius, LayerMask.GetMask("MovingPlatform"));
			foreach(Collider a in c) {
				if (a.gameObject.layer == 11) {
					playerController.Move(a.gameObject.GetComponent<MovingPlatform>().velocity * Time.fixedDeltaTime);
				}
			}
		}
	}
	private void Sprint()
	{
		if (sprinting && moveInput.y < .1f)
		{
			sprinting = false;
		}

		if (sprinting && !isCrouching)
		{
			currentSpeed = Mathf.Lerp(currentSpeed, sprintSpeed, sprintEase);
			//playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOVKick, FOVEase - .1f);

			if (grounded)
			{
				playerCamera.transform.localPosition = new Vector3(0.0f, Mathf.Sin(Time.time * bobSpeed) * bobHeight + originalCameraY, 0.0f);
			}
		}
		else
		{
			currentSpeed = Mathf.Lerp(currentSpeed, playerMaxSpeed, sprintEase);
			//playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, originalFOV, FOVEase);

			if (!isCrouching)
			{
				playerCamera.transform.localPosition = new Vector3(0, originalCameraY, 0);
			}
		}
	}

	public void CrouchInput(bool _bool)
	{
		isCrouching = _bool;
	}


	private void Crouch()
	{
		if (isCrouching)
		{

		}

		if (isCrouching && !sprinting)
		{
			playerController.height = (originalHeight / 2) + .50f;
			// playerController.center = new Vector3(0, .6f, 0);
			playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, new Vector3(playerCamera.transform.localPosition.x, (playerCamera.transform.localPosition.y / 2) - .25f, playerCamera.transform.localPosition.z), .25f);
		}
		else
		{
			playerController.height = originalHeight;
			//playerController.center = new Vector3(0, 1, 0);
			playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, new Vector3(playerCamera.transform.localPosition.x, originalCameraY, playerCamera.transform.localPosition.z), .25f);
		}


		//anim.SetBool("Crouch", isCrouching);

		int layerMask = 1 << 9;
		layerMask = ~layerMask;

		// RaycastHit hit;
		// if ((Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.up), out hit, originalHeight, layerMask)))
		// {
		//    //Debug.Log("hit");
		//    obstacleAbove = true;
		// }
		// else
		// {
		//    obstacleAbove = false;
		//    //isCrouching = false;
		// }


	}

	#endregion
	#region State Management
	public void Die()
	{
		Respawn();
	}
	public void Respawn()
	{
		GameObject spawnPoint = RespawnPoint.Instance.gameObject;
		gameObject.transform.position = spawnPoint.transform.position;
		GetComponent<PlayerTraits>().health = GetComponent<PlayerTraits>().maxHealth;
		GetComponentInChildren<WeaponManager>().ReloadAllWeapons();
	}
	#endregion
}


