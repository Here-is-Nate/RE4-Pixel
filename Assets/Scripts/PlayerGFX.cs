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
            animator.SetFloat("Horizontal", SetMovementCoordinateByAim().x);
            animator.SetFloat("Vertical", SetMovementCoordinateByAim().y);

            return;
        }

        if(player.MovementKeyPressed()) {
            animator.SetFloat("Horizontal", player.softMovement.x);
            animator.SetFloat("Vertical", player.softMovement.y);
        }
    }

    void SetActiveAnimatorLayer(int activeIndex) {
        for(int i = 0; i < animator.layerCount; i++) animator.SetLayerWeight(i, i == activeIndex ? 1f : 0f);
    }

    Vector2 SetMovementCoordinateByAim() {
        float lookingAngle = playerAim.lookAtAngle;

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
        int index = Mathf.FloorToInt((lookingAngle + 22.5f) / 45f) % 8;

        return lookCoordinates[index];
    }

    #endregion
}
