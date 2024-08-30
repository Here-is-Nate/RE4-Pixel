using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunSO gunSO;

    [Header("Stats")]
    private float _firePower;
    private float _fireRate;
    private float _reloadSpd;
    private int _capacity;

    [Header("Ammo")]
    private int curAmmo;

    #region Properties
    public float firePower {get { return _firePower; } set { _firePower = value; } }
    public float fireRate {get { return _fireRate;} set { _fireRate = value; } }
    public float reloadSpd {get { return _reloadSpd;} set { _reloadSpd = value;}}
    public int capacity {get { return _capacity;} set { _capacity = value; } }
    public int ammo {get {return curAmmo; } set {curAmmo = value;}}
    #endregion

    void Start() {
        SetGunStats();
    }

    void SetGunStats() {
        firePower = gunSO.firePower[gunSO.firePowerLevel];
        fireRate = gunSO.fireRate[gunSO.fireRateLevel];
        reloadSpd = gunSO.reloadSpd[gunSO.reloadSpdLevel];
        capacity = gunSO.capacity[gunSO.capacityLevel];

        curAmmo = gunSO.curAmmo;
    }
}
