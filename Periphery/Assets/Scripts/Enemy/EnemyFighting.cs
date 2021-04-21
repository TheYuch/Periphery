using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighting : MonoBehaviour, IDamageable
{
    private const int maxHealth = 3;

    private WeaponBase currentWeapon;
    private int currentHealth;
    private SpriteRenderer rend;
    private Color thisColor;

    public HealthBar healthBar;
    

    private void Awake()
    {
        //currentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
        rend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        thisColor = rend.color;
        //currentWeapon.InitWeapon();
    }

    private void FixedUpdate()
    {
        //currentWeapon.UpdateWeapon();
    }
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
