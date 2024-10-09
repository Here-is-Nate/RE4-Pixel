using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    private Player player;
    private Rigidbody2D rigidBody2D;

    [Header("Field of View Config")]
    [SerializeField] private float fieldOfViewRadius;
    [SerializeField] private float stopChaseFieldRadius;
    [SerializeField] private LayerMask playerMask;

    [Header("Chasing Config")]
    private bool isChasing;
    private Vector2 exitDirection;

    [Header("Movement Config")]
    [SerializeField] private float movementSpd;
    private Vector2 _movement;
    private bool _isMoving;

    [Header("Looking Settings")]
    private Vector2 _evenLookAtCoordinate;
    private int lookAtIndex;
    public bool colliding;

    #region Properties
    public Vector2 movement {get { return _movement; }}
    public bool isMoving {get {return _isMoving;}}
    public Vector2 evenLookAtCoordinate {get {return _evenLookAtCoordinate;}}
    #endregion

    void Start() {
        player = FindObjectOfType<Player>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        FieldOfView();

        if(isChasing) ChasePlayer();
        
        _isMoving = isChasing;
    }

    void FieldOfView() {
        float hitRadius;

        if(!isChasing) hitRadius = fieldOfViewRadius;
        else hitRadius = stopChaseFieldRadius;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, hitRadius, playerMask);

        if(hit && !isChasing) {
            isChasing = true;
            return;
        }

        if(!hit && isChasing) isChasing = false;
    
    }

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

    public void ChasePlayer() {
        if (!colliding) {
            _movement = (player.transform.position - transform.position).normalized;

            Move(_movement);
        } else Move(exitDirection);
    }

    void Move(Vector2 direction) {
        rigidBody2D.MovePosition(rigidBody2D.position + direction * movementSpd * Time.fixedDeltaTime);
    }

    public void GetDamage(float damage) {
        Debug.Log("Tomou Dano! " + damage);
    }

    public void Colliding(LayerMask layer, Transform colliderPosition) {
        if(LayerMask.LayerToName(layer) == "Obstacle") DodgeObstacle(colliderPosition);
    }

    void DodgeObstacle(Transform obstacle) {
        GetLookAtCoordinate();

        int complementaryIndex = lookAtIndex + 2;

        if(complementaryIndex > 7) complementaryIndex -= 8;

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

        exitDirection = lookCoordinates[complementaryIndex];
    }
}
