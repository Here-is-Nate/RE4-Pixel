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


    [Header("Inputs")]
    private Vector2 _movement;

    [Header("Controllers")]
    [SerializeField] private bool _isRunning;
    [SerializeField] private bool _isAiming;

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

        if(Input.GetKeyDown(KeyCode.LeftShift)) _isRunning = !_isRunning;
        if(Input.GetKeyUp(KeyCode.LeftShift) && _runType == ERunType.Hold) _isRunning = false;

        if(Input.GetKeyDown(KeyCode.Mouse1)) _isAiming = !_isAiming;
        if(Input.GetKeyUp(KeyCode.Mouse1) && _aimType == EAimingType.Hold) _isAiming = false;
    }
    #endregion

    #region Movement
    void OnMove() {
        rigidBody2D.MovePosition(rigidBody2D.position + _movement * _moveSpd * Time.fixedDeltaTime);

        // Block Move Speed Change if the player is aiming
        if(_isAiming) return;

        _moveSpd = _isRunning ? _runSpd : _walkSpd;
    }

    void OnRun() {
        if(_movement.sqrMagnitude <= 0) _isRunning = false;
    }

    void OnAim() {
        if(_isAiming) {
            _isRunning = false;

            switch(_aimMoveType) {
                case EMoveAimType.Stop: _moveSpd = 0f; break;
                case EMoveAimType.Move: _moveSpd = _aimSpd; break;
            }

        }
    }
    #endregion
}
