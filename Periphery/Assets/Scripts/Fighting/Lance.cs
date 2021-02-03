using UnityEngine;

public class Lance : WeaponBase
{
    private Vector3 localPosition;
    private float joystickMag = 0f;
    public override void InitWeapon()
    {
        //todo: make weapon point in the same direction as the previous one when weapon switched
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), base.PlayerCollider);
        transform.rotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        localPosition = transform.localPosition;
    }

    public override void UpdateWeapon()
    {
        if (!base.JoystickIsPressed)
        {
            transform.localPosition = localPosition;
            return;
        }
        Quaternion target = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(base.JoystickDirection.y, base.JoystickDirection.x) - 90);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, target, Time.fixedDeltaTime * 7);
      
        joystickMag = base.JoystickDirection.magnitude;       
        transform.position = base.PlayerPos + (transform.up * joystickMag * WeaponBase.moveScale);
        localPosition = transform.localPosition;

    }

    public override void StopWeapon()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), base.PlayerCollider, false); //undo ignore
    }
}
