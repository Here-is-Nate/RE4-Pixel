using UnityEngine;

/// <summary>
/// This is the Main class of the player, but it'll be used to controll
/// The player movements only
/// </summary>
public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rigidBody2D;

    // Enumerators
    /// <summary>
    /// Provides two types of run, holding or toggle the run button
    /// </summary>
    private enum ERunType {
        Toggle,
        Hold
    }

    /// <summary>
    /// Provides two behaviors when aiming, freeze while aiming or can moving while aiming
    /// </summary>
    private enum EMoveAimType {
        Stop,
        Move
    }

    /// <summary>
    /// Provides two types os aim, holding the aim button or just pressing
    /// </summary>
    private enum EAimingType {
        Hold,
        Toggle
    }

    /// <summary>
    /// Provides two types of movement, one stop when the keys are stop pressed, the other are more
    /// softly, stoping gradually along the time
    /// </summary>
    private enum EMovementType {
        Hard,
        Soft
    }

    /// <summary>
    /// Game Configurations, after, I want to put it in a JSon file
    /// </summary>
    [Header("Configs")]
    // Movement Type & Speed
    [SerializeField] private ERunType _runType;
    private float _moveSpd;
    private float _walkSpd = 2f;
    private float _runSpd = 4f;

    // Aim Move Type & Speed
    [SerializeField] private EMoveAimType _aimMoveType;
    private float _aimSpd = 1f;

    // Aim Type
    [SerializeField] private EAimingType _aimType;

    // Movemente Tyoe
    [SerializeField] private EMovementType _movementType;


    [Header("Inputs")]
    private Vector2 _movement;
    // Used to control the player stop, to allow it to stop on diagonal and axis
    private Vector2 _softMovement;
    // Used to allow change the movement type in the game, to a soft or hard movement
    private Vector2 _curMovement;

    [Header("Controllers")]
    [SerializeField] private bool _isWalking;
    [SerializeField] private bool _isRunning;
    [SerializeField] private bool _isAiming;

    #region Properties
    public Vector2 movement {get {return _movement;}}
    public Vector2 softMovement {get {return _softMovement;}}
    public Vector2 curMovement {get {return _curMovement;} set {_curMovement = value;}}
    public bool isWalking {get {return _isWalking;}}
    public bool isRunning {get {return _isRunning;}}
    public bool isAiming {get {return _isAiming;}}
    #endregion

    void Start() {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        OnInput();
        OnRun();
        OnAim();
    }

    void FixedUpdate() {
        OnMove();
    }

    #region Inputs
    void OnInput() {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _softMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(Input.GetKeyDown(KeyCode.LeftShift)) _isRunning = !_isRunning;
        if(Input.GetKeyUp(KeyCode.LeftShift) && _runType == ERunType.Hold) _isRunning = false;

        if(Input.GetKeyDown(KeyCode.Mouse1)) _isAiming = !_isAiming;
        if(Input.GetKeyUp(KeyCode.Mouse1) && _aimType == EAimingType.Hold) _isAiming = false;
    }
    #endregion

    #region Movement
    void OnMove() {
        curMovement = _movementType == EMovementType.Hard ? _movement : _softMovement;
        rigidBody2D.MovePosition(rigidBody2D.position + curMovement * _moveSpd * Time.fixedDeltaTime);

        // Block Move Speed Change if the player is aiming
        if(_isAiming) return;
        
        _moveSpd = _isRunning ? _runSpd : _walkSpd;
        _isWalking = _isRunning ? false : IsMoving();
    }

    void OnRun() {
        if(_isRunning) _isWalking = false;

        if(!IsMoving()) _isRunning = false;
    }

    void OnAim() {
        if(_isAiming) {
            _isRunning = false;
            _isWalking = false;

            // Set if you can move or not when aiming
            switch(_aimMoveType) {
                case EMoveAimType.Stop: _moveSpd = 0f; break;
                case EMoveAimType.Move: _moveSpd = _aimSpd; break;
            }

        }
    }
    
    public bool MovementKeyPressed(){
        return _movement.sqrMagnitude > 0;
    }

    public bool IsMoving() {
        return curMovement.sqrMagnitude > 0;
    }
    #endregion
}