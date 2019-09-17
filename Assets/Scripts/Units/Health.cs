using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitUIDisplay))]
[RequireComponent(typeof(UnitEffects))]
public class Health : UnitChild
{
    [HideInInspector]
    public UnitBase unitBase;

    [HideInInspector]
    public UnitUIDisplay unitUIDisplay;

    [HideInInspector]
    public UnitEffects unitEffects;

    private int maxHealth;

    private int currHealth;

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currHealth = maxHealth;
    }

    public void Attack(int damage, float forceValue, UnitBase attacker)
    {
        // Calculate whether it missed or not.

        const int minDamageFontSize = 10;
        const int maxDamageFontSize = 25;
        int minMaxFontDiff = maxDamageFontSize - minDamageFontSize;

        const int deathFontSize = 8;
        float damageSize = System.Math.Max(((float)damage / maxHealth), 0.08f); // How big the damage was for this unit.

        int fontSize = minDamageFontSize + ((int)(damageSize * minMaxFontDiff));

        currHealth = System.Math.Max(0, currHealth - damage);

        unitUIDisplay.SetHealth((float)currHealth / maxHealth);

        if (currHealth <= 0)
        {
            unitUIDisplay.DisplayHitText("<color=red><b>DEAD</b></color>", deathFontSize);
            Die(attacker);
        }
        else
        {
            Vector3 attackDir = (this.transform.position - attacker.transform.position).normalized;

            unitUIDisplay.DisplayHitText(damage.ToString(), fontSize);

            if (damage > 0)
            {
                unitEffects.DisplayHit(attackDir, damageSize);

                // Apply velocity depending on weapon force value and the damage size.
                unitBase.AddVelocityExternal((attackDir * damageSize * 350) * forceValue);
                unitBase.AddVelocityExternal(Vector3.up * damageSize * 110 * forceValue);
            }
        }
    }

    private void Die(UnitBase killer)
    {
        unitBase.SignalDeath(killer);
        Destroy(this.gameObject, 2f);
    }

    protected override void EditorOnValidate()
    {
        base.EditorOnValidate();

        unitBase = this.GetComponent<UnitBase>();
        unitUIDisplay = this.GetComponent<UnitUIDisplay>();
        unitEffects = this.GetComponent<UnitEffects>();
    }
}
