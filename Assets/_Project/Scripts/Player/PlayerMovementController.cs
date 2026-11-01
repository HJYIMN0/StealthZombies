using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController playerCharacterController;
    [SerializeField] private PlayerRotationController playerRotationController;
    [SerializeField] private InputActionReference movementInput;
    [SerializeField][Range(1.1f, 100f)] private float MaxSpeed = 2f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deceleration = 2f;
    private float _currentPlayerSpeed;
    public Vector3 MovementDirection { get; private set; }
    public Vector2 PlayerInput { get; private set; }

    private void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogWarning("You forgot to assign camera transform! Searching for camera.main");
            cameraTransform = Camera.main.transform;
        } 
    }

    private void Update()
    {
        PlayerInput = ReadInput();
        DetermineDirection();
        DeterminePlayerSpeed();
        MoveCharacter();
    }

    private Vector2 ReadInput()
    {
        Vector2 movementInputValue = movementInput.action.ReadValue<Vector2>();

        return movementInputValue.normalized;
    }

    private void DetermineDirection()
    {
        if (PlayerInput.sqrMagnitude < 0.001f) return;
        
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Eliminiamo la componente verticale per mantenere il movimento sul piano orizzontale.
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        MovementDirection = cameraForward * PlayerInput.y + cameraRight * PlayerInput.x;
        
    }

    private void MoveCharacter()
    {
        Debug.Log($"Input: {PlayerInput} | Direction: {MovementDirection} | Speed: {_currentPlayerSpeed} | Rotating: {playerRotationController.IsRotating}");

        if (playerRotationController.IsRotating)
            return;

        playerCharacterController.Move(MovementDirection * _currentPlayerSpeed * Time.deltaTime);
    }

    private void DeterminePlayerSpeed()
    {
        if (PlayerInput.sqrMagnitude < 0.001f)
        {
            _currentPlayerSpeed = Mathf.MoveTowards(_currentPlayerSpeed, 0f, deceleration * Time.deltaTime);
        }
        else
        {
            _currentPlayerSpeed = Mathf.MoveTowards(_currentPlayerSpeed, MaxSpeed, acceleration * Time.deltaTime);
        }
    }
}
