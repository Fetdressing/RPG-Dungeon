using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitUIDisplay))]
public class Health : MonoBehaviour
{
    [HideInInspector]
    public UnitBase unitBase;

    [HideInInspector]
    public UnitUIDisplay unitUIDisplay;

    private int maxHealth;

    private int currHealth;

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currHealth = maxHealth;
    }

    public void Attack(int damage)
    {
        // Calculate whether it missed or not.

        const int minDamageFontSize = 10;
        const int maxDamageFontSize = 25;
        int minMaxFontDiff = maxDamageFontSize - minDamageFontSize;

        const int deathFontSize = 8;

        int fontSize = minDamageFontSize + ((int)(((float)damage / maxHealth) * minMaxFontDiff));

        currHealth = System.Math.Max(0, currHealth - damage);

        unitUIDisplay.SetHealth((float)currHealth / maxHealth);

        if (currHealth <= 0)
        {
            unitUIDisplay.DisplayHitText("<color=red><b>DEAD</b></color>", deathFontSize);
            Die();
        }
        else
        {
            unitUIDisplay.DisplayHitText(damage.ToString(), fontSize);
        }
    }

    private void Die()
    {
        unitBase.Die();
        Destroy(this.gameObject);
    }

    private void OnValidate()
    {
        unitBase = this.GetComponent<UnitBase>();
        unitUIDisplay = this.GetComponent<UnitUIDisplay>();
    }
}
