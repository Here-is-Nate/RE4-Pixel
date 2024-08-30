using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    public static GunHolder instance;

    private bool _haveGun;
    public bool haveGun {get {return _haveGun;}}

    void Awake() {
        instance = this;
    }

    void Update() {
        if(transform.childCount > 0) _haveGun = true;
        else _haveGun = false;
    }
}
