using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Gun/Pistol")]
public class PistolSO : GunSO
{
    [Header("Gun Type")]
    public EGunType gunType = EGunType.PISTOL;
}
