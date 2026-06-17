using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _velocity = 1f;
    [SerializeField] private float _jumpForce = 1f;
    [SerializeField] private float _rotatingSensX = 10f;
    [SerializeField] private float _rotatingSensY = 10f;
    [Header("Components")]
    private Rigidbody _rigidbody;
    [Header("Input")]
    private PlayerActionSystem _actionMap;
    [Header("Camera")]
    private Camera _camera;
    [Header("Layer mask")]
    [SerializeField] private LayerMask _layerMask;

    float _yaw = 0f;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        _actionMap = new();
    }
    private void OnEnable() => _actionMap.Enable();
    private void OnDisable() => _actionMap.Disable();
    private void FixedUpdate()
    {
        MovementUpdate();
        RotationUpdate();
        Jump();
    }
    private void RotationUpdate()
    {
        _yaw += Input.GetAxis("Mouse X") * _rotatingSensX;
        _rigidbody.MoveRotation(Quaternion.Euler(0, _yaw, 0));
    }
    private void MovementUpdate()
    {
        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;
        forward.y = 0;
        right.y = 0;

        Vector2 input = Vector2.zero;

        if (_actionMap.PlayerActionMap.Forward.IsPressed())
        {
            input.x = 1;
        }
        if (_actionMap.PlayerActionMap.Back.IsPressed())
        {
            input.x = -1;
        }
        if (_actionMap.PlayerActionMap.Right.IsPressed())
        {
            input.y = 1;
        }
        if (_actionMap.PlayerActionMap.Left.IsPressed())
        {
            input.y = -1;
        }
        Vector3 velocity = forward * input.x + right * input.y;
        velocity = velocity.normalized;
        velocity *= _velocity;
        velocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = velocity;
    }
    private void Jump()
    {
        if (_actionMap.PlayerActionMap.Jump.IsPressed() && IsGrounded())
        {
            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, _jumpForce, _rigidbody.linearVelocity.z);
        }
    }
    private bool IsGrounded()
    {
        BoxCollider collider = GetComponent<BoxCollider>();

        return Physics.Raycast(transform.position, Vector3.down, collider.bounds.size.y/2 + 0.01f, _layerMask);
    }
}
