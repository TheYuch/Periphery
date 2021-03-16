using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    //public Vector3 targetPos; //To reduce Lerp jitter; every weapon wielding entity will need this variable

    private DynamicJoystick moveJoystick;

    private Rigidbody2D rb;
    private const float moveSpeed = 5f;
    private const float rotSpeed = 7f;
    private const int angleOffset = 90;

    private Quaternion prevRotation;

    private bool playerIsMoving; //Will be registered as "moving" even when joystick magnitude is 0
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        playerIsMoving = (moveJoystick.isPressed) ? true : false;

        float joystickAngleInDegs = Mathf.Rad2Deg * Mathf.Atan2(moveJoystick.Direction.y, moveJoystick.Direction.x);

        prevRotation = transform.rotation;

        if (playerIsMoving)
        {
            if(Mathf.Abs(((Vector2)(transform.rotation * Vector3.up)).magnitude - moveJoystick.Direction.magnitude) < 0.005f) {
                Vector3 joystickRotationVector = new Vector3(Mathf.Cos(joystickAngleInDegs), Mathf.Sin(joystickAngleInDegs), 0);
                transform.rotation = Quaternion.Euler(joystickRotationVector);
            }
            Quaternion target = Quaternion.Euler(0, 0, joystickAngleInDegs - angleOffset);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, rotSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.rotation = prevRotation;
        }
        Vector2 movement = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);
        //targetPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
