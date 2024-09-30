using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [Header("Movement Config")]
    [SerializeField] private float movementSpd;
    private Vector2 _movement;
    private bool _isMoving;

    #region Properties
    public Vector2 movement {get { return _movement; }}
    public bool isMoving {get {return _isMoving;}}
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

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, fieldOfViewRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopChaseFieldRadius);
    }

    void ChasePlayer() {
        // float zCache = transform.position.z;
        // transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpd * Time.deltaTime);
        // transform.position = new Vector3(transform.position.x, transform.position.y, zCache);
        _movement = player.transform.position - transform.position;

        float xMovement = _movement.x != 0f ? _movement.x / Math.Abs(_movement.x) : 0f;
        float yMovement = _movement.y != 0f ? _movement.y / Math.Abs(_movement.y) : 0f;

        rigidBody2D.MovePosition(rigidBody2D.position + new Vector2(xMovement, yMovement) * movementSpd * Time.fixedDeltaTime);
    }

    public void GetDamage(float damage) {
        Debug.Log("Tomou Dano! " + damage);
    }
}
