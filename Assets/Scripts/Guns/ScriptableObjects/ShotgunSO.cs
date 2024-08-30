using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Gun/Shotgun")]
public class ShotgunSO : GunSO
{
    [Header("Gun Type")]
    public EGunType gunType = EGunType.SHOTGUN;

}