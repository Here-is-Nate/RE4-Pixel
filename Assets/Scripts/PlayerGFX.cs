using UnityEngine;

public class PlayerGFX : MonoBehaviour
{
    [Header("References")]
    private Player player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        SetAnimatorParameters();

        FlipSprite();
    }

    #region Animator Handler
    void FlipSprite() {
        if(player.movement.x < 0) spriteRenderer.flipX = true;
        if(player.movement.x > 0) spriteRenderer.flipX = false;
    }

    int GetTransition() {
        if(player.isRunning) return 2;
        if(player.isWalking) return 1;
        
        return 0;
    }

    void SetAnimatorParameters() {
        animator.SetInteger("Transition", GetTransition());

        if(player.MovementKeyPressed()) {
            animator.SetFloat("Horizontal", player.softMovement.x);
            animator.SetFloat("Vertical", player.softMovement.y);
        }
    }
    #endregion
}
