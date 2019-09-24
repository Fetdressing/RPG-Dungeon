using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatHandler : UnitChild
{
    public Stats.ObjectStats startStats;
    private Stats.ObjectStats addedStats; // To be used in future for when being able to reduce stats.


    private Stats.Secondary calculatedStats; // We calculate the secondary because its those that are relevant for when doing actions.

    private List<System.Action<Stats.Secondary>> statsChangedCallback = new List<System.Action<Stats.Secondary>>();

    public void RegisterOnStatsChanged(System.Action<Stats.Secondary> callback)
    {
        callback.Invoke(calculatedStats); // Callback straight away aswell.
        statsChangedCallback.Add(callback);
    }

    protected override void OnInit()
    {
        base.OnInit();

        CalculateCurrentStats();
    }

    private void CalculateCurrentStats()
    {
        calculatedStats = Stats.CalculateSecondary(startStats.primary);
    }
}
