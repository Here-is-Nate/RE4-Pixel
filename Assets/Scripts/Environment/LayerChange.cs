using UnityEngine;

public class LayerChange : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("PlayerFeet")) {
            Transform playerPos = collider.GetComponentInParent<Player>().transform;
            SpriteRenderer spriteRenderer = playerPos.GetComponentInChildren<SpriteRenderer>();
            
            spriteRenderer.sortingOrder = 9;
            return;
        }

        if(collider.CompareTag("EnemyFeet")) {
            Transform enemyPos = collider.GetComponentInParent<Enemy>().transform;
            SpriteRenderer spriteRenderer = enemyPos.GetComponentInChildren<SpriteRenderer>();
            
            spriteRenderer.sortingOrder = 9;
            return;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.CompareTag("PlayerFeet")) {
            Transform playerPos = collider.GetComponentInParent<Player>().transform;
            SpriteRenderer spriteRenderer = playerPos.GetComponentInChildren<SpriteRenderer>();
            
            spriteRenderer.sortingOrder = 11;
            return;
        }

        if(collider.CompareTag("EnemyFeet")) {
            Transform enemyPos = collider.GetComponentInParent<Enemy>().transform;
            SpriteRenderer spriteRenderer = enemyPos.GetComponentInChildren<SpriteRenderer>();
            
            spriteRenderer.sortingOrder = 11;
            return;
        }
    }
}