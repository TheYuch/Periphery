using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    /*
     * Base class for all weapons to inherit. Includes:
     *     - updating sword rotation/movement based on fighting joystick
     *     - unleashing abilities depending on if they are unlocked
     *     - deactivating/activating the weapon (i.e. switching weapons)
     *     - more features when the game is more developed.
     */
    protected const float moveScale = 1f;
    protected Rigidbody2D rb;

    private FightingController fightingController;

    protected bool moveJoystickIsPressed { get { return fightingController.moveJoystick.isPressed; } }

    protected Collider2D ParentCollider { get { return transform.parent.GetComponent<Collider2D>(); } }
    protected Vector3 ParentPosition { get { return transform.parent.position; } }
    protected bool JoystickIsPressed { get { return fightingController.fightingJoystick.isPressed; } }
    protected Vector2 JoystickDirection { get { return fightingController.fightingJoystick.Direction; } }
    //NOTE: JoystickDirection is not normalized, its magnitude is the joystick's magnitude relative to its center

    public Rigidbody2D ParentRB; //accessed by weapon collisions

    private const float ReturnSpeed = 10f;

    public bool isEnemy;

    private void Awake()
    {
        ParentRB = transform.parent.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        fightingController = GameObject.Find("Player").GetComponent<FightingController>();
    }

    public abstract void InitWeapon();

    public abstract void UpdateWeapon();

    public abstract void StopWeapon();

    protected void UpdateReturnToParent()
    {
        if (Vector2.Distance(transform.position, ParentPosition) <= 0.00005f)
        {
            transform.position = this.ParentPosition;
            return;
        }

        transform.position = Vector3.Lerp(this.transform.position, this.ParentPosition, ReturnSpeed * Time.deltaTime);
    }
}
