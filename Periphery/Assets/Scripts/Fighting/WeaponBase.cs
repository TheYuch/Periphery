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
    protected Collider2D PlayerCollider { get { return fightingController.gameObject.GetComponent<Collider2D>(); } }
    protected Vector3 PlayerPos { get { return fightingController.gameObject.transform.position; } }
    protected bool JoystickIsPressed { get { return fightingController.fightingJoystick.isPressed; } }
    protected Vector2 JoystickDirection { get { return fightingController.fightingJoystick.Direction; } }
    //NOTE: JoystickDirection is not normalized, its magnitude is the joystick's magnitude relative to its center

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fightingController = GameObject.Find("Player").GetComponent<FightingController>();
    }

    public abstract void InitWeapon();

    public abstract void UpdateWeapon();

    public abstract void StopWeapon();
}
