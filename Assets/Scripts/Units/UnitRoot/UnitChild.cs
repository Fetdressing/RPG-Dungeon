using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitChild : MonoBehaviour
{
    [System.NonSerialized]
    public bool alive = true;


    protected UnitRoot unitRoot;
    private bool isInit = false;

    public virtual void OnDeath(UnitRoot killer)
    {
        alive = false;
    }

    protected virtual void OnInit()
    {

    }

    protected virtual void OnStatsChanged(Stats.Secondary currentStats)
    {

    }

    protected virtual void EditorOnValidate()
    {

    }

    public void Init(UnitRoot unitRoot)
    {
        if (isInit)
        {
            return;
        }

        this.unitRoot = unitRoot;
        this.unitRoot.statHandler.RegisterOnStatsChanged(OnStatsChanged);

        isInit = true;

        OnInit();
    }

    private void OnValidate()
    {
        EditorOnValidate();
    }
}
