using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private Image lifeHUD;

    private float maxPlayerLife = 10;
    private float playerLife;

    void Start() {
        lifeHUD.fillAmount = 1;

        playerLife = maxPlayerLife;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.T)) {
            OnGetDamage();
        }
    }

    void OnGetDamage() {
        playerLife--;

        lifeHUD.fillAmount = playerLife / maxPlayerLife;
    }
}
