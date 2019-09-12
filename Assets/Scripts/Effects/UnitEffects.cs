using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffects : UnitChild
{
    private IEnumerator currIE = null;

    [SerializeField]
    private AnimationCurve pokeCurve;

    [SerializeField]
    private AnimationCurve deathCurve;

    [SerializeField]
    private Transform effectObject; // Object that we play transform effects on.

    private Vector3 effectObjectLocalStartPos;

    private Quaternion effectObjectLocalStartRot;

    public void DisplayHit(Vector3 dir, float norForce)
    {
        if (currIE != null || !alive)
        {
            return;
        }

        effectObject.transform.localPosition = effectObjectLocalStartPos;
        effectObject.transform.localRotation = effectObjectLocalStartRot;

        dir = new Vector3(dir.x, 0, dir.z);

        currIE = PokeAwayIE(dir, norForce);
        StartCoroutine(currIE);
    }

    public override void OnDeath(UnitBase killer)
    {
        base.OnDeath(killer);

        StopAllCoroutines();

        // Play death animation.
        StartCoroutine(DeathIE((this.transform.position - killer.transform.position).normalized));
    }

    private IEnumerator DeathIE(Vector3 dir)
    {
        float effectTime = 0.8f;
        float accumTime = 0f;

        Vector3 turnAroundVec = Vector3.Cross(Vector3.up, dir.normalized);

        while (accumTime < effectTime)
        {
            float currVal = deathCurve.Evaluate(accumTime / effectTime);
            effectObject.RotateAround(effectObject.transform.position, turnAroundVec, currVal * 18);
            
            effectObject.transform.localPosition = effectObjectLocalStartPos + (dir * currVal * 7);

            accumTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        currIE = null;
    }

    private IEnumerator PokeAwayIE(Vector3 dir, float norForce)
    {
        const float baseForce = 1.7f;
        float effectTime = 0.8f * norForce;
        float accumTime = 0f;

        Vector3 turnAroundVec = Vector3.Cross(Vector3.up, dir.normalized);

        while (accumTime < effectTime)
        {
            float currVal = pokeCurve.Evaluate(accumTime / effectTime);
            effectObject.RotateAround(effectObject.transform.position, turnAroundVec, currVal * 10);

            //effectObject.transform.localRotation = effectObjectLocalStartRot + 

            effectObject.transform.localPosition = effectObjectLocalStartPos + (dir * currVal * norForce * baseForce);

            accumTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        effectObject.transform.localPosition = effectObjectLocalStartPos;
        effectObject.transform.localRotation = effectObjectLocalStartRot;
        currIE = null;
    }

    protected override void OnInit()
    {
        base.OnInit();

        effectObjectLocalStartPos = effectObject.localPosition;
        effectObjectLocalStartRot = effectObject.localRotation;
    }
}
