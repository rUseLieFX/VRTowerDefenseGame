using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Building_RocketLauncher : DPSTurret
{
    [SerializeField] Transform gunPoint;
    [SerializeField] GameObject rocket;
    [SerializeField] float rocketRadius;
    [SerializeField] int smallRocketsShot;
    [SerializeField] int smallRocketDmg;
    [SerializeField] float smallRocketRadius;
    [SerializeField] float smallRocketSpread;

    [SerializeField] Vector3 targetPos;
    void Update()
    {
        if (target == null) //Ha nincsen célpont..
        {
            GetNewTarget();
        } else
        { 
            if (attackSpeedTimer <= 0)
            {
                GetNewTarget();
                if (target != null)
                {
                    targetPos = target.transform.position;
                    Shoot();
                }
            }
        }


        if (attackSpeedTimer > 0)
        {
            attackSpeedTimer -= Time.deltaTime;
        }
    }

    protected override void Shoot()
    {
        attackSpeedTimer = attackSpeed;
        Rocket spawnedRocket = Instantiate(rocket, gunPoint.position, Quaternion.LookRotation(Vector3.up)).GetComponent<Rocket>();
        spawnedRocket.target = target;
        spawnedRocket.targetPosition = targetPos;
        spawnedRocket.dmg = damage;
        spawnedRocket.damageType = damageType;
        spawnedRocket.radius = rocketRadius;

        if (upgradeType) //Ha a cluster ágon megyünk
        {
            for (int i = 0; i < smallRocketsShot; i++)
            {
                spawnedRocket = Instantiate(rocket, gunPoint.position, Quaternion.LookRotation(Vector3.up)).GetComponent<Rocket>();
                Vector3 random = new Vector3(Random.Range(-smallRocketSpread, smallRocketSpread), 0, Random.Range(-smallRocketSpread, smallRocketSpread));
                spawnedRocket.targetPosition = targetPos + random;
                spawnedRocket.turnSpeed = 3.5f;
                spawnedRocket.dmg = smallRocketDmg;
                spawnedRocket.radius = smallRocketRadius;
                spawnedRocket.damageType = damageType;
                spawnedRocket.transform.localScale *= 0.3f;
            }
        }

    }

    public override void Upgrade(bool upgradeType)
    {
        if (maxLevel())
        {
            Debug.Log("Max fejlesztésen vagyunk.");
            return;
        }

        if (hasEnoughMoney())
        {
            if (!upgradeType) //Egylövetû erõs
            {
                switch (upgradeLvl)
                {
                    case 1:
                    case 2:
                        attackSpeed -= 0.5f;
                        range += 2;
                        damage += 5;
                        break;
                    case 3:
                        attackSpeed -= 1f;
                        damage += 15;
                        break;
                    case 4:
                        range += 2;
                        attackSpeed -= 0.3f;
                        break;
                }
            }
            else //Cluster
            {
                switch (upgradeLvl)
                {
                    case 1:
                        rocketRadius = 1.6f;
                        smallRocketsShot = 3;
                        smallRocketRadius = 0.95f;
                        smallRocketDmg = damage / 5;
                        smallRocketSpread = 3.3f;
                        break;
                    case 2:
                        damage += 2;
                        range += 4;
                        smallRocketsShot = 4;
                        smallRocketSpread = 2.8f;
                        break;
                    case 3:
                        rocketRadius = 2.1f;
                        smallRocketDmg = damage / 3;
                        smallRocketsShot += 1;
                        smallRocketRadius = 1.25f;
                        smallRocketSpread = 2.5f;
                        break;
                    case 4:
                        smallRocketsShot += 3;
                        smallRocketSpread = 2.2f;
                        break;


                }
            }
        }
    }
}
