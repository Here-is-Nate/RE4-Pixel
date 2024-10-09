using UnityEngine;

public class CheckObstacle : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float checkerRadius;
    [SerializeField] private LayerMask layerMasks;

    [Header("References")]
    private Enemy enemy;

    void Start() {
        enemy = GetComponentInParent<Enemy>();
    }

    void Update()
    {
        CheckColision();
    }

    public void CheckColision() {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, checkerRadius, layerMasks);

        if(hit == null) {
            enemy.colliding = false;
            return;
        }

        enemy.Colliding(hit.gameObject.layer, hit.transform);
        enemy.colliding = true;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, checkerRadius);
    }
}
