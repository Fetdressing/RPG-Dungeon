using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]
    public Stats.Weapon baseStats;

    public int GetDamageRoll()
    {
        return System.Math.Max(baseStats.damage + Random.Range(-baseStats.damageRange, baseStats.damageRange), 0);
    }
}
