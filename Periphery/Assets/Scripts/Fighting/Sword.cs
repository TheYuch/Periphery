using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponBase
{
    private const float swordLength = 1f; //todo: set this based on sprite image length
    private Vector2 tipPosition { get { return transform.position + transform.up * swordLength; } }
    private Vector3 localPosition;

    public override void UpdateWeapon(Vector2 joystickDirection, Vector3 playerPos, bool joystickIsPressed)
    {
        if (!joystickIsPressed)
        {
            transform.localPosition = localPosition;
            return;
        }

        Vector2 joystickRelativePos = joystickDirection * WeaponBase.moveScale;
        //rotation
        Vector3 forward = (Vector2)(transform.position - playerPos) - joystickRelativePos;
        //note transform.position is position of hilt of weapon
        if (!forward.Equals(Vector2.zero))
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg + 90);
        }

        //translation (set transform.position so that tip position is equal to joystickrelativepos
        transform.position += (Vector3)(joystickRelativePos - tipPosition) + playerPos;

        localPosition = transform.localPosition;
    }
}
