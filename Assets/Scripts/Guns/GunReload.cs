using UnityEngine;

public class GunReload : MonoBehaviour
{
    [Header("References")]
    private Player player;
    private Gun gun;

    [Header("Reload Stats")]
    private bool _reloading;
    private float reloadTime;
    private float reloadTimeCount;

    #region Properties
    public bool reloading {get {return _reloading;}}
    #endregion

    void Start() {
        player = FindObjectOfType<Player>();
        gun = GetComponent<Gun>();

        reloadTime = gun.reloadSpd;
    }

    void Update() {
        OnReload();

        if(reloading) {
            reloadTimeCount += Time.deltaTime;

            if(reloadTimeCount >= reloadTime) {
                reloadTimeCount = 0;
                _reloading = false;
            }
        }
    }

    void OnReload() {
        if(player.isAiming && Input.GetKeyDown(KeyCode.R)) {
            if(gun.ammo == gun.capacity) return;
            Reload();
        }
    }

    void Reload() {
        _reloading = true;

        gun.gunSFX.PlayOneShot(gun.reloadingSFX);
        gun.IncreaseAmmo(gun.capacity - gun.ammo);
    }
}
