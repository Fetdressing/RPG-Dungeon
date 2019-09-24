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

        UnitRoot[] unitBases = FindObjectsOfType<UnitRoot>();
        List<UnitRoot> unitRootList = new List<UnitRoot>();

        for (int i = 0; i < unitBases.Length; i++)
        {
            unitRootList.Add(unitBases[i]);
        }

        UnitRoot.allActiveUnits = unitRootList;
    }
}
