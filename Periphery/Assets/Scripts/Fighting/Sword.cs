using UnityEngine;

public class Sword : WeaponBase
{
    private const float swordLength = 1f; //todo: set this based on sprite image length
    private Vector2 tipPosition { get { return transform.position + transform.up * swordLength; } }
    private Vector3 localPosition;

    public override void InitWeapon()
    {
        //todo: make weapon point in the same direction as the previous one when weapon switched
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), base.PlayerCollider);
        transform.localPosition = Vector3.zero;
        localPosition = transform.localPosition;
        transform.rotation = Quaternion.identity;
    }

    public override void UpdateWeapon()
    {
        if (!base.JoystickIsPressed)
        {
            transform.localPosition = localPosition;
            return;
        }

        Vector2 joystickRelativePos = base.JoystickDirection * WeaponBase.moveScale;
        //rotation
        Vector3 forward = (Vector2)(transform.position - base.PlayerPos) - joystickRelativePos;
        //note transform.position is position of hilt of weapon
        if (!forward.Equals(Vector2.zero))
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg + 90);
        }

        //translation (set transform.position so that tip position is equal to joystickrelativepos
        transform.position += (Vector3)(joystickRelativePos - tipPosition) + base.PlayerPos;

        localPosition = transform.localPosition;
    }

    public override void StopWeapon()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), base.PlayerCollider, false); //undo ignore
    }
}
