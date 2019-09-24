using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(UnitStatHandler))]
// Root class for all unit related root objects.
public class UnitRoot : UnitChild
{
    public static List<UnitRoot> allActiveUnits = new List<UnitRoot>();

    public Team team;

    [HideInInspector]
    public Collider[] colliders;

    [HideInInspector]
    public UnitStatHandler statHandler;

    [HideInInspector]
    public Health health;

    public enum Team
    {
        PLAYER,
        ENEMY,
        PASSIVE
    }

    [HideInInspector]
    [SerializeField]
    private UnitChild[] myChildren;

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

    public override void OnDeath(UnitRoot killer)
    {
        base.OnDeath(killer);

        allActiveUnits.Remove(this);
    }

    public void SignalDeath(UnitRoot killer)
    {
        if (!alive)
        {
            return;
        }

        alive = false;

        for (int i = 0; i < myChildren.Length; i++)
        {
            myChildren[i].OnDeath(killer);
        }
    }

    protected override void EditorOnValidate()
    {
        base.EditorOnValidate();

        rBody = this.GetComponent<Rigidbody>();
        colliders = this.GetComponentsInChildren<Collider>();
        health = this.GetComponent<Health>();
        myChildren = this.gameObject.GetComponents<UnitChild>();
        statHandler = this.gameObject.GetComponent<UnitStatHandler>();
    }

    private void Awake()
    {
        statHandler.Init(this); // Initialize first because other scripts might need access to it.

        for (int i = 0; i < myChildren.Length; i++)
        {
            myChildren[i].Init(this);
        }
    }
}
