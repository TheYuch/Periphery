using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighting : MonoBehaviour
{
    private WeaponBase currentWeapon;

    private void Awake()
    {
        currentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
    }

    private void Start()
    {
        currentWeapon.InitWeapon();
    }

    private void FixedUpdate()
    {
        currentWeapon.UpdateWeapon();
    }
}
