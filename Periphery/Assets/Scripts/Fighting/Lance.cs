using UnityEngine;
using System.Collections.Generic;

public class Lance : WeaponBase
{
    private Vector3 localPosition;
    private Quaternion prevRotation;
    private float joystickMag = 0f;
    private bool isReturning;

    private const float rotSpeed = 10f;
    private const float returnSpeed = 10f;
    private const int angleOffset = 90;
    private const float forceScale = 10f;

    public Collider2D defenseCol;
    public Collider2D offenseCol;
    public override void InitWeapon()
    {
        //todo: make weapon point in the same direction as the previous one when weapon switched
        Physics2D.IgnoreCollision(this.defenseCol, base.ParentCollider);
        Physics2D.IgnoreCollision(this.offenseCol, base.ParentCollider);
        transform.rotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        localPosition = transform.localPosition;
        prevRotation = transform.rotation;
        isReturning = false;
    }

    public override void UpdateWeapon()
    {
        if (!base.JoystickIsPressed)
        {
            if(base.PlayerIsMoving)
            { 
                StartCoroutine(base.ReturnToPlayer(returnSpeed, isReturning));
            }       
            transform.rotation = prevRotation;
            return;
        }

        Quaternion target = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(base.JoystickDirection.y, base.JoystickDirection.x) - angleOffset);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, target, Time.fixedDeltaTime * rotSpeed);
      
        joystickMag = base.JoystickDirection.magnitude;       
        transform.position = base.ParentPosition + (transform.up * joystickMag * WeaponBase.moveScale);
        localPosition = transform.localPosition;
        prevRotation = transform.rotation;
    }

    public override void StopWeapon()
    {
        Physics2D.IgnoreCollision(this.defenseCol, base.ParentCollider, false); //undo ignore
        Physics2D.IgnoreCollision(this.offenseCol, base.ParentCollider, false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D pt in collision.contacts)
        {
            pt.otherRigidbody.AddForceAtPosition(pt.relativeVelocity / forceScale, pt.point);
            ExecuteAbilities(base.ReturnCollisionAbilities(pt.otherCollider.gameObject.name));
        }
    }
    private void ExecuteAbilities(List<AbilityExecuter> abilities)
    {
        foreach (AbilityExecuter ability in abilities)
        {
            ability();
        }
    }
}
