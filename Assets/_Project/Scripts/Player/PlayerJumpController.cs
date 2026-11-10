using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerJumpController : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpInput;
    [SerializeField] private PlayerActionQueue playerActionQueue;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private int jumpNumber = 1;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private ForceMode jumpForceMode = ForceMode.Impulse;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField][Range(0f, 1f)] private float coyoteTime = 0.2f;

    private int _currentJumpCount;
    public bool IsGrounded { get; private set; }
    private float _coyoteTimeCounter;

    private void Start()
    {
        jumpInput.action.performed += TryJump;
        _currentJumpCount = 0;
    }
    private bool CanJump()
    {
        Vector3 origin = transform.position + Vector3.up * groundCheckRadius;

        IsGrounded = Physics.SphereCast(
            origin,
            groundCheckRadius,
            Vector3.down,
            out RaycastHit groundHit,
            groundCheckDistance,
            groundLayer
        );

        if (IsGrounded)
        {
            _currentJumpCount = 0;
            _coyoteTimeCounter = 0f;
            return true;
        }

        if (_currentJumpCount == 0)
        {
            _coyoteTimeCounter += Time.deltaTime;
            return _coyoteTimeCounter <= coyoteTime;
        }

        return false;
    }

    private void OnDestroy()
    {
        jumpInput.action.performed -= TryJump;
    }
    private void TryJump(InputAction.CallbackContext context)
    {
        //if (!CanJump()) return;
        playerActionQueue.ExecuteAction(CanJump, Jump);
    }
    private void Jump()
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
