using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sword : WeaponBase
{
    private const float angleOffset = -90f;
    private const float forceMultiplier = 500f;
    private const float torqueMultipler = 0.15f;

    private Quaternion prevRotation;
    private float prevAngle = 0f;
    private Vector2 prevPos = Vector2.zero;

    private List<ContactPoint2D> collisionsCopy = new List<ContactPoint2D>();
    private Sprite thisSprite;

    public bool canSpin = false;
    public GameObject weaponRing;
    public Collider2D defenseCol;
    public Collider2D offenseCol;

    public override void InitWeapon()
    {
        //TODO: make weapon point in the same direction as the previous one when weapon switched
        Physics2D.IgnoreCollision(this.defenseCol, base.ParentCollider);
        Physics2D.IgnoreCollision(this.offenseCol, base.ParentCollider);
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
        prevRotation = transform.rotation;
        //StartCoroutine(SpawnBladeWind());
    }

    public override void UpdateWeapon()
    {
        if (base.moveJoystickIsPressed)
        {
            base.UpdateReturnToParent();

            transform.rotation = prevRotation;
            prevAngle = 0f;
            prevPos = Vector2.zero;
            return;
        }

        Vector2 pos = base.JoystickDirection;
        base.rb.AddForce((pos - prevPos) * forceMultiplier);
        
        float angle = 5f * Mathf.Rad2Deg * Mathf.Atan2(base.JoystickDirection.y * WeaponBase.moveScale, base.JoystickDirection.x * WeaponBase.moveScale) + angleOffset;
        float torque = Mathf.DeltaAngle(prevAngle, angle) * torqueMultipler;
        base.rb.AddTorque(torque);

        prevAngle = angle;
        prevPos = pos;
        prevRotation = transform.rotation;
    }

    public override void StopWeapon()
    {
        Physics2D.IgnoreCollision(this.defenseCol, base.ParentCollider, false); //undo ignore
        Physics2D.IgnoreCollision(this.offenseCol, base.ParentCollider, false);
    }

    private void OnCollisionEnter2D(Collision2D collision) //TODO: do defensive/offensive weapon collisions
    {
        collisionsCopy.Clear();
        foreach (ContactPoint2D pt in collision.contacts)
        {
            collisionsCopy.Add(pt);
            //note: pt.other... is the ...(component) of this current weapon
            //BUT: because "OnCollisionEnter2D" is called after the collision happened,
            //the velocities have already updated. Thus, according to Newton's laws,
            //if the current weapon moved and collided with say the enemy weaepon,
            //even though the current weapon had velocity magnitude, when this function is called,
            //the enemy weapon would have greater velocity magnitude.
            if (pt.collider.transform.parent != null) //if what is collided is the weapon of an enemy
            {
                //print(pt.collider.gameObject.name + "'s magnitude after collision: " + pt.rigidbody.velocity.magnitude);
                //print(pt.otherCollider.gameObject.name + "'s magnitude after collision: " + pt.otherRigidbody.velocity.magnitude);
                if (pt.rigidbody.velocity.magnitude > pt.otherRigidbody.velocity.magnitude) //note: flipped b/c collision already happened (reason above)
                { 

                    pt.collider.gameObject.GetComponent<WeaponBase>().ParentRB.AddForceAtPosition(-pt.relativeVelocity * 100, pt.point); //-pt.relativeVelocity for same reason as described above;
                    pt.rigidbody.velocity = Vector2.zero;
                    pt.rigidbody.angularVelocity = 0f;
                    pt.rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
            else
            {
                Color tmp = new Color(166, 25, 15);
                StartCoroutine(pt.collider.gameObject.GetComponent<IDamageable>().damageFlash(0.1f, tmp));
                pt.rigidbody.AddForceAtPosition(pt.relativeVelocity * 100, pt.point);
            }
        }
    }

    private void OnCollisionExit2D()
    {
        print(collisionsCopy);
        foreach(ContactPoint2D pt in this.collisionsCopy)
        {
            if(pt.collider.transform.parent == null)
            {
                pt.collider.gameObject.GetComponent<IDamageable>().takeDamage(1);
            }
        }
    }

    private IEnumerator SpawnBladeWind()
    {
        yield return new WaitForSeconds(1f);
        for(int i=0; i<6; i++)
        {
            GameObject weaponInstance = Instantiate(weaponRing) as GameObject;
            SpinBehavior weaponInstanceCode = weaponInstance.GetComponent<SpinBehavior>();
            weaponInstanceCode.parent = this.ParentCollider.gameObject;
            weaponInstanceCode.index = i;
            //print(i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0f);
        canSpin = true;
    }
}