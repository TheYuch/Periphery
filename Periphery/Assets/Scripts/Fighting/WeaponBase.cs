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

    private FightingController fightingController;

    protected bool PlayerIsMoving { get { return transform.parent.GetComponent<PlayerMovement>().playerIsMoving; } }
    protected Collider2D ParentCollider { get { return transform.parent.GetComponent<Collider2D>(); } }
    protected Vector3 ParentPosition { get { return transform.parent.position; } }
    protected bool JoystickIsPressed { get { return fightingController.fightingJoystick.isPressed; } }
    protected Vector2 JoystickDirection { get { return fightingController.fightingJoystick.Direction; } }
    //NOTE: JoystickDirection is not normalized, its magnitude is the joystick's magnitude relative to its center

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fightingController = GameObject.Find("Player").GetComponent<FightingController>();
    }

    public abstract void InitWeapon();

    public abstract void UpdateWeapon();

    public abstract void StopWeapon();

    protected virtual List<AbilityExecuter> ReturnCollisionAbilities(string weaponName)
    {
        List<AbilityExecuter> abilityList = new List<AbilityExecuter>();
        switch (weaponName)
        {
            case "Sword":
                abilityList.Add(SwordAbility);
                break;
            case "Lance":
                abilityList.Add(LanceAbility);
                break;
            case "Chainball":
                abilityList.Add(ChainballAbility);
                break;
        }
        return abilityList;
    }
    protected IEnumerator ReturnToPlayer(float moveReturnSpeed, bool runningFlag)
    {
        runningFlag = true;
        while (transform.position != this.ParentPosition)
        {
            if (Mathf.Abs(transform.position.magnitude - this.ParentPosition.magnitude) <= 0.00005f)
            {
                rb.position = this.ParentPosition;
                runningFlag = false;
                yield break;
            }
            else
            {
                rb.position = Vector3.Lerp(this.rb.position, this.ParentPosition, moveReturnSpeed * Time.deltaTime);
                yield return null;
            }
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector2.up), rotReturnSpeed * Time.fixedDeltaTime);                 
     
        }
    }

    protected delegate void AbilityExecuter();
    protected void SwordAbility()
    {
        print("bruh");     
    }
    protected void LanceAbility()
    {
        print("Frank");
    }
    protected void ChainballAbility()
    {
        print("Monke");
    }
}
