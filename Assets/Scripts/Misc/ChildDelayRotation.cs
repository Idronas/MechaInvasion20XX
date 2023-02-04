using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChildDelayRotation : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 4f;

    [SerializeField]
    float maxTurn = 3f;

    public LocalPlayerControllerState player;

    private InputAction look;

    void Update()
    {
        look = player.playerInput.actions["Look"];

        Vector2 mouseInput = look.ReadValue<Vector2>();

        ApplyRotation(GetRotation(mouseInput));
    }

    Quaternion GetRotation(Vector2 mouse)
    {
        mouse = Vector2.ClampMagnitude(mouse, maxTurn);

        Quaternion rotX = Quaternion.AngleAxis(-mouse.y, Vector3.right);
        Quaternion rotY = Quaternion.AngleAxis(mouse.x, Vector3.up);

        Quaternion targetRot = rotX * rotY;

        return targetRot;
    }

    void ApplyRotation(Quaternion targetRot)
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, rotateSpeed * Time.deltaTime);
    }
}