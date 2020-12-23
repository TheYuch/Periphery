using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightingController : MonoBehaviour
{
    [SerializeField] private Joystick fightingJoystick;

    //TODO: (Yuchen) make a list of all weapons, activate them to switch when more weapons coded
    private WeaponBase currentWeapon;

    private void Awake()
    {
        currentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
    }

    private void FixedUpdate()
    {
        currentWeapon.UpdateWeapon(fightingJoystick.Direction, transform.position, fightingJoystick.isPressed);
    }
}
