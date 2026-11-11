using Unity.VisualScripting;
using UnityEngine;

public class AimingUIController : MonoBehaviour
{
    [SerializeField] private PlayerCameraController playerCameraController;
    private bool isAiming => playerCameraController.IsAiming;

    /// <summary>
    /// This method must be connected to PlayerCameraController UnityEvent OnCameraChanged
    /// </summary>
    public void SwitchUiVisibility()
    {
        gameObject.SetActive(isAiming);
    }
}
