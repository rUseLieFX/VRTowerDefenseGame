using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Building_Thumper : DPSTurret
{
    [SerializeField] GameObject particleEffect;
    [SerializeField] float slowAmount, slowTime;
    // Update is called once per frame
    void Update()
    {
        if (attackSpeedTimer <= 0)
        {
            Shoot();
        }
        else attackSpeedTimer -= Time.deltaTime;
    }

    protected override void Shoot()
    {
        var close_Enemy = new List<Enemy>();
        foreach (var item in WaveManager.instance.Enemies)
        {
            if (Vector3.Distance(transform.position, item.transform.position) <= range)
            {
                close_Enemy.Add(item);
            }
        }

        if (close_Enemy.Count == 0) return;



        foreach (var item in close_Enemy)
        {
            

            item.Damage(damage, damageType);
            GameObject particleObj = Instantiate(particleEffect, item.transform.position, Quaternion.identity);
            if (upgradeType) //Ha a lassító ág van fejlesztve.
            {
                item.Slow(slowAmount, slowTime);
                var particle = particleObj.GetComponent<ParticleSystem>();
                var lopott = particle.main;

                lopott.startColor = Color.cyan;
                
            }
        }
        attackSpeedTimer = attackSpeed;

    }

    public override void Upgrade(bool upType)
    {
        if (maxLevel())
        {
            Debug.Log("Max fejlesztésen vagyunk.");
            return;
        }

        if (hasEnoughMoney())
        {
            if (!upType) //Sebzõ ág
            {
                switch (upgradeLvl)
                {
                    case 1:
                    case 2:
                        damage += 3;
                        attackSpeed -= 0.1f;
                        range += 0.5f;
                        break;

                }
            }
            else //Lassító ág
            {
                switch (upgradeLvl)
                {
                    case 1:
                        slowAmount = 0.80f;
                        slowTime = 0.8f;
                        break;
                    case 2:
                        slowAmount = 0.68f;
                        break;
                }
            }
        }
        else Debug.Log("Nincs elég pénz fejlesztésre!");
    }
}
