using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitChild : MonoBehaviour
{
    [System.NonSerialized]
    public bool alive = true;

    private bool isInit = false;

    public virtual void OnDeath(UnitBase killer)
    {
        alive = false;
    }

    protected virtual void OnInit()
    {

    }

    protected virtual void EditorOnValidate()
    {

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

        OnInit();
    }

    private void OnValidate()
    {
        EditorOnValidate();
    }
}
