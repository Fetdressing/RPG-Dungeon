using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public float shootSpeed = 20;
    public float shootSpeedRange = 1;

    public Collider[] colliders;
    private int damage;
    private float forceValue; // How hard the hit is (weapon stat).
    private UnitRoot attacker;

    [HideInInspector]
    [SerializeField]
    private Rigidbody rbody;

    public void Set(int damage, float forceValue, float hitRateRange, UnitRoot target, UnitRoot attacker)
    {
        this.damage = damage;
        this.attacker = attacker;
        this.forceValue = forceValue;

        Vector3 dir = target.transform.position - attacker.transform.position;
        dir = new Vector3(dir.x, 0, dir.z).normalized;

        float randomHitDir = Random.Range(-hitRateRange, hitRateRange);
        dir = Quaternion.Euler(0, randomHitDir, 0) * dir;

        this.transform.forward = dir;
        this.rbody.velocity = dir * (shootSpeed + Random.Range(0, shootSpeedRange));
        
        // Ignore collision versus caster.
        for (int i = 0; i < attacker.colliders.Length; i++)
        {
            for (int y = 0; y < colliders.Length; y++)
            {
                Physics.IgnoreCollision(attacker.colliders[i], colliders[y]);
            }
        }

        Destroy(this.gameObject, 3);
    }

    public void OnTriggerEnter(Collider other)
    {
        UnitRoot unitRoot = other.GetComponent<UnitRoot>();

        if (unitRoot != null)
        {
            unitRoot.health.Attack(damage, forceValue, attacker);
            Destroy(this.gameObject);
        }
    }

    private void OnValidate()
    {
        this.rbody = this.GetComponent<Rigidbody>();
        this.colliders = this.GetComponentsInChildren<Collider>();
    }
}
