using Unity.Mathematics;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    private Player player;

    [Header("Position Variables")]
    // Store current mouse position when aiming
    private Vector3 mousePos;
    private Vector2 direction;
    private float _lookAtAngle;

    public float lookAtAngle {get { return _lookAtAngle;}}

    void Start() {
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isAiming) OnAiming(); 
    }

    void OnAiming() {
        _lookAtAngle = GetAimAngle();
    }
    public float GetAimAngle() {
        // Get the mouse position in the world
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the direction of mouse positio
        direction = mousePos - transform.position;

        // Create a angle using the Tan of x,y, (180/PI) is used to transform radians to degrees
        float angle = Mathf.Atan2(direction.x, direction.y) * (180 / math.PI);

        // Used to not deny angles
        if (angle < 0) angle += 360;

        return angle;
    }
    
}
