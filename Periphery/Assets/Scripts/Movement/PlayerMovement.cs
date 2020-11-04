using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private DynamicJoystick moveJoystick;

    private Rigidbody2D rb;
    private const float moveSpeed = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
