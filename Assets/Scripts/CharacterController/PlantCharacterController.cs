using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlantCharacterController : MonoBehaviour
{
    #region Definitions
    public enum Characterstates
    {
        GroundedLocomotion = 0,
        Airborne = 1,
        Climbing = 2,
        Rooted = 3,
        Dead = 4,
    }

    [System.Serializable]
    public struct StateChanged
    {
        public Characterstates From, To;

        public StateChanged(Characterstates from, Characterstates to)
        {
            From = from;
            To = to;
        }
    }

    // can be made readonly for build, but for debug leave as modifiable struct
    [System.Serializable]
    public struct Breadcrump
    {
        public Characterstates Characterstate;
        public Vector2 Position;
        public Vector2 Velocity;

        public Breadcrump(Vector2 position, Vector2 velocity, Characterstates characterstates)
        {
            Position = position;
            Velocity = velocity;
            Characterstate = characterstates;
        }
    }
    #endregion

    #region References
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Plant_Base_Env _plantPrefab;
    [SerializeField] private CharacterControllerSettings _settings;
    #endregion References   

    #region Bookkeeping    
    private Game _app;

    [Header("Serialized for debug purposes. DO NOT mess around with this.")]
    [SerializeField] private Characterstates _characterstate = Characterstates.GroundedLocomotion;

    /// <summary>
    /// Event for the Animator to listen to. Allows to develop movement and animation completely separately.
    /// </summary>
    public UnityEvent<StateChanged> OnCharacterStateChanged;

    [SerializeField] private LayerMask _groundCheckLayermask;
    [SerializeField] private LayerMask _climbLayerMask;

    private Vector2 _movementInput;

    private float _jumpBuffer;
    private float _rootBuffer;
    private float _coyoteTime;

    public bool JumpWasPressed => _jumpBuffer > 0f;
    public bool RootWasPressed => _rootBuffer > 0f;
    public bool CoyoteTime => _coyoteTime > 0f;
    #endregion Bookkeeping

    public void Setup(Game app)
    {
        _app = app;
        _rb.freezeRotation = true;
        _rb.gravityScale = 0;
    }

    #region Inputs
    public void SetMovementInput(Vector2 input)
    {
        _movementInput = input;
    }

    public void PressJump()
    {
        _jumpBuffer = _settings.InputBufferTime;
    }

    public void PressRoot()
    {
        _rootBuffer = _settings.InputBufferTime;
    }

    public void KillPlant()
    {
        _app.GameOver();
        Stop();

        Characterstates old = _characterstate;
        _characterstate = Characterstates.Dead;

        OnCharacterStateChanged?.Invoke(new(old, _characterstate));
    }

    public void TryTakeRootOnWaterOrKillplant()
    {
        // case we hit something we shouldn't have.
        if (!_app.CanAdvanceToNextPlant())
        {
            KillPlant();
        }
        else
        {
            _app.CurrentPlantTakeRoot();
        }
    }

    public void TakeRoot()
    {
        // whatever we need to do to take root. Mainly disabling behaviour script... ?
        Debug.Log($"{this.name} took root");
        gameObject.SetActive(false);

        Characterstates old = _characterstate;
        _characterstate = Characterstates.Rooted;

        OnCharacterStateChanged?.Invoke(new(old, _characterstate));
        Instantiate(_plantPrefab, transform.position, transform.rotation);
    }

    internal void StartFromBreadcrump(Breadcrump breadcrump)
    {
        Debug.Log($"{this.name} started at {breadcrump.Position} with velocity {breadcrump.Velocity} in state {breadcrump.Characterstate}");
        this.gameObject.SetActive(true);

        _rb.linearVelocity = breadcrump.Velocity;
        _rb.position = breadcrump.Position;
        _characterstate = breadcrump.Characterstate;
    }
    #endregion Inputs   

    #region Update
    void Update()
    {
        _jumpBuffer = Mathf.Max(_jumpBuffer - Time.deltaTime, 0);
        _rootBuffer = Mathf.Max(_rootBuffer - Time.deltaTime, 0);
        _coyoteTime = Mathf.Max(_coyoteTime - Time.deltaTime, 0);
    }

    void FixedUpdate()
    {
        HandleStateChanges();

        _app.PushBreadcrump(
            new Breadcrump(
                position: _rb.position,
                velocity: _rb.linearVelocity,
                _characterstate
            ));

        // tick
        switch (_characterstate)
        {
            case Characterstates.GroundedLocomotion:
                {
                    // slowly approach the desired velocity of the playercharacter, linear velocity change.
                    bool isreversing = _movementInput.x * _rb.linearVelocityX < 0f;

                    // add additional acceleration when changing direction to make the controller feel more responsive. 
                    float reversingAccelerationBoost = isreversing ? _settings.ReversingFactor : 1f;
                    _rb.linearVelocityX = Mathf.MoveTowards(_rb.linearVelocityX, _movementInput.x * _settings.TopSpeed * reversingAccelerationBoost, Time.deltaTime * _settings.Acceleration);

                    // apply downward acceleration
                    _rb.linearVelocityY = Mathf.MoveTowards(_rb.linearVelocityY, _settings.MaxFallSpeed, Mathf.Abs(_settings.GravityDown * Time.deltaTime));
                }
                break;

            case Characterstates.Airborne:
                {
                    if (_movementInput.x != 0)
                    {
                        float sign = Mathf.Sign(_movementInput.x);
                        _rb.linearVelocityX = Mathf.MoveTowards(_rb.linearVelocityX, _settings.TopSpeed * sign, Time.deltaTime * _settings.AirFriction * _settings.Acceleration);
                    }

                    // apply downward acceleration
                    bool movingUp = _rb.linearVelocityY > 0;
                    // use two different gravities to make the jump feel more bouncy. a "correct" fall usually feels too floaty. So heavier down arc makes it feel better.
                    float effectiveGravity = movingUp ? _settings.GravityUp : _settings.GravityDown;
                    _rb.linearVelocityY = Mathf.MoveTowards(_rb.linearVelocityY, _settings.MaxFallSpeed, Mathf.Abs(effectiveGravity * Time.deltaTime));
                }
                break;

            case Characterstates.Climbing:
                {
                    // while climbing we're not subject to gravity, and only change the velocity to whatever the input demands.
                    _rb.linearVelocityX = Mathf.MoveTowards(_rb.linearVelocityX, _movementInput.x * _settings.TopSpeed, Time.deltaTime * _settings.Acceleration);
                    _rb.linearVelocityY = Mathf.MoveTowards(_rb.linearVelocityY, _movementInput.y * _settings.TopSpeed, Time.deltaTime * _settings.Acceleration);
                }
                break;

            case Characterstates.Rooted:
                gameObject.SetActive(false);
                break;

            case Characterstates.Dead:
                // apply downward acceleration. Ensures we sink or do whatever when dead.
                _rb.linearVelocityY = Mathf.MoveTowards(_rb.linearVelocityY, _settings.MaxFallSpeed, Mathf.Abs(_settings.GravityDown * Time.deltaTime));
                _rb.linearVelocityX = Mathf.MoveTowards(_rb.linearVelocityX, 0, Time.deltaTime * _settings.Acceleration);
                break;

            default:
                Debug.LogWarning($"Update tick in state: {_characterstate}");
                break;
        }
    }

    void HandleStateChanges()
    {
        RaycastHit2D groundHit = Physics2D.CircleCast(transform.position, .5f, Vector2.down, .52f, _groundCheckLayermask);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.02f);
        Characterstates oldState = _characterstate;

        bool climbColliderVisible = Physics2D.OverlapBox(transform.position, new Vector2(.5f, 1), 0, _climbLayerMask);

        if (_app.CanAdvanceToNextPlant() && RootWasPressed)
        {
            if (_plantPrefab.CanPlaceAtPosition(transform.position))
            {
                _rootBuffer = -1f;
                Debug.Log("wood needed.");
                _app.CurrentPlantTakeRoot();
                return;
            }
            else
            {
                Debug.Log("Can't place that there m'lord");
            }
        }

        switch (_characterstate)
        {
            case Characterstates.GroundedLocomotion:
                {
                    _coyoteTime = _settings.CoyoteTime;

                    if (climbColliderVisible && _movementInput.y > 0)
                    {
                        // hard stop vertical when climbing ... ?
                        _rb.linearVelocityY = 0;
                        _characterstate = Characterstates.Climbing;
                    }
                    else if (JumpWasPressed && groundHit)
                    {
                        _characterstate = Characterstates.Airborne;
                        Jump();
                    }
                    else if (!groundHit)
                    {
                        _characterstate = Characterstates.Airborne;
                    }
                }
                break;

            case Characterstates.Airborne:
                {
                    if (climbColliderVisible && _movementInput.y > 0)
                    {
                        // hard stop vertical when climbing ... ?
                        _rb.linearVelocityY = 0;
                        _characterstate = Characterstates.Climbing;
                    }
                    else if (JumpWasPressed && CoyoteTime)
                    {
                        _characterstate = Characterstates.Airborne;
                        Jump();
                    }
                    // begin jump, apply upwards impulse
                    else if (groundHit)
                    {
                        _characterstate = Characterstates.GroundedLocomotion;
                    }
                }
                break;

            case Characterstates.Climbing:
                {
                    // climbing resets coyote timer, case you fall out to the bottom by accident you can recover this way.
                    _coyoteTime = _settings.CoyoteTime;

                    // holding down on the ground on the bottom of a vine to exit climb
                    if (groundHit && _movementInput.y < 0)
                    {
                        _characterstate = Characterstates.GroundedLocomotion;
                    }
                    else if (JumpWasPressed && CoyoteTime)
                    {
                        _characterstate = Characterstates.Airborne;
                        Jump();
                    }
                    // in the air on the vine, noop
                    else if (!climbColliderVisible)
                    {
                        // in the air not on the vine, start falling.
                        _characterstate = Characterstates.Airborne;
                    }
                }
                break;

            case Characterstates.Dead:
                break;

            default:
                Debug.LogWarning($"Unhandled input in state: {_characterstate}");
                return;
        }

        if (oldState != _characterstate)
        {
            OnCharacterStateChanged?.Invoke(new(oldState, _characterstate));
        }
    }

    #endregion Update
    private void Jump()
    {
        _jumpBuffer = -1f;
        _coyoteTime = -1f;

        // without double jump we don't need to worry about upward velocity correction...
        float rawVelocity = Mathf.Sqrt(-2 * _settings.JumpHeight * _settings.GravityUp);
        _rb.linearVelocityY = rawVelocity;
    }

    internal void SuperJump()
    {
        _jumpBuffer = -1f;
        _coyoteTime = -1f;

        // without double jump we don't need to worry about upward velocity correction...
        float rawVelocity = Mathf.Sqrt(-2 * _settings.SuperJumpHeight * _settings.GravityUp);
        _rb.linearVelocityY = rawVelocity;

        var old = _characterstate;
        _characterstate = Characterstates.Airborne;

        OnCharacterStateChanged?.Invoke(new(old, _characterstate));
    }

    internal void Stop()
    {
        _movementInput = Vector2.zero;
        _jumpBuffer = -1f;
        _coyoteTime = -1f;
        _rootBuffer = -1f;
    }

    public bool CanTakeRoot()
    {
        return _plantPrefab.CanPlaceAtPosition(transform.position);
    }
}
