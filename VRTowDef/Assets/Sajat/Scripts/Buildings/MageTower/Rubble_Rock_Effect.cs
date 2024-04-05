using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubble_Rock_Effect : MonoBehaviour
{
    [Tooltip("Despawn, attackspeed, dmg, slow, slowtime")]
    public float[] effectData;
    /*
     *  rubbleEffectDecay,
     *  rubbleEffectTickTime,
     *  rubbleEffectDamage,
     *  rubbleEffectSlow,
     *  rubbleEffectSlowTime
     */
    public bool type;
    [SerializeField] float radius;
    [SerializeField] float attackTimer;
    [SerializeField] float decayTimer;
    [SerializeField] GameObject[] particles;

    void Start()
    {
        decayTimer = effectData[0];
        particles[type ? 1 : 0].SetActive(true);
    }

    void Update()
    {
        decayTimer -= Time.deltaTime;
        if (decayTimer <= 0) { Destroy(gameObject); }

        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        else DamageTick();
    }

    void DamageTick() 
    {
        var close_Enemy = new List<Enemy>();
        foreach (var item in WaveManager.instance.Enemies)
        {
            if (Vector3.Distance(transform.position, item.transform.position) <= radius)
            {
                close_Enemy.Add(item);
            }
        }

        if (close_Enemy.Count == 0) return;

        foreach (var item in close_Enemy)
        {
            item.Damage((int)effectData[2], DamageType.Magic);
            //Ha van lassítás, akkor lassítsuk is az ellenséget.
            if (effectData[3]>0) item.Slow(effectData[3], effectData[4]);
        }

        attackTimer = effectData[1];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
