using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerRotationController : MonoBehaviour
{
    [Header("Input settings")]
    [SerializeField] private InputActionReference lookInput;

    [Header("References")]
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private GameObject aimingCamera;
    [SerializeField] private Rigidbody playerRb;

    [Header("Rotation speed settings")]
    [SerializeField] private float rotationSpeedOnMainCamera = 5f;
    [SerializeField] private float rotationSpeedOnAimingCamera = 5f;

    public bool IsRotating { get; private set; }
    private Vector3 playerDirection => playerMovementController.MovementDirection;
    private Quaternion targetRotation;

    private float _currentRotationSpeed;

    private void FixedUpdate()
    {
        EvaluateRotationType();
    }

    private void EvaluateRotationType()
    {
        switch (aimingCamera.activeSelf)
        {
            case true:
                RotatePlayerToCamera();
                break;
            case false:
                RotatePlayerToMovementDirection();
                break;
        }
    }

    /// <summary>
    /// Rotation when player is aiming
    /// </summary>
    private void RotatePlayerToCamera()
    {
        Vector3 cameraForward = aimingCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        targetRotation = Quaternion.LookRotation(cameraForward);
        playerRb.MoveRotation(targetRotation);
    }

    /// <summary>
    /// Rotation when player is not aiming
    /// </summary>
    private void RotatePlayerToMovementDirection()
    {
        if (playerDirection.sqrMagnitude < 0.001f)
            return;

        targetRotation = Quaternion.LookRotation(playerDirection);

        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        playerRb.rotation = Quaternion.Slerp(
            playerRb.rotation,
            targetRotation,
            rotationSpeedOnMainCamera * Time.fixedDeltaTime
        );
    }
}