using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpController : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpInput;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private int jumpNumber = 1;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private ForceMode jumpForceMode = ForceMode.Impulse;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private int _currentJumpCount;
    public bool IsGrounded { get; private set; }

    private void Start()
    {
        jumpInput.action.performed += TryJump;
        _currentJumpCount = 0;
    }

    private void Update()
    {
        Vector3 origin = transform.position + Vector3.up * groundCheckRadius;
        IsGrounded = Physics.SphereCast(origin, groundCheckRadius, Vector3.down, out RaycastHit groundHit, groundCheckDistance + 0.01f, groundLayer, QueryTriggerInteraction.Ignore);

        if (IsGrounded && _currentJumpCount != 0)
        {
            _currentJumpCount = 0;
        }

        Debug.Log($"[PlayerJumpController] Is Grounded: {IsGrounded} | Current Jump Count: {_currentJumpCount}");
    }

    private void OnDestroy()
    {
        jumpInput.action.performed -= TryJump;
    }
    private void TryJump(InputAction.CallbackContext context)
    {
        if (_currentJumpCount < jumpNumber)
        {
            _currentJumpCount++;
            Debug.Log($"Jumping! Current jump count: {_currentJumpCount}");
            playerRb.AddForce(Vector3.up * jumpForce, jumpForceMode);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Vector3 origin = transform.position + Vector3.up * groundCheckRadius;
        Gizmos.DrawWireSphere(origin + Vector3.down * groundCheckDistance, groundCheckRadius);
    }
}
