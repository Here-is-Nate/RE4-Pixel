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
        Vector2 direction = new Vector2(enemy.GetEvenLookAtCoordinate().x, enemy.GetEvenLookAtCoordinate().y);

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        weaponAnimator.SetFloat("Horizontal", direction.x);
        weaponAnimator.SetFloat("Vertical", direction.y);

        animator.SetInteger("Transition", enemy.isMoving ? 1 : 0);
    }
}
