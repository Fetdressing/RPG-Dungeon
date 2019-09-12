using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Root class for all unit related root objects.
public abstract class UnitRoot : UnitChild
{
    public Team team;

    public enum Team
    {
        PLAYER,
        ENEMY,
        PASSIVE
    }

    [HideInInspector]
    [SerializeField]
    private UnitChild[] myChildren;

    public void SignalDeath(UnitBase killer)
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
        myChildren = this.gameObject.GetComponents<UnitChild>();
    }
}
