using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats : MonoBehaviour
{
    [System.Serializable]
    public class Main
    {
        public int strength;

        public int speed;

        public int spirit;
    }

    [System.Serializable]
    public class Secondary
    {
        public int moveSpeed;

        public int health;

        public int healthReg;
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

    [System.Serializable]
    public class Extra
    {
        public float attackspeedMult;

        public float lifesteal;


    }
}
