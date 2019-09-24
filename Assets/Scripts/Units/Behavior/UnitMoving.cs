using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitMoving : UnitRoot
{
    public WeaponBase weapon;

    private float nextAttackReady = 0.0f;

    private List<Target> allPotTargets = new List<Target>();

    private Target currTarget;


    private float baseMoveSpeed = 35f;
    private float maxMoveSpeed = 15f;
    private float moveSpeedExternalForceMult = 1;
    private float timerExternalForceWearOff = 0;

    Vector3 myPos;
    Vector3 targetPos;
    Vector3 targetDir;

    public override void SetVelocityExternal(Vector3 velocity)
    {
        base.SetVelocityExternal(velocity);

        const float maxVelReduce = 65;
        float reduceForce = System.Math.Min(velocity.magnitude / maxVelReduce, 1);

        timerExternalForceWearOff = Time.time + (reduceForce * 0.5f);

        moveSpeedExternalForceMult = moveSpeedExternalForceMult - reduceForce;
        moveSpeedExternalForceMult = System.Math.Max(moveSpeedExternalForceMult, 0);
    }

    public override void AddVelocityExternal(Vector3 velocity)
    {
        base.AddVelocityExternal(velocity);

        const float maxVelReduce = 65;
        float reduceForce = System.Math.Min(velocity.magnitude / maxVelReduce, 1);

        timerExternalForceWearOff = Time.time + (reduceForce * 0.5f);

        moveSpeedExternalForceMult = moveSpeedExternalForceMult - reduceForce;
        moveSpeedExternalForceMult = System.Math.Max(moveSpeedExternalForceMult, 0);
    }

    private void Attack()
    {
        weapon.Attack(currTarget.UnitBase, this);
    }

    protected override void OnInit()
    {
        base.OnInit();

        FetchAllPotentialTargets();
    }

    private void Update()
    {
        if (health.alive == false)
        {
            return;
        }

        float storedYVel = rBody.velocity.y; // Store Y-velocity to not change it in the update.
        rBody.velocity = new Vector3(this.rBody.velocity.x, 0, this.rBody.velocity.z);

        // Always lerp back the external mult. If we were hit by external force then we want to bring controll back to self again.
        moveSpeedExternalForceMult += Time.deltaTime * 0.28f;
        moveSpeedExternalForceMult = System.Math.Min(moveSpeedExternalForceMult, 1);

        if (currTarget != null && currTarget.Alive)
        {
            myPos = new Vector3(transform.position.x, 0, transform.position.z);
            targetPos = new Vector3(currTarget.Position.x, 0, currTarget.Position.z);
            targetDir = (targetPos - myPos).normalized;
            float targetDistance = Vector3.Distance(myPos, targetPos);

            this.transform.forward = targetDir;
            
            if (IsWithinAttackRange(targetDistance))
            {
                if (Time.time > nextAttackReady)
                {
                    nextAttackReady = Time.time + weapon.baseStats.attackspeed;

                    Attack();
                }
            }
            
            if (!IsWithinPreferredAttackRange(targetDistance))
            {
                // Go chase!
                Vector3 newVelocity = this.rBody.velocity + (this.transform.forward * baseMoveSpeed * moveSpeedExternalForceMult * Time.deltaTime);

                if (newVelocity.sqrMagnitude < this.rBody.velocity.sqrMagnitude || newVelocity.magnitude < maxMoveSpeed)
                {
                    this.rBody.velocity = newVelocity;
                }
            }
            else
            {
                if (timerExternalForceWearOff < Time.time)
                {
                    this.rBody.velocity = this.rBody.velocity * 0.75f; // Slow down velocity.
                }
            }
        }
        else
        {
            currTarget = GetBestTarget();
        }

        this.rBody.velocity = new Vector3(this.rBody.velocity.x, storedYVel, this.rBody.velocity.z);
    }

    private bool IsWithinAttackRange(float targetDistance)
    {
        if (targetDistance < weapon.baseStats.attackRange)
        {
            return true;
        }

        return false;
    }

    private bool IsWithinPreferredAttackRange(float targetDistance)
    {
        if (targetDistance < (weapon.baseStats.attackRange * weapon.baseStats.attackRangePreferredNor))
        {
            return true;
        }

        return false;
    }

    #region TARGETS
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

    private void FetchAllPotentialTargets()
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

    private class Target
    {
        private UnitRoot target;
        private UnitRoot owner;

        private float maxDistanceFactor = 100;

        public float threat;

        public Target(UnitRoot target, UnitRoot owner)
        {
            this.target = target;
            this.owner = owner;

        }

        public bool Alive
        {
            get
            {
                return target != null && target.health.alive;
            }
        }

        public Vector3 Position
        {
            get
            {
                return target.transform.position;
            }
        }

        public UnitRoot UnitBase
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
            threat += (maxDistanceFactor - distance) / maxDistanceFactor;
        }
    }
    #endregion
}

