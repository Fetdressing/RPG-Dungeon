using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats : MonoBehaviour
{
    // Calculates secondary attributes from primary.
    public static Secondary CalculateSecondary(Primary primary)
    {
        Secondary secondary;

        secondary.moveSpeed = (int)(primary.speed * 0.1f);
        secondary.health = (int)(primary.strength * 10f);
        secondary.healthReg = (int)(primary.strength * 0.1f);
        secondary.meleeDamage = (int)(primary.strength * 0.1f);
        secondary.rangedDamage = (int)(primary.speed * 0.1f);
        secondary.attackspeedMult = (int)(primary.speed * 0.1f);

        // Attributes not affected by the primary stats.
        secondary.lifesteal = 0;

        return secondary;
    }

    [System.Serializable]
    public struct Primary
    {
        public int strength;

        public int speed;

        public int spirit;
    }

    [System.Serializable]
    public struct Secondary
    {
        public int moveSpeed;

        public int health;

        public int healthReg;

        public int meleeDamage;

        public int rangedDamage;

        public float attackspeedMult;

        public float lifesteal;
    }

    /// <summary>
    /// Stats used by units and items.
    /// </summary>
    [System.Serializable]
    public struct ObjectStats
    {
        public Stats.Primary primary;
        public Stats.Secondary secondary;
    }

    [System.Serializable]
    public struct Weapon
    {
        public int damage;

        public int damageRange;

        public float attackspeed;

        public float attackRange;
        
        [Range(0, 1)]
        public float attackRangePreferredNor; // Value between 0 and 1.

        [Range(0, 1)]
        public float forceValue; // Value between 0 and 1. How strong the weapon hits.
    }
}
