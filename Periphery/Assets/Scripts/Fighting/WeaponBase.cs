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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public abstract void UpdateWeapon(Vector2 joystickDirection, Vector3 playerPos, bool joystickIsPressed);
}
