using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitUIDisplay))]
[RequireComponent(typeof(UnitEffects))]
public class Health : UnitChild
{
    [HideInInspector]
    public UnitUIDisplay unitUIDisplay;

    [HideInInspector]
    public UnitEffects unitEffects;

    [SerializeField]
    private int maxHealth;

    private int currHealth = -1;

    private int healthReg;
    private float nextHealthRegTick = 0.0f;
    private const float healthRegTickRate = 0.5f;

    public void SetMaxHealth(int maxHealth, bool keepCurrHealth = false)
    {
        if (currHealth < 0)
        {
            keepCurrHealth = false;
        }

        float currHealthNor = (float)currHealth / maxHealth;

        this.maxHealth = maxHealth;

        if (keepCurrHealth)
        {
            float newCurrHealth = (float)this.maxHealth * currHealthNor;
            this.currHealth = (int)newCurrHealth;
        }
        else
        {
            this.currHealth = maxHealth;
        }
    }

    public void Attack(int damage, float forceValue, UnitRoot attacker)
    {
        // Calculate whether it missed or not.

        const int minDamageFontSize = 10;
        const int maxDamageFontSize = 25;
        int minMaxFontDiff = maxDamageFontSize - minDamageFontSize;

        const int deathFontSize = 8;
        float damageSize = (float)damage / maxHealth; // How big the damage was for this unit.

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
                unitRoot.AddVelocityExternal((attackDir * damageSize * 350) * forceValue);
                unitRoot.AddVelocityExternal(Vector3.up * damageSize * 110 * forceValue);
            }
        }
    }

    public void Heal(int heal, UnitRoot healer)
    {
        currHealth = System.Math.Min(maxHealth, currHealth - heal);
    }

    private void Die(UnitRoot killer)
    {
        unitRoot.SignalDeath(killer);
        Destroy(this.gameObject, 2f);
    }

    protected override void EditorOnValidate()
    {
        base.EditorOnValidate();

        unitRoot = this.GetComponent<UnitRoot>();
        unitUIDisplay = this.GetComponent<UnitUIDisplay>();
        unitEffects = this.GetComponent<UnitEffects>();
    }

    protected override void OnInit()
    {
        base.OnInit();

        nextHealthRegTick = Time.time;
    }

    protected override void OnStatsChanged(Stats.Secondary currentStats)
    {
        base.OnStatsChanged(currentStats);

        SetMaxHealth(currentStats.health, true);
        this.healthReg = currentStats.healthReg;
    }

    private void Update()
    {
        if (Time.time > nextHealthRegTick)
        {
            // Add health reg.
            Heal(healthReg, this.unitRoot);
            nextHealthRegTick = Time.time + healthRegTickRate;
        }
    }
}
