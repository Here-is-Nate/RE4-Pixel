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

    [Header("Aim Config")]
    [SerializeField] private LayerMask layerMask;
    private enum EAimType {
        LASER,
        CROSSHAIR
    }
    [SerializeField] private EAimType aimType;
    private float maxShakeValue = 1.8f;
    private float shakeValue;
    private GameObject _aimedGO;
    private bool _isAiming;

    [Header("Crosshair Config")]
    [SerializeField] private GameObject crossHairRender;
    [SerializeField] private Transform crossHairCenter;
    [SerializeField] private GameObject[] crossHairSides;
    private float minCrossHairDistance = 0.1f;

    [Header("Laser Config")]
    [SerializeField] private GameObject laserPoint;
    private float laserDistance = 10f;

    #region Properties
    public GameObject aimedGO {get {return _aimedGO;}}
    public bool isAiming {get {return _isAiming;}}
    #endregion

    void Start() {
        player = GetComponentInParent<Player>();
        playerAim = player.GetComponent<PlayerAim>();

        laserRenderer = GetComponentInChildren<LineRenderer>();
    }

    void Update() {
        _isAiming = player.isAiming;

        if(isAiming) OnAim();
        else OnNotAim();

        StopShake();
    }

    void StopShake() {
        if(shakeValue >= 0f) {
            shakeValue -= Time.deltaTime;
            if(shakeValue < 0f) shakeValue = 0f;
        }
    }

    public void ResetShake() {
        shakeValue = maxShakeValue;
    }

    void OnNotAim() {
        EraseLaser();

        DeactiveLaserPoint();
        DeactiveCrossHair();

        Cursor.visible = true;
    }

    void OnAim() {
        float direction = playerAim.GetAimAngle();

        Cursor.visible = false;

        // Convert a degree angle to radians
        float dirRadians = direction * Mathf.Deg2Rad;

        float xPosition = Mathf.Sin(dirRadians);
        float yPosition = Mathf.Cos(dirRadians);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(xPosition, yPosition), laserDistance, layerMask);

        if(hit) _aimedGO = hit.transform.gameObject;
        else _aimedGO = null;

        switch(aimType) {
            case EAimType.LASER: LaserAim(hit, xPosition, yPosition); break;
            case EAimType.CROSSHAIR: CrossHairAim(); break;
        }
        
    }

    public GameObject GetAimedGameObject() {
        return aimedGO;
    }

    #region Laser Handler
    void LaserAim(RaycastHit2D hit, float xPosition, float yPosition) {
        if(hit) {
            DrawLaser(laserRenderer.transform.position, hit.point);

            laserPoint.SetActive(true);
            laserPoint.transform.position = hit.point;
            
        } else {
            DrawLaser(laserRenderer.transform.position, new Vector2(laserRenderer.transform.position.x + xPosition * laserDistance, laserRenderer.transform.position.y + yPosition * laserDistance));

            DeactiveLaserPoint();
        }
    }

    void DrawLaser(Vector2 startPos, Vector2 endPos) {
        laserRenderer.SetPosition(0, startPos);
        laserRenderer.SetPosition(1, endPos);
    }

    void EraseLaser() {
        DrawLaser(Vector2.zero, Vector2.zero);
    }

    void DeactiveLaserPoint() {
        if(laserPoint.activeInHierarchy) {
            laserPoint.transform.position = Vector2.zero;
            laserPoint.SetActive(false);
        }
    }
    #endregion

    #region Crosshair Handler
    void CrossHairAim() {
        crossHairRender.SetActive(true);

        float crossDistance;
        if(shakeValue > 1.2f) crossDistance = 0.3f;
        else crossDistance = shakeValue / 1.2f * 0.3f;

        DrawCrossHair(Camera.main.ScreenToWorldPoint(Input.mousePosition), crossDistance);
    }

    void DrawCrossHair(Vector2 position, float crossDistance) {
        crossHairRender.transform.position = position;

        // Create a distance between cross sides
        crossHairSides[0].transform.position = new Vector2(crossHairRender.transform.position.x, crossHairRender.transform.position.y + crossDistance + minCrossHairDistance);
        crossHairSides[1].transform.position = new Vector2(crossHairRender.transform.position.x + crossDistance + minCrossHairDistance, crossHairRender.transform.position.y);
        crossHairSides[2].transform.position = new Vector2(crossHairRender.transform.position.x, crossHairRender.transform.position.y - (crossDistance + minCrossHairDistance));
        crossHairSides[3].transform.position = new Vector2(crossHairRender.transform.position.x - (crossDistance + minCrossHairDistance), crossHairRender.transform.position.y);

    }

    void DeactiveCrossHair() {
        if(crossHairRender.activeInHierarchy) crossHairRender.SetActive(false);
    }
    #endregion
}
