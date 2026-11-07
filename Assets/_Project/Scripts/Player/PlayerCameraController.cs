using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private InputActionReference aimingInput;

    [Header("Camera settings")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject aimingCamera;

    private bool IsAiming => aimingCamera.activeSelf;

    private void Start()
    {
        mainCamera.SetActive(true);
        aimingCamera.SetActive(false);
    }

    private void Update()
    {
        if (aimingInput.action.IsPressed())
        {
            if (aimingCamera.activeSelf)
                return;
            mainCamera.SetActive(false);
            aimingCamera.SetActive(true);
        }
        else
        {
            if (mainCamera.activeSelf)
                return;
            mainCamera.SetActive(true);
            aimingCamera.SetActive(false);
        }
    }

    private void OnAimingPerformed(InputAction.CallbackContext context)
    {
        if (context.action.inProgress)
        {
            mainCamera.SetActive(false);
            aimingCamera.SetActive(true);
        }
        else
        {
            mainCamera.SetActive(true);
            aimingCamera.SetActive(false);
        }
    }

}