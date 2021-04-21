using UnityEngine;
using System.Collections;
public class FightingController : MonoBehaviour, IDamageable 
{
    [SerializeField] public Joystick fightingJoystick;
    [SerializeField] public Joystick moveJoystick;

    //TODO: (Yuchen) only allow weapon switching (with button) once you leave combat with A.I. (for 5 secs)
    //this means you're not getting hit by any A.I. AND your weapon is not touching A.I.
    private GameObject[] allWeapons;
    private WeaponBase currentWeapon;
    private int curWeaponIdx;

    private Color thisColor;
    private SpriteRenderer rend;

    private int maxHealth = 5;
    private int currentHealth;

    public HealthBar healthBar;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        thisColor = rend.color;
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
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        SwitchWeapon(0);
    }

    private void FixedUpdate()
    {
        currentWeapon.UpdateWeapon();
    }

    //StartCoroutine(FlashDamage(0.1f, Color.red, thisColor));

    void IDamageable.takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator IDamageable.damageFlash(float speed, Color damageColor, params Color[] returnColor)
    {
        if (returnColor.Length == 0)
        { 
            rend.color = damageColor;
            while (rend.color != thisColor)
            {
                rend.color = Color.Lerp(rend.color, thisColor, Mathf.PingPong(Time.time, speed));
                yield return null;
            }
        } 
        else
        {
            rend.color = damageColor;
            while (rend.color != returnColor[0])
            {
                rend.color = Color.Lerp(rend.color, returnColor[0], Mathf.PingPong(Time.time, speed));
                yield return null;
            }
        }        
    }
}
