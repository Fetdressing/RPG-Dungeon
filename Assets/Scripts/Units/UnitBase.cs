using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitUIDisplay))]
public class UnitBase : MonoBehaviour
{
    public static List<UnitBase> allActiveUnits = new List<UnitBase>();

    public UnitUIDisplay unitUIDisplay;

    private bool isInit = false;

    public WeaponBase weapon;

    private float nextAttackReady = 0.0f;

    private List<Target> allPotTargets = new List<Target>();

    private Target currTarget;

    private void Attack()
    {
        int damage = weapon.GetDamageRoll();
        currTarget.UnitBase.unitUIDisplay.DisplayHitText(damage.ToString());
    }

    private void Awake()
    {
        unitUIDisplay = this.GetComponent<UnitUIDisplay>();
        Init();
    }

    private void Init()
    {
        if (isInit)
        {
            return;
        }

        isInit = true;

        FetchTargets();
    }

    private void Update()
    {
        if (!isInit)
        {
            return;
        }

        if (currTarget != null)
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
            allPotTargets[i].CalculateThreat();

            if (allPotTargets[i].threat > biggestThreat)
            {
                biggestThreat = allPotTargets[i].threat;
                bestIndex = i;
            }
        }

        return allPotTargets[bestIndex];
    }

    private void FetchTargets()
    {
        allPotTargets.Clear();

        for (int i = 0; i < allActiveUnits.Count; i++)
        {
            allPotTargets.Add(new Target(allActiveUnits[i], this));
        }
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
