using UnityEngine;

public class EnemyGFX : MonoBehaviour
{
    [Header("References")]
    private Enemy enemy;
    private Animator animator;
    [SerializeField] private Animator weaponAnimator;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        SetAnimatorParameters();
    }

    void SetAnimatorParameters() {
        float horizontalMovement = enemy.movement.x;
        float verticalMovement = enemy.movement.y;

        animator.SetFloat("Horizontal", horizontalMovement);
        animator.SetFloat("Vertical", verticalMovement);

        weaponAnimator.SetFloat("Horizontal", horizontalMovement);
        weaponAnimator.SetFloat("Vertical", verticalMovement);

        animator.SetInteger("Transition", enemy.isMoving ? 1 : 0);
    }
}
