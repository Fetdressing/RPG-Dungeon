using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffects : UnitChild
{
    private IEnumerator animationIE = null;
    private IEnumerator materialIE = null;

    [Header("Animation")]

    [SerializeField]
    private AnimationCurve pokeCurve;

    [SerializeField]
    private AnimationCurve deathCurve;

    [SerializeField]
    private Transform effectObject; // Object that we play transform effects on.

    private Vector3 effectObjectLocalStartPos;

    private Quaternion effectObjectLocalStartRot;

    [Header("Materials")]

    private static Material hitMaterial;

    [HideInInspector]
    [SerializeField]
    private RenderComp[] renderComps;

    [Header("Particle Systems")]

    [SerializeField]
    private ParticleSystem[] bloodParticleSystemPrefabs;

    private void PlayBloodEffect(float normalizedForce)
    {
        if (bloodParticleSystemPrefabs != null && bloodParticleSystemPrefabs.Length > 0)
        {
            int nearestIndex = Mathf.FloorToInt(normalizedForce * (float)bloodParticleSystemPrefabs.Length);
            ParticleSystem spawnParticleSystem = Instantiate(bloodParticleSystemPrefabs[nearestIndex].gameObject).GetComponent<ParticleSystem>();
            spawnParticleSystem.transform.position = this.transform.position;
            Destroy(spawnParticleSystem.gameObject, 2f);
        }
    }

    public void DisplayHit(Vector3 dir, float norForce)
    {
        if (!alive)
        {
            return;
        }

        // Play blood effect.
        PlayBloodEffect(norForce);

        if (animationIE == null)
        {
            effectObject.transform.localPosition = effectObjectLocalStartPos;
            effectObject.transform.localRotation = effectObjectLocalStartRot;

            dir = new Vector3(dir.x, 0, dir.z);

            animationIE = PokeAwayIE(dir, norForce);
            StartCoroutine(animationIE);
        }

        if (materialIE == null)
        {
            materialIE = PlayHitMat();
            StartCoroutine(materialIE);
        }
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

        animationIE = null;
    }

    private IEnumerator PokeAwayIE(Vector3 dir, float norForce)
    {
        norForce = System.Math.Max(norForce, 0.08f); // Make it have a minimum value.

        const float baseForce = 1.7f;
        float effectTime = 0.8f * norForce;
        float accumTime = 0f;

        Vector3 turnAroundVec = Vector3.Cross(Vector3.up, dir.normalized);

        while (accumTime < effectTime)
        {
            float currVal = pokeCurve.Evaluate(accumTime / effectTime);
            effectObject.RotateAround(effectObject.transform.position, turnAroundVec, currVal * 10);

            effectObject.transform.localPosition = effectObjectLocalStartPos + (dir * currVal * norForce * baseForce);

            accumTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        effectObject.transform.localPosition = effectObjectLocalStartPos;
        effectObject.transform.localRotation = effectObjectLocalStartRot;
        animationIE = null;
    }

    private IEnumerator PlayHitMat()
    {
        for (int i = 0; i < renderComps.Length; i++)
        {
            renderComps[i].SetMaterial(hitMaterial);
        }

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < renderComps.Length; i++)
        {
            renderComps[i].Reset();
        }

        materialIE = null;
    }

    protected override void OnInit()
    {
        base.OnInit();

        if (hitMaterial == null)
        {
            hitMaterial = Resources.Load<Material>("HitEffect/HitMaterial");
        }

        effectObjectLocalStartPos = effectObject.localPosition;
        effectObjectLocalStartRot = effectObject.localRotation;
    }

    protected override void EditorOnValidate()
    {
        base.EditorOnValidate();

        Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
        renderComps = new RenderComp[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            renderComps[i] = new RenderComp();
            renderComps[i].renderer = renderers[i];
            renderComps[i].startMaterial = renderers[i].sharedMaterial;
        }
    }

    private class RenderComp
    {
        public Renderer renderer;
        public Material startMaterial;

        public void SetMaterial(Material material)
        {
            renderer.material = material;
        }

        public void Reset()
        {
            renderer.material = startMaterial;
        }
    }
}
