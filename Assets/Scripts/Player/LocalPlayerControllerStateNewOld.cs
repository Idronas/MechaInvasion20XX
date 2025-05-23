using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


[System.Serializable]
public class LocalPlayerControllerStateNewOld : MonoBehaviour
{
   [Header("Movement")]
   private float moveSpeed;
   public float walkSpeed;
   public float sprintSpeed;

   public float groundDrag;

   [Header("Jumping")]
   public float jumpForce;
   public float jumpCooldown;
   public float airMultiplier;
   bool readyToJump;

   [Header("Crouching")]
   public float crouchSpeed;
   public float crouchYScale;
   private float startYScale;

   [Header("Keybinds")]
   public PlayerInput playerInput;
   private InputAction move;
   private InputAction crouch;
   private InputAction sprint;
   private InputAction jump;
   private float lookSensitivity;

   [Header("Ground Check")]
   public float playerHeight;
   public LayerMask whatIsGround;
   bool grounded;

   [Header("Slope Handling")]
   public float maxSlopeAngle;
   private RaycastHit slopeHit;
   private bool exitingSlope;


   public Transform orientation;

   float horizontalInput;
   float verticalInput;

   Vector3 moveDirection;

   Rigidbody rb;

   public MovementState state;
   public enum MovementState
   {
      walking,
      sprinting,
      crouching,
      air
   }

   private void Start()
   {
      rb = GetComponent<Rigidbody>();
      rb.freezeRotation = true;

      readyToJump = true;

      startYScale = transform.localScale.y;
   }

   private void Update()
   {
      // ground check
      grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

      MyInput();
      SpeedControl();
      StateHandler();

      // handle drag
      if (grounded)
         rb.drag = groundDrag;
      else
         rb.drag = 0;
   }

   private void FixedUpdate()
   {
      MovePlayer();
   }

   private void MyInput()
   {

      lookSensitivity = SettingsManager.Instance.lookSensitivity;
      crouch = playerInput.actions["Crouch"];
      sprint = playerInput.actions["Sprint"];
      move = playerInput.actions["Move"];
      jump = playerInput.actions["Jump"];

      horizontalInput = move.ReadValue<Vector2>().x;
      verticalInput = move.ReadValue<Vector2>().y;

      // when to jump
      if (jump.triggered && readyToJump && grounded)
      {
         readyToJump = false;

         Jump();

         Invoke(nameof(ResetJump), jumpCooldown);
      }

      // start crouch
      if (crouch.triggered)
      {
         transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
         rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
      }

      // stop crouch
      if (!crouch.WasReleasedThisFrame())
      {
         transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
      }
   }

   private void StateHandler()
   {
      // Mode - Crouching
      if (crouch.IsPressed())
      {
         state = MovementState.crouching;
         moveSpeed = crouchSpeed;
      }

      // Mode - Sprinting
      else if (grounded && sprint.IsPressed())
      {
         state = MovementState.sprinting;
         moveSpeed = sprintSpeed;
      }

      // Mode - Walking
      else if (grounded)
      {
         state = MovementState.walking;
         moveSpeed = walkSpeed;
      }

      // Mode - Air
      else
      {
         state = MovementState.air;
      }
   }

   private void MovePlayer()
   {
      // calculate movement direction
      moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

      // on slope
      if (OnSlope() && !exitingSlope)
      {
         rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

         if (rb.velocity.y > 0)
            rb.AddForce(Vector3.down * 80f, ForceMode.Force);
      }

      // on ground
      else if (grounded)
         rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

      // in air
      else if (!grounded)
         rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

      // turn gravity off while on slope
      rb.useGravity = !OnSlope();
   }

   private void SpeedControl()
   {
      // limiting speed on slope
      if (OnSlope() && !exitingSlope)
      {
         if (rb.velocity.magnitude > moveSpeed)
            rb.velocity = rb.velocity.normalized * moveSpeed;
      }

      // limiting speed on ground or in air
      else
      {
         Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

         // limit velocity if needed
         if (flatVel.magnitude > moveSpeed)
         {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
         }
      }
   }

   private void Jump()
   {
      exitingSlope = true;

      // reset y velocity
      rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

      rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
   }
   private void ResetJump()
   {
      readyToJump = true;

      exitingSlope = false;
   }

   private bool OnSlope()
   {
      if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
      {
         float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
         return angle < maxSlopeAngle && angle != 0;
      }

      return false;
   }

   private Vector3 GetSlopeMoveDirection()
   {
      return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
   }

   public void Explode(Vector3 force)
   {
      rb.AddForce(force, ForceMode.Impulse);
   }

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


