using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{

   public float xSens;
   public float ySens;

   public PlayerInput playerInput;
   private InputAction look;

   public Transform orientation;

   float xRotation;
   float yRotation;

   void Start()
   {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
      look = playerInput.actions["Look"];
   }

   // Update is called once per frame
   void Update()
   {
      float mouseX = look.ReadValue<Vector2>().x * Time.deltaTime * xSens;
      float mouseY = look.ReadValue<Vector2>().y * Time.deltaTime * ySens;

      yRotation += mouseX;
      xRotation -= mouseY;
      xRotation = Mathf.Clamp(xRotation, -90f, 90f);

      transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
      orientation.rotation = Quaternion.Euler(0, yRotation, 0);

   }
}
