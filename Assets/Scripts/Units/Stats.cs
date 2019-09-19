using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats : MonoBehaviour
{
    [System.Serializable]
    public struct Primary
    {
        public int strength;

        public int speed;

        public int spirit;
    }

    // Calculates secondary attributes from primary.
    public static Secondary CalculateSecondary(Primary primary)
    {
        Secondary secondary;

        secondary.moveSpeed = (int)(primary.speed * 0.1f);
        secondary.health = (int)(primary.strength * 10f);
        secondary.healthReg = (int)(primary.strength * 0.1f);
        secondary.attackspeedMult = (int)(primary.speed * 0.1f);

        return secondary;
    }

    [System.Serializable]
    public struct Secondary
    {
        public int moveSpeed;

        public int health;

        public int healthReg;

        public float attackspeedMult;
    }

    // Attributes that are not affected by the primary attributes.
    public struct Unaffected
    {
        public float lifesteal;
    }

    [System.Serializable]
    public class Weapon
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
