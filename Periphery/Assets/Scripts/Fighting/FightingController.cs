using UnityEngine;

public class FightingController : MonoBehaviour
{
    [SerializeField] public Joystick fightingJoystick;

    //TODO: (Yuchen) only allow weapon switching (with button) once you leave combat with A.I. (for 5 secs)
    //this means you're not getting hit by any A.I. AND your weapon is not touching A.I.
    private GameObject[] allWeapons;
    private WeaponBase currentWeapon;
    private int curWeaponIdx;

    private void Awake()
    {
        allWeapons = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            allWeapons[i] = child.gameObject;
            i++;
        }
    }

    private void SwitchWeapon(int weaponIdx) //idx for allWeapons array
    {
        if (currentWeapon != null)
        {
            currentWeapon.StopWeapon();
            allWeapons[curWeaponIdx].SetActive(false);
        }

        allWeapons[weaponIdx].SetActive(true);
        curWeaponIdx = weaponIdx;
        currentWeapon = allWeapons[weaponIdx].GetComponent<WeaponBase>();
        currentWeapon.InitWeapon();
    }

    private void Start()
    {
        SwitchWeapon(0);
    }

    private void FixedUpdate()
    {
        currentWeapon.UpdateWeapon();
    }
}
