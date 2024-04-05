using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class Building_Rubble : DPSTurret
{
    [SerializeField] float minRange;
    [SerializeField] Transform gunPoint;

    [Header("Rubble")] 
    [SerializeField] GameObject rockObj;
    [SerializeField] Vector3 targetPos;
    [SerializeField] float rubbleEffectDecay; //Meddig maradjon a h�trahagyott dolog a f�ld�n.
    [SerializeField] float rubbleEffectDamage;
    [SerializeField] float rubbleEffectSlow;
    [SerializeField] float rubbleEffectSlowTime;
    [SerializeField] float rubbleEffectTickTime; //Milyen gyakran tickeljen az effekt - attackspeed.

    private void Update()
    {
        
        if (target == null) //Ha nincsen c�lpont..
        {
            GetNewTarget();
        }
        else
        {
            if (attackSpeedTimer <= 0)
            {
                GetNewTarget();
                if (target != null)
                {
                    targetPos = target.transform.position; // Object reference not set to an instance of an object
                    Shoot();
                }

            }
        }


        if (attackSpeedTimer > 0)
        {
            attackSpeedTimer -= Time.deltaTime;
        }
    }

    protected override void GetNewTarget()
    {
        var osszes_enemy = WaveManager.instance.Enemies;
        var close_Enemy = new List<Enemy>();
        foreach (var item in osszes_enemy)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);
            //Range-n bel�l van-e, de minRange-n k�v�l?
            if (distance <= range && distance > minRange)
            {
                close_Enemy.Add(item);
            }
        }

        target = UseTargeting(close_Enemy.ToArray());
    }

    protected override void Shoot()
    {
        attackSpeedTimer = attackSpeed;
        Rubble_Rock rock = Instantiate(rockObj, gunPoint.position, Quaternion.identity).GetComponent<Rubble_Rock>();

        rock.targetPos = targetPos;
        rock.dmg = damage;
        rock.damageType = damageType;
        if (upgradeLvl > 0)
        {
            rock.rockType = (upgradeType) ? 1 : 0; // M�reg vagy t�z
            float[] effectData =
            {
                rubbleEffectDecay,
                rubbleEffectTickTime,
                rubbleEffectDamage,
                rubbleEffectSlow,
                rubbleEffectSlowTime
            };

            rock.effectData = effectData;
        }
    }

    public override void Upgrade(bool aasd)
    {
        if (maxLevel())
        {
            Debug.Log("Max fejleszt�sen vagyunk.");
            return;
        }

        if (hasEnoughMoney())
        {
            if (!upgradeType) //T�zes �g
            {
                switch (upgradeLvl)
                {
                    case 1:
                        damageType = DamageType.Magic;
                        rubbleEffectDamage = 1;
                        rubbleEffectTickTime = 0.15f;
                        rubbleEffectDecay = 3f;
                        range += 1f;
                        break;
                }
            }
            else //M�rges �g
            {
                switch (upgradeLvl) 
                {
                    case 1:
                        damageType = DamageType.Magic;
                        rubbleEffectSlow = 0.8f;
                        rubbleEffectTickTime = 0.2f;
                        rubbleEffectSlowTime = 0.25f;
                        rubbleEffectDecay = 5f;
                        range += 1f;
                        break;
                }
            }
        }
        else Debug.Log("Nincs el�g p�nz fejleszt�sre!");
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, minRange);
        if (target != null)
        {
            Debug.DrawLine(transform.position, target.transform.position,Color.red);
        }
    }
}

