using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.AI;

public enum EnemyStates {
    Idle,
    Chasing,
    Dead,
}

public class Enemy : MonoBehaviour
{
    [Header("FSM")]
    private EnemyStates currentState;

    [Header("References")]
    private Player player;
    private Rigidbody2D rigidBody2D;

    [Header("Field of View Config")]
    [SerializeField] private float fieldOfViewRadius;
    [SerializeField] private float stopChaseFieldRadius;
    [SerializeField] private LayerMask playerMask;


    [Header("Movement Config")]
    [SerializeField] private float movementSpd;
    private NavMeshAgent navMeshAgent;
    private Vector2 _movement;
    private bool _isMoving;

    /// <summary>
    /// Used only by LayerChange Script, it controlls if the entity is colliding with a object, that changes
    /// the behavior when the entity collide with another entity
    /// </summary>
    [Header("Layer Status")]
    private bool _inAObject;
    public bool inAObject {get {return _inAObject;} set { _inAObject = value; }}

    [Header("Looking Settings")]
    private Vector2 _evenLookAtCoordinate;
    private int lookAtIndex;

    #region Properties
    public Vector2 movement {get { return _movement; }}
    public bool isMoving {get {return _isMoving;}}
    public Vector2 evenLookAtCoordinate {get {return _evenLookAtCoordinate;}}
    #endregion

    void Start() {
        player = FindObjectOfType<Player>();
        rigidBody2D = GetComponent<Rigidbody2D>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    void Update() {
        switch(currentState) {
            case EnemyStates.Idle: Idle(); break;
            case EnemyStates.Chasing: Chasing(); break;
        }

        _isMoving = currentState == EnemyStates.Chasing ? true : false;
    }

    #region Finite State Machines
    void Idle() {
        if(FieldOfView(fieldOfViewRadius)) currentState = EnemyStates.Chasing;
    }
    void Chasing() {
        _movement = (player.transform.position - transform.position).normalized;
        // Move(_movement);
        navMeshAgent.speed = movementSpd;

        navMeshAgent.SetDestination(player.transform.position);

        if(!FieldOfView(stopChaseFieldRadius)) currentState = EnemyStates.Idle;
    }
    #endregion

    #region Action Handlers
    bool FieldOfView(float hitRadius) {
        return Physics2D.OverlapCircle(transform.position, hitRadius, playerMask);
    }

    public void GetDamage(float damage) {
        Debug.Log("Tomou Dano! " + damage);
    }
    #endregion

    #region Coordinate Handler
    /// <summary>
    /// Returns a coordinate x,y from player looking at
    /// </summary>
    public Vector2 GetLookAtCoordinate() {
        // Create a angle using the Tan of x,y, (180/PI) is used to transform radians to degrees
        float angle = Mathf.Atan2(_movement.x, _movement.y) * (180 / math.PI);

        // Used to not deny negative angles
        if (angle < 0) angle += 360;

        // Set the coordinates to use by a given angle
        Vector2[] lookCoordinates = new Vector2[] {
            new Vector2(0, 1),    // i:0 | A: 0 - 22.5   | 337.5 - 360
            new Vector2(1, 1),    // i:1 | A: 22.5 - 67.5
            new Vector2(1, 0),    // i:2 | A: 67.5 - 112.5
            new Vector2(1, -1),   // i:3 | A: 112.5 - 157.5
            new Vector2(0, -1),   // i:4 | A: 157.5 - 202.5
            new Vector2(-1, -1),  // i:5 | A: 202.5 - 247.5
            new Vector2(-1, 0),   // i:6 | A: 247.5 - 292.5
            new Vector2(-1, 1)    // i:7 | A: 292.5 - 337.5
        };

        // Get the current index in the array using the angle
        int index = Mathf.FloorToInt((angle + 22.5f) / 45f) % 8;

        _evenLookAtCoordinate = index % 2 != 0 ? lookCoordinates[index - 1] : lookCoordinates[index];
        lookAtIndex = index;

        return lookCoordinates[index];
    }

    /// <summary>
    /// Returns only odds coordinates of GetLookAtCoordinate
    /// </summary>
    public Vector2 GetEvenLookAtCoordinate() {
        GetLookAtCoordinate();
        return evenLookAtCoordinate;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, fieldOfViewRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopChaseFieldRadius);
    }
    #endregion
}
