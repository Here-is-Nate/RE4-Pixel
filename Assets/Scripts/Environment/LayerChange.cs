using UnityEngine;

public class LayerChange : MonoBehaviour
{
    private enum ELayerType {
        OBJECT,
        ENEMY,
        PLAYER
    }

    [SerializeField] private ELayerType layerType;

    void OnTriggerEnter2D(Collider2D collider) {
        switch(layerType) {
            case ELayerType.OBJECT: CheckObjectCollision(collider, true); break;
            case ELayerType.ENEMY: CheckEntityCollision(collider); break;
            case ELayerType.PLAYER: CheckEntityCollision(collider); break;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        switch(layerType) {
            case ELayerType.OBJECT: CheckObjectCollision(collider, false); break;
            case ELayerType.ENEMY: CheckEntityCollision(collider); break;
            case ELayerType.PLAYER: CheckEntityCollision(collider); break;
        }
    }

    #region Collision Handlers
    /// <summary>
    /// Checks the collision between object and entity
    /// </summary>
    /// <param name="collider">The 'it' collider</param>
    /// <param name="gate">A bool that says if the collider is Entering (true) or Exiting (false)</param>
    void CheckObjectCollision(Collider2D collider, bool gate) {
        if(collider.CompareTag("PlayerFeet")) {
            Player player = collider.GetComponentInParent<Player>();

            player.inAObject = gate;
            player.GetComponentInChildren<SpriteRenderer>().sortingOrder = gate ? 8 : 12;
            return;
        }

        if(collider.CompareTag("EnemyFeet")) {
            Enemy enemy = collider.GetComponentInParent<Enemy>();

            enemy.inAObject = gate;
            enemy.GetComponentInChildren<SpriteRenderer>().sortingOrder = gate ? 8 : 12;
            return;
        }
        
    }

    void CheckEntityCollision(Collider2D collider) {
        if(collider.CompareTag("PlayerFeet")) {
            // Player enters the Enemy
            Player player = collider.GetComponentInParent<Player>();
            Enemy enemy = GetComponentInParent<Enemy>();
            SpriteRenderer playerSpriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
            SpriteRenderer enemySpriteRenderer = enemy.GetComponentInChildren<SpriteRenderer>();

            if(player.inAObject || enemy.inAObject) playerSpriteRenderer.sortingOrder = 8;
            else playerSpriteRenderer.sortingOrder = 11;
            
            enemySpriteRenderer.sortingOrder = enemy.inAObject ? 9 : 12;
        }

        if(collider.CompareTag("EnemyFeet")) {
            Player player = GetComponentInParent<Player>();
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            SpriteRenderer playerSpriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
            SpriteRenderer enemySpriteRenderer = enemy.GetComponentInChildren<SpriteRenderer>();

            if(enemy.inAObject || player.inAObject) enemySpriteRenderer.sortingOrder = 8;
            else enemySpriteRenderer.sortingOrder = 11;

            playerSpriteRenderer.sortingOrder = player.inAObject ? 9 : 12;
        }
    }
    #endregion
}