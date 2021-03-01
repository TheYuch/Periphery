using UnityEngine;

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
        if (base.isEnemy)
        {
            //fighting code for AI enemy
        }
        else
        {
            if (base.JoystickIsPressed) //probably not going to use rb.MoveRotation because unnecessary
            {
                Quaternion target = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(base.JoystickDirection.y, base.JoystickDirection.x) - 90);
                chainHandle.rotation = Quaternion.Lerp(chainHandle.rotation, target, Time.fixedDeltaTime * 7);

                float magnitude = base.JoystickDirection.magnitude;
                ballSpring.dampingRatio = 1f - (magnitude * 0.3f);
            }
        }
    }

    public override void StopWeapon()
    {
        //todo: for chainball, have an animation of returning the chains.
    }

    private void OnCollisionEnter2D(Collision2D collision) //TODO: do defensive/offensive weapon collisions
    {
        //check Sword.cs OnCollisionEnter2D code comments for explanation
        foreach (ContactPoint2D pt in collision.contacts)
        {
            if (pt.collider.transform.parent != null)
            {
                if (pt.collider.transform.parent.tag.Equals("ChainBall")) //TODO: Don't use tags. Fix Chainball collision
                    continue;

                if (pt.rigidbody.velocity.magnitude > pt.otherRigidbody.velocity.magnitude)
                {
                    pt.collider.gameObject.GetComponent<WeaponBase>().ParentRB.AddForceAtPosition(-pt.relativeVelocity * 100, pt.point);
                    pt.rigidbody.velocity = Vector2.zero;
                    pt.rigidbody.angularVelocity = 0f;
                }
            }
            else
            {
                pt.rigidbody.AddForceAtPosition(pt.relativeVelocity * 100, pt.point);
            }
        }
    }
}
