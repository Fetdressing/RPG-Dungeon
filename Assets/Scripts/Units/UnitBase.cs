using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class UnitBase : MonoBehaviour
{
    public static List<UnitBase> allActiveUnits = new List<UnitBase>();



    private bool isInit = false;

    public Health health;

    public WeaponBase weapon;

    private float nextAttackReady = 0.0f;

    private List<Target> allPotTargets = new List<Target>();

    private Target currTarget;



    public void Die()
    {
        allActiveUnits.Remove(this);
    }

    private void Attack()
    {
        int damage = weapon.GetDamageRoll();
        currTarget.UnitBase.health.Attack(damage);
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (isInit)
        {
            return;
        }

        isInit = true;

        health.SetMaxHealth(100);

        FetchTargets();
    }

    private void Update()
    {
        if (!isInit)
        {
            return;
        }

        if (currTarget != null && currTarget.Alive)
        {
            if (Time.time > nextAttackReady)
            {
                Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
                Vector2 targetPos = new Vector2(currTarget.Position.x, currTarget.Position.z);

                if (Vector2.Distance(myPos, targetPos) < weapon.baseStats.attackRange)
                {
                    nextAttackReady = Time.time + weapon.baseStats.attackspeed;

                    Attack();
                }
            }
        }
        else
        {
            currTarget = GetBestTarget();
        }
    }

    private Target GetBestTarget()
    {
        float biggestThreat = float.MinValue;
        int bestIndex = -1;

        for (int i = 0; i < allPotTargets.Count; i++)
        {
            if (allPotTargets[i] == null || allPotTargets[i].Alive == false)
            {
                continue;
            }

            allPotTargets[i].CalculateThreat();

            if (allPotTargets[i].threat > biggestThreat)
            {
                biggestThreat = allPotTargets[i].threat;
                bestIndex = i;
            }
        }

        if (bestIndex < 0)
        {
            return null;
        }

        return allPotTargets[bestIndex];
    }

    private void FetchTargets()
    {
        allPotTargets.Clear();

        for (int i = 0; i < allActiveUnits.Count; i++)
        {
            // Make sure we are not adding ourselves to targets.
            if (allActiveUnits[i] == this)
            {
                continue;
            }

            allPotTargets.Add(new Target(allActiveUnits[i], this));
        }
    }

    private void OnValidate()
    {
        health = this.GetComponent<Health>();
    }

    private class Target
    {
        private UnitBase target;
        private UnitBase owner;

        private float maxDistanceFactor = 100;

        public float threat;

        public Target(UnitBase target, UnitBase owner)
        {
            this.target = target;
            this.owner = owner;

        }

        public bool Alive
        {
            get
            {
                return target != null;
            }
        }

        public Vector3 Position
        {
            get
            {
                return target.transform.position;
            }
        }

        public UnitBase UnitBase
        {
            get
            {
                return target;
            }
        }

        public void CalculateThreat()
        {
            Vector2 myPos = new Vector2(owner.transform.position.x, owner.transform.position.z);
            Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.z);

            float distance = Vector2.Distance(myPos, targetPos);

            threat = 0;
            threat += distance / maxDistanceFactor;
        }
    }
}
