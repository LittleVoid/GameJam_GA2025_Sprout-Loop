using UnityEngine;

[CreateAssetMenu(menuName = "Project/Charactercontroller/Settings")]
public class CharacterControllerSettings : ScriptableObject
{
    [Header("Movement Definitions")]
    [SerializeField] private float _acceleration;

    [SerializeField, Range(0f, 1f)] private float _airFriction;

    [SerializeField] private float _topSpeed;

    [SerializeField, Tooltip("Additional acceleration when character velocity and input direction are in opposite directions. Only horizonal movement.")]
    private float _reversingFactor;

    [SerializeField, Tooltip("Friction applied when no movement input is pressed")]
    private float _stoppingfriction;


    [Header("Jumping")]
    [SerializeField] private float _coyoteTime;

    [SerializeField] private float _jumpInputBuffering;

    [SerializeField] private float _jumpHeight;
    [SerializeField] private int _superJumpHeight;

    [SerializeField] private float _gravityUp;
    [SerializeField] private float _gravityDown;
    [SerializeField] private float _maxFallSpeed;


    [Header("Follower Mechanic")]
    [SerializeField] private float _positionDeltaforBreadcrumps;


    [Header("GroundCheck")]
    [SerializeField] private float _groundCheckDistance;

    #region getter
    public float PositionDeltaforBreadcrumps => _positionDeltaforBreadcrumps;
    public float Acceleration => _acceleration;
    public float TopSpeed => _topSpeed;
    public float ReversingFactor => _reversingFactor;

    public float CoyoteTime => _coyoteTime;
    public float JumpHeight => _jumpHeight;
    public int SuperJumpHeight => _superJumpHeight;
    public float GravityUp => _gravityUp;
    public float GravityDown => _gravityDown;
    public float MaxFallSpeed => _maxFallSpeed;


    public float InputBufferTime => _jumpInputBuffering;
    public float AirFriction => _airFriction;
    public float Stoppingfriction => _stoppingfriction;
    public float GroundCheckDistance => _groundCheckDistance;
    #endregion
}