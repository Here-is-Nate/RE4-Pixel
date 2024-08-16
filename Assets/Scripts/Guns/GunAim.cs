using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunAim : MonoBehaviour
{
    [Header("References")]
    private Player player;
    private PlayerAim playerAim;
    private LineRenderer laserRenderer;

    [Header("Laser Variables")]
    private float laserDistance = 5f;

    void Start() {
        player = GetComponentInParent<Player>();
        playerAim = GetComponent<PlayerAim>();

        laserRenderer = GetComponentInChildren<LineRenderer>();
    }

    void Update() {
        if(player.isAiming) OnAim();
        else OnNotAim();
    }

    void OnNotAim() {
        EraseLaser();
    }

    void OnAim() {
        float direction = playerAim.GetAimAngle();

        // Convert a degree angle to radians
        float dirRadians = direction * Mathf.Deg2Rad;

        float xPosition = Mathf.Sin(dirRadians);
        float yPosition = Mathf.Cos(dirRadians);

        DrawLaser(laserRenderer.transform.position, new Vector2(laserRenderer.transform.position.x + xPosition * laserDistance, laserRenderer.transform.position.y + yPosition * laserDistance));
    }

    void DrawLaser(Vector2 startPos, Vector2 endPos) {
        laserRenderer.SetPosition(0, startPos);
        laserRenderer.SetPosition(1, endPos);
    }

    void EraseLaser() {
        DrawLaser(Vector2.zero, Vector2.zero);
    }
}
