using UnityEngine;

public class LayerChange : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("PlayerFeet")) {
            Transform playerPos = collider.GetComponentInParent<Player>().transform;
            
            playerPos.position = new Vector3(playerPos.position.x, playerPos.position.y, 0f);
            return;
        }

        if(collider.CompareTag("EnemyFeet")) {
            Transform enemyPos = collider.GetComponentInParent<Enemy>().transform;
            
            enemyPos.position = new Vector3(enemyPos.position.x, enemyPos.position.y, 0f);
            return;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.CompareTag("PlayerFeet")) {
            Transform playerPos = collider.GetComponentInParent<Player>().transform;
            
            playerPos.position = new Vector3(playerPos.position.x, playerPos.position.y, -2f);
            return;
        }

        if(collider.CompareTag("EnemyFeet")) {
            Transform enemyPos = collider.GetComponentInParent<Enemy>().transform;
            
            enemyPos.position = new Vector3(enemyPos.position.x, enemyPos.position.y, -2f);
            return;
        }
    }
}