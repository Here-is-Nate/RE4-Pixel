using UnityEngine;

public class GunSO : ScriptableObject
{
    [Header("Gun Reference")]
    public int gunID;
    public enum EGunType {
        PISTOL,
        SHOTGUN,
    }

    [Header("Stats")]
    
    public float[] firePower;
    public float[] fireRate;
    public float[] reloadSpd;
    public int[] capacity;

    [Header("Stats Level")]
    public int firePowerLevel;
    public int fireRateLevel;
    public int reloadSpdLevel;
    public int capacityLevel;

    [Header("Ammo QTD")]
    public int curAmmo;

    [Header("SFX")]
    public AudioClip shootingSFX;
    public AudioClip reloadingSFX;

    [Header("HUD")]
    public Sprite wpnHUDSprite;
}