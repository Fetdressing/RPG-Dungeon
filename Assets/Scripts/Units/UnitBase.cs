using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody))]
public class UnitBase : UnitRoot
{
    public static List<UnitBase> allActiveUnits = new List<UnitBase>();

    public Collider[] colliders;

    public Health health;
    [SerializeField]
    protected Rigidbody rBody;

    public virtual void SetVelocityExternal(Vector3 velocity)
    {
        this.rBody.velocity = velocity;
    }

    public virtual void AddVelocityExternal(Vector3 velocity)
    {
        this.rBody.velocity += velocity;
    }

    public override void OnDeath(UnitBase killer)
    {
        base.OnDeath(killer);

        allActiveUnits.Remove(this);
    }

    protected override void OnInit()
    {
        base.OnInit();

        health.SetMaxHealth(3000);
    }

    protected override void EditorOnValidate()
    {
        base.EditorOnValidate();

        colliders = this.GetComponentsInChildren<Collider>();
        health = this.GetComponent<Health>();
        rBody = this.GetComponent<Rigidbody>();
    }   
}
