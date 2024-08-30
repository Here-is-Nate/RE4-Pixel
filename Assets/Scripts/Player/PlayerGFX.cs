using UnityEngine;

public class PlayerGFX : MonoBehaviour
{
    [Header("References")]
    private Player player;
    private PlayerAim playerAim;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GetComponentInParent<Player>();
        playerAim = player.GetComponentInChildren<PlayerAim>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        SetAnimatorParameters();

        if(player.isAiming) {
            FlipSpriteByAim();
            SetActiveAnimatorLayer(1);
        } else {
            FlipSpriteByMovement();
            SetActiveAnimatorLayer(0);
        }
    }

    #region Animator Handler
    void FlipSpriteByMovement() {
        if(player.movement.x < 0) spriteRenderer.flipX = true;
        if(player.movement.x > 0) spriteRenderer.flipX = false;
    }

    void FlipSpriteByAim() {
        if(playerAim.lookAtAngle > 202.5f && playerAim.lookAtAngle <= 337.5f) spriteRenderer.flipX = true;
        if(playerAim.lookAtAngle > 337.5f || playerAim.lookAtAngle <= 202.5f) spriteRenderer.flipX = false;
    }

    int GetTransition() {
        if(player.isAiming) {
            if(player.isWalking) return 4;
            else return 3;
        }
        if(player.isRunning) return 2;
        if(player.isWalking) return 1;
        
        return 0;
    }

    void SetAnimatorParameters() {
        animator.SetInteger("Transition", GetTransition());

        if(player.isAiming) {
            animator.SetFloat("Horizontal", playerAim.GetLookAtCoordinate().x);
            animator.SetFloat("Vertical", playerAim.GetLookAtCoordinate().y);

            return;
        }

        if(player.MovementKeyPressed()) {
            animator.SetFloat("Horizontal", player.softMovement.x);
            animator.SetFloat("Vertical", player.softMovement.y);
        }
    }

    void SetActiveAnimatorLayer(int activeIndex) {
        for(int i = 0; i < animator.layerCount; i++) {
            animator.SetLayerWeight(i, i == activeIndex ? 1f : 0f);
        }
    }
    #endregion
}
