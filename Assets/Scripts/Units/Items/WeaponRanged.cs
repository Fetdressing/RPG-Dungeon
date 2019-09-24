using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRanged : WeaponBase
{

    public WeaponProjectile projectilePrefab;

    public float hitRateRange = 3; // How many degrees that the aim can vary.

    public override void Attack(UnitRoot target, UnitRoot attacker)
    {
        int damage = GetDamageRoll();

        WeaponProjectile weaponProjectile = Instantiate(projectilePrefab.gameObject).GetComponent<WeaponProjectile>();
        weaponProjectile.transform.position = this.transform.position;

        weaponProjectile.Set(damage, baseStats.forceValue, hitRateRange, target, attacker);
    }
}
