using UnityEngine;
using UnityEngine.Tilemaps;

public class LayerChange : MonoBehaviour
{
    [Header("References")]
    private TilemapRenderer tileRenderer;

    void Start() {
        tileRenderer = GetComponent<TilemapRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Player") || collider.CompareTag("Enemy")) {
            tileRenderer.sortingOrder = 11;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.CompareTag("Player") || collider.CompareTag("Enemy")) {
            tileRenderer.sortingOrder = 1;
        }
    }
}
