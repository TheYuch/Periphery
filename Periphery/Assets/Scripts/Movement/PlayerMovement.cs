using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    //public Vector3 targetPos; //To reduce Lerp jitter; every weapon wielding entity will need this variable

    private DynamicJoystick moveJoystick;
    private Rigidbody2D rb;
    private float joystickAngleInDegs;

    [SerializeField] private MapManager mapManager;

    private const float defaultMoveSpeed = 5f;
    private const float rotSpeed = 7f;
    private const int angleOffset = 90;

    private Quaternion prevRotation;

    private bool playerIsMoving; //Will be registered as "moving" even when joystick magnitude is 0

    void Awake()
    {
        joystickAngleInDegs = 0f;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        playerIsMoving = (moveJoystick.isPressed) ? true : false;

        joystickAngleInDegs = Mathf.Rad2Deg * Mathf.Atan2(moveJoystick.Direction.y, moveJoystick.Direction.x);
        if(joystickAngleInDegs < 0f)
        {
            joystickAngleInDegs = 180 + (180 + joystickAngleInDegs);
        }
        prevRotation = transform.rotation;

        if (playerIsMoving)
        {
            if(Mathf.Abs(((Vector2)(transform.rotation * Vector3.up)).magnitude - moveJoystick.Direction.magnitude) < 0.005f) {
                Vector3 joystickRotationVector = new Vector3(Mathf.Cos(joystickAngleInDegs), Mathf.Sin(joystickAngleInDegs), 0);
                transform.rotation = Quaternion.Euler(joystickRotationVector);
            }
            Vector3 target = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, joystickAngleInDegs - angleOffset);
            //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, target, rotSpeed * Time.deltaTime);
            transform.eulerAngles = target;
        }
        else
        {
            transform.rotation = prevRotation;
        }
        Vector2 movement = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);
        //targetPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        float moveSpeed = defaultMoveSpeed;
        TileData tmp = mapManager.getTileData(rb.position);
        if (tmp != null) moveSpeed = tmp.walkingSpeed;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
