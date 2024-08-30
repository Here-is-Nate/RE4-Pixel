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

    /// <summary>
    /// Returns a float angle that matches with the direction that mouse is
    /// </summary>
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

    /// <summary>
    /// Returns a coordinate x,y from player looking at
    /// </summary>
    public Vector2 GetLookAtCoordinate() {
        float lookingAngle = lookAtAngle;

        // Set the coordinates to use by a given angle
        Vector2[] lookCoordinates = new Vector2[] {
            new Vector2(0, 1),    // i:0 | A: 0 - 22.5   | 337.5 - 360
            new Vector2(1, 1),    // i:1 | A: 22.5 - 67.5
            new Vector2(1, 0),    // i:2 | A: 67.5 - 112.5
            new Vector2(1, -1),   // i:3 | A: 112.5 - 157.5
            new Vector2(0, -1),   // i:4 | A: 157.5 - 202.5
            new Vector2(-1, -1),  // i:5 | A: 202.5 - 247.5
            new Vector2(-1, 0),   // i:6 | A: 247.5 - 292.5
            new Vector2(-1, 1)    // i:7 | A: 292.5 - 337.5
        };

        // Get the current index in the array using the angle
        int index = Mathf.FloorToInt((lookingAngle + 22.5f) / 45f) % 8;

        return lookCoordinates[index];
    }

    public float GetLookAtIndex() {
        float lookingAngle = lookAtAngle;

        // Get the current index in the array using the angle
        return Mathf.FloorToInt((lookingAngle + 22.5f) / 45f) % 8;
    }
    
}
