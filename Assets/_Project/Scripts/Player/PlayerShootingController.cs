using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerActionQueue playerActionQueue;
    [SerializeField] private PlayerCameraController playerCameraController;
    [SerializeField] private InputActionReference shootActionReference;
    [SerializeField] private GameObject shootParticle;
    [SerializeField][Tooltip("This should be cinemachine brain")] private Camera playerCamera;

    [Header("Shooting Settings")]
    [SerializeField] private bool canShootOnlyWhileAiming = false;
    [SerializeField][Min(0.1f)] private float shootCooldown = 0.5f;
    [SerializeField] private float shootRange = 100f;
    [SerializeField] private LayerMask shootableLayers;
    private float _lastShootTime = 0f;
    private bool _hasShot;
    private void OnEnable()
    {
        shootActionReference.action.performed += TryShoot;
    }

    private void OnDisable()
    {
        shootActionReference.action.performed -= TryShoot;
    }

    public void TryShoot(InputAction.CallbackContext context)
    {
        playerActionQueue.ExecuteAction(CanShoot, Shoot);
    }

    private bool CanShoot()
    {
        if (canShootOnlyWhileAiming && !playerCameraController.IsAiming)
        {
            Debug.Log("Cannot shoot: Player is not aiming.");
            return false;
        }
        return _lastShootTime >= shootCooldown;
    }
    private void Update()
    {
        _lastShootTime += Time.deltaTime;
        if (_hasShot && _lastShootTime >= shootCooldown)
        {
            _hasShot = false;
            Debug.Log("Player shot!");
        }
    }

    public void Shoot()
    {
        Debug.Log("Player shot!");
        _hasShot = true;
        _lastShootTime = 0f;
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hitInfo, shootRange, shootableLayers);

        if (hitInfo.collider != null)
        {
            Debug.Log($"Hit object: {hitInfo.collider.name}");
            shootParticle.transform.position = hitInfo.point;
            shootParticle.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            shootParticle.SetActive(true);
            // Implement hit logic here
        }
        else
        {
            Debug.Log("No object hit.");
        }
        // Implement shooting logic here
    }

    private void OnDrawGizmos()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * shootRange);
        }
    }
}
