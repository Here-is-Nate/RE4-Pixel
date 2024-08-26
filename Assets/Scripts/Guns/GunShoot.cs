using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [Header("References")]
    private GunAim gunAim;

    void Start() {
        gunAim = GetComponent<GunAim>();
    }

    void Update() {
        OnShoot();
    }

    void OnShoot() {
        if(gunAim.isAiming && Input.GetKeyDown(KeyCode.Mouse0)) Shoot();
    }

    void Shoot() {
        GameObject hitted = gunAim.GetAimedGameObject();

        if(hitted == null) return;

        if(hitted.CompareTag("Enemy")) {
            hitted.gameObject.GetComponentInParent<Enemy>().GetDamage();
        }
    }
}
