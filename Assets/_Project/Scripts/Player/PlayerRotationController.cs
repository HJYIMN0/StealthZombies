using Unity.Android.Gradle.Manifest;
using Unity.AppUI.Core;
using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private float maxRotationSpeed = 2f;
    [SerializeField][Range(10f, 180f)] private float rotationCap = 45f;

    public bool IsRotating { get; private set; }
    private Vector3 playerDirection => playerMovementController.MovementDirection;
    private Quaternion targetRotation;

    private void Update()
    {
        if (playerDirection.sqrMagnitude < 0.001f)
            return;

        targetRotation = Quaternion.LookRotation(playerDirection);

        float angle = Quaternion.Angle(transform.rotation, targetRotation);

        IsRotating = angle > rotationCap;

        Debug.Log($"Angle: {angle:F2} | IsRotating: {IsRotating}");

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            maxRotationSpeed * Time.deltaTime
        );
    }
    private void EvaluateRotation()
    {
        float angle = Quaternion.Angle(transform.rotation, targetRotation);

        IsRotating = angle > rotationCap;
    }
}
