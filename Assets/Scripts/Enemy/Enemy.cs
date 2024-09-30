using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    private Player player;

    [Header("Field of View Config")]
    [SerializeField] private float fieldOfViewRadius;
    [SerializeField] private float stopChaseFieldRadius;
    [SerializeField] private LayerMask playerMask;

    [Header("Chasing Config")]
    [SerializeField] private NavMeshAgent navMeshAgent;
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

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
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
        // transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpd * Time.deltaTime);
        navMeshAgent.SetDestination(player.transform.position);

        _movement = player.transform.position - transform.position;
    }

    public void GetDamage(float damage) {
        Debug.Log("Tomou Dano! " + damage);
    }
}
