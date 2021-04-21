using UnityEngine;

public class Lance : WeaponBase
{
    private Quaternion prevRotation;

    private const float rotSpeed = 10f;
    private const int angleOffset = 90;

    public GameObject razeAbility;
    public Collider2D defenseCol;
    public Collider2D offenseCol;
    public override void InitWeapon()
    {
        //todo: make weapon point in the same direction as the previous one when weapon switched
        Physics2D.IgnoreCollision(this.defenseCol, base.ParentCollider);
        Physics2D.IgnoreCollision(this.offenseCol, base.ParentCollider);
        transform.rotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        prevRotation = transform.rotation;
        //InitRaze();
    }

    public override void UpdateWeapon()
    {
        if (!base.JoystickIsPressed)
        {
            base.UpdateReturnToParent();

            transform.rotation = prevRotation;
            return;
        }

        Quaternion target = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(base.JoystickDirection.y, base.JoystickDirection.x) - angleOffset);
        base.rb.MoveRotation(Quaternion.Lerp(gameObject.transform.rotation, target, Time.fixedDeltaTime * rotSpeed).eulerAngles.z);

        float joystickMag = base.JoystickDirection.magnitude;
        base.rb.MovePosition(base.ParentPosition + (transform.up * joystickMag * WeaponBase.moveScale));
        
        prevRotation = transform.rotation;
    }

    public override void StopWeapon()
    {
        Physics2D.IgnoreCollision(this.defenseCol, base.ParentCollider, false); //undo ignore
        Physics2D.IgnoreCollision(this.offenseCol, base.ParentCollider, false);
    }
    private void OnCollisionEnter2D(Collision2D collision) //TODO: do defensive/offensive weapon collisions
    {
        //check Sword.cs OnCollisionEnter2D code comments for explanation
        foreach (ContactPoint2D pt in collision.contacts)
        {
            if (pt.collider.transform.parent != null)
            {
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

    private void InitRaze()
    {
        GameObject raze = Instantiate(razeAbility, transform.position, Quaternion.identity);
        raze.GetComponent<ImpactIndicator>().parent = this.gameObject;
    }
}
