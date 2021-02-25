using UnityEngine;
using System.Collections.Generic;
public class Sword : WeaponBase
{
    private const float swordLength = 1f; //todo: set this based on sprite image length
    private const float returnSpeed = 0.25f;
    private const float rotSpeed = 10f;
    private Vector2 tipPosition { get { return transform.position + transform.up * swordLength; } }
    private Vector3 localPosition;
    private Quaternion prevRotation;

    public Collider2D defenseCol;
    public Collider2D offenseCol;
    public override void InitWeapon()
    {
        //todo: make weapon point in the same direction as the previous one when weapon switched
        Physics2D.IgnoreCollision(this.defenseCol, base.ParentCollider);
        Physics2D.IgnoreCollision(this.offenseCol, base.ParentCollider);
        transform.localPosition = Vector3.zero;
        localPosition = transform.localPosition;
        transform.rotation = Quaternion.identity;
        prevRotation = transform.rotation;
    }

    public override void UpdateWeapon()
    {

        if (!base.JoystickIsPressed)
        {
            if (base.PlayerIsMoving)
            {
                StartCoroutine(base.returnToOrigin(returnSpeed, rotSpeed));
            }

            return;
        }

        Vector2 joystickRelativePos = base.JoystickDirection * WeaponBase.moveScale;
        //rotation
        Vector3 forward = (Vector2)(transform.position - base.ParentPosition) - joystickRelativePos;
        //note transform.position is position of hilt of weapon
        if (!forward.Equals(Vector2.zero))
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg + 90);
        }

        //translation (set transform.position so that tip position is equal to joystickrelativepos
        transform.position += (Vector3)(joystickRelativePos - tipPosition) + base.ParentPosition;

        localPosition = transform.localPosition;
        //prevRotation = transform.rotation;
    }

    public override void StopWeapon()
    {
        Physics2D.IgnoreCollision(this.defenseCol, base.ParentCollider, false); //undo ignore
        Physics2D.IgnoreCollision(this.offenseCol, base.ParentCollider, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("collided!");
        foreach (ContactPoint2D pt in collision.contacts)
        {
            ExecuteAbilities(base.ReturnCollisionAbilities(pt.otherCollider.gameObject.name));
        }
    }
    private void ExecuteAbilities(List<AbilityExecuter> abilities)
    {
        foreach(AbilityExecuter ability in abilities)
        {
            ability();
        }
    }
}
