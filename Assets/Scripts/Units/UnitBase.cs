using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class UnitBase : UnitRoot
{
    public static List<UnitBase> allActiveUnits = new List<UnitBase>();
    

    public Health health;

    public override void OnDeath(UnitBase killer)
    {
        base.OnDeath(killer);

        allActiveUnits.Remove(this);
    }

    protected override void OnInit()
    {
        base.OnInit();

        health.SetMaxHealth(100);
    }

    protected override void EditorOnValidate()
    {
        base.EditorOnValidate();
        health = this.GetComponent<Health>();
    }   
}
