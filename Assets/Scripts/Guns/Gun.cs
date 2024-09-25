using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunSO gunSO;

    [Header("HUD References")]
    [SerializeField] private Image wpnImage;
    [SerializeField] private Text wpnAmmoQtd;

    [Header("Stats")]
    private float _firePower;
    private float _fireRate;
    private float _reloadSpd;
    private int _capacity;

    [Header("Ammo")]
    private int curAmmo;

    [Header("SFX")]
    private AudioSource _gunSFX;
    private AudioClip _shootingSFX;
    private AudioClip _reloadingSFX;

    #region Properties
    public float firePower {get { return _firePower; } set { _firePower = value; } }
    public float fireRate {get { return _fireRate;} set { _fireRate = value; } }
    public float reloadSpd {get { return _reloadSpd;} set { _reloadSpd = value;}}
    public int capacity {get { return _capacity;} set { _capacity = value; } }
    public int ammo {get {return curAmmo; } set {curAmmo = value;}}
    public AudioSource gunSFX {get { return _gunSFX;}}
    public AudioClip shootingSFX {get { return _shootingSFX;} set { _shootingSFX = value;}}
    public AudioClip reloadingSFX {get { return _reloadingSFX;} set {_reloadingSFX = value;}}
    #endregion

    void Awake() {
        SetGunStats();
    }
 
    void SetGunStats() {
        _gunSFX = GetComponentInChildren<AudioSource>();

        firePower = gunSO.firePower[gunSO.firePowerLevel];
        fireRate = gunSO.fireRate[gunSO.fireRateLevel];
        reloadSpd = gunSO.reloadSpd[gunSO.reloadSpdLevel];
        capacity = gunSO.capacity[gunSO.capacityLevel];
        shootingSFX = gunSO.shootingSFX;
        reloadingSFX = gunSO.reloadingSFX;

        curAmmo = gunSO.curAmmo;

        // HUD
        wpnImage.sprite = gunSO.wpnHUDSprite;
        wpnAmmoQtd.text = curAmmo.ToString();
    }

    #region Ammo Handler
    public void DecreaseAmmo() {
        ammo--;
        SetAmmoHUD();
        // gunSO.curAmmo = ammo;
    }

    public void DecreaseAmmo(int qtd) {
        ammo -= qtd;
        SetAmmoHUD();
    }

    public void IncreaseAmmo() {
        ammo++;
        SetAmmoHUD();
    }

    public void IncreaseAmmo(int qtd) {
        ammo += qtd;
        SetAmmoHUD();
    }

    private void SetAmmoHUD() {
        wpnAmmoQtd.text = curAmmo.ToString();
    }
    #endregion
}
