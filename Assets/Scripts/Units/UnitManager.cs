using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private static UnitManager instance;

    public static UnitManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        UnitBase[] unitBases = FindObjectsOfType<UnitBase>();
        List<UnitBase> unitBaseList = new List<UnitBase>();

        for (int i = 0; i < unitBases.Length; i++)
        {
            unitBaseList.Add(unitBases[i]);
        }

        UnitBase.allActiveUnits = unitBaseList;
    }
}
