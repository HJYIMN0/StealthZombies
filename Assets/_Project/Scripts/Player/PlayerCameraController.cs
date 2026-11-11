using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private InputActionReference aimingInput;

    [Header("Camera settings")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject aimingCamera;

    [Header("Gameplay Settings")]
    [SerializeField] private PlayerJumpController playerJumpController;
    [SerializeField] private bool canShootWhileJumping = true;

    public bool IsAiming => aimingCamera.activeSelf;

    public Camera ActiveCamera => IsAiming ? aimingCamera.GetComponent<Camera>() : mainCamera.GetComponent<Camera>();
    public UnityEvent OnCameraChanged;

    private void Start()
    {
        mainCamera.SetActive(true);
        aimingCamera.SetActive(false);
    }

    private void Update()
    {
        if (aimingInput.action.IsPressed())
        {
            if (!playerJumpController.IsGrounded && !canShootWhileJumping)
                return;

            if (aimingCamera.activeSelf)
                return;
            mainCamera.SetActive(false);
            aimingCamera.SetActive(true);
            OnCameraChanged?.Invoke();
        }
        else
        {
            if (mainCamera.activeSelf)
                return;
            mainCamera.SetActive(true);
            aimingCamera.SetActive(false);
            OnCameraChanged?.Invoke();
        }
    }

}