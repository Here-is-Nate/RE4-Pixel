using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [Header("References")]
    private Gun gun;
    private GunAim gunAim;
    private Animator animator;

    [Header("Shoot Variables")]
    private bool canShoot;
    private float canFireCount;

    void Start() {
        gun = GetComponent<Gun>();
        gunAim = GetComponent<GunAim>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update() {
        OnShoot();
    }

    void OnShoot() {
        if(gunAim.isAiming && Input.GetKey(KeyCode.Mouse0)) Shoot();

        if(!canShoot) {
            canFireCount += Time.deltaTime;
            if(canFireCount >= gun.fireRate) {
                canShoot = true;
            }
        }
    }

    void Shoot() {
        if(gun.ammo <= 0) return;

        if(canShoot) {
            canFireCount = 0;
            canShoot = false;
            gun.ammo--;

            animator.SetTrigger("Shoot");

            GameObject hitted = gunAim.GetAimedGameObject();

            if(hitted == null) return;

            if(hitted.CompareTag("Enemy")) {
                hitted.gameObject.GetComponentInParent<Enemy>().GetDamage(gun.firePower);
            }
        }
    }
}
