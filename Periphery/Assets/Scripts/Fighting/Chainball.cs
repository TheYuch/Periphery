using UnityEngine;
using System.Collections.Generic;

public class Chainball : WeaponBase
{
    private Transform chainHandle;
    private SpringJoint2D ballSpring;

    public override void InitWeapon()
    {
        //todo: make weapon point in the same direction as the previous one when weapon switched
        chainHandle = transform.GetChild(0).transform;
        ballSpring = transform.GetChild(transform.childCount - 1).gameObject.GetComponent<SpringJoint2D>();
    }

    public override void UpdateWeapon()
    {
        if (base.JoystickIsPressed)
        {
            Quaternion target = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(base.JoystickDirection.y, base.JoystickDirection.x) - 90);
            chainHandle.rotation = Quaternion.Lerp(chainHandle.rotation, target, Time.fixedDeltaTime * 7);

            float magnitude = base.JoystickDirection.magnitude;
            ballSpring.dampingRatio = 1f - (magnitude * 0.3f);
        }
    }

    public override void StopWeapon()
    {
        //todo: for chainball, have an animation of returning the chains.
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
        foreach (AbilityExecuter ability in abilities)
        {
            ability();
        }
    }
}
