using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    /// <summary>
    /// This function should only be used for OnCollisionExit2D
    /// </summary>
    void takeDamage(int damage);

    IEnumerator damageFlash(float speed, Color damageColor, params Color[] returnColor);
}
