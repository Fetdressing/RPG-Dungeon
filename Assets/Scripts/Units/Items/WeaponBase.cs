using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ItemBase
{
    [SerializeField]
    public Stats.Weapon baseStats;

    public virtual void Attack(UnitRoot unit, UnitRoot attacker)
    {
        int damage = GetDamageRoll();
        unit.health.Attack(damage, baseStats.forceValue, attacker);
    }

    protected int GetDamageRoll()
    {
        return System.Math.Max(baseStats.damage + Random.Range(-baseStats.damageRange, baseStats.damageRange), 0);
    }
}
