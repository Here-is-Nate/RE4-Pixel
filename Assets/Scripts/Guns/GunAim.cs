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
    private float shakeValueLimiter = 1.2f;
    private float shakeValue;
    private GameObject _aimedGO;
    private bool _isAiming;

    [Header("Crosshair Config")]
    [SerializeField] private GameObject crossHairRender;
    [SerializeField] private Transform crossHairCenter;
    [SerializeField] private GameObject[] crossHairSides;
    [SerializeField] private GameObject crossHairHitPoint;
    private float minCrossHairDistance = 0.1f;

    [Header("Laser Config")]
    [SerializeField] private GameObject laserPoint;
    [SerializeField] private bool shakeLaser;
    private float laserDistance = 10f;
    private float xPosition;
    private float yPosition;
    private float laserShakeDrag = 0.04f;
    private float laserShakeDragCount;
    private float laserShakeInterval = 0.05f;

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

        _aimedGO = null;

        Cursor.visible = true;
    }

    void OnAim() {
        Cursor.visible = false;

        switch(aimType) {
            case EAimType.LASER: LaserAim(); break;
            case EAimType.CROSSHAIR: CrossHairAim(); break;
        }  
    }

    public GameObject GetAimedGameObject() {
        switch(aimType) {
            case EAimType.LASER: return aimedGO;
            case EAimType.CROSSHAIR: return GetCrossHairHitPointGO();
        }

        return aimedGO;
    }

    #region Laser Handler
    void LaserAim() {
        float direction = playerAim.GetAimAngle();

        // Convert a degree angle to radians
        float dirRadians = direction * Mathf.Deg2Rad;

        if(!shakeLaser) {
            xPosition = Mathf.Sin(dirRadians);
            yPosition = Mathf.Cos(dirRadians);
        }

        if(shakeLaser) {
            laserShakeDragCount += Time.deltaTime;
    
            if(laserShakeDragCount >= laserShakeDrag) {
                laserShakeDragCount = 0;

                float crossDistance;

                if(shakeValue > shakeValueLimiter) crossDistance = laserShakeInterval;
                else crossDistance = shakeValue / shakeValueLimiter * laserShakeInterval;

                xPosition = Mathf.Sin(dirRadians) + Random.Range(-crossDistance, crossDistance);
                yPosition = Mathf.Cos(dirRadians) + Random.Range(-crossDistance, crossDistance);
            }
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(xPosition, yPosition), laserDistance, layerMask);

        if(hit) {
            DrawLaser(laserRenderer.transform.position, hit.point);

            laserPoint.SetActive(true);
            laserPoint.transform.position = hit.point;
            
            _aimedGO = hit.transform.gameObject;
        } else {
            DrawLaser(laserRenderer.transform.position, new Vector2(laserRenderer.transform.position.x + xPosition * laserDistance, laserRenderer.transform.position.y + yPosition * laserDistance));

            DeactiveLaserPoint();

            _aimedGO = null;
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

        // Get a Random Point Inside the Crosshair
        float hitPointX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float hitPointY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        hitPointX += Random.Range(-crossDistance, crossDistance);
        hitPointY += Random.Range(-crossDistance, crossDistance);

        crossHairHitPoint.transform.position = new Vector2(hitPointX, hitPointY);
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

    GameObject GetCrossHairHitPointGO() {
        Collider2D collider = Physics2D.OverlapPoint(crossHairHitPoint.transform.position);

        return collider != null ? collider.gameObject : null;
    }
    #endregion
}
