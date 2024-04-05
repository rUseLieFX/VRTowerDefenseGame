using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserBuilding : DPSTurret
{
    [Header("Laser turret stuff")]
    [SerializeField] List<Enemy> targets;
    [SerializeField] int maxTargets;
    [SerializeField] LineRenderer[] lines;

    int startDmg;
    [SerializeField] int dmgIncrease;
    [SerializeField] int maxDmg;
    [SerializeField] int dmgIncTicksNeeded;
    [SerializeField] int dmgIncTicksLeft;

    [Header("Debug")]
    [SerializeField] GameObject laserPrefab;

    private void Start()
    {
        startDmg = damage;
        lines = GetComponentsInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (targets.Count < maxTargets) GetNewTarget();

        if (targets.Count > 0)
        {
            List<Enemy> enemiesToRemove = new List<Enemy>();
            foreach (var item in targets)
            {
                if (item == null)
                {
                    enemiesToRemove.Add(item);
                }
            }


            foreach (var item in targets) 
            {
                if (enemiesToRemove.Contains(item)) continue;
                if (Vector3.Distance(transform.position, item.transform.position) > range)
                {
                    enemiesToRemove.Add(item);
                }
            }

            foreach (var item in enemiesToRemove)
            {
                targets.Remove(item);
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if (targets.Count-1 < i)
                {
                    lines[i].enabled = false;
                    continue;
                }

                lines[i].enabled = true;
                lines[i].SetPosition(1, targets[i].transform.position - transform.position); 
            } 
        }
        else
        {
            foreach (var item in lines)
            {
                item.enabled = false;
            }
        }


        

        if (attackSpeedTimer <= 0 && targets.Count > 0)
        {
            GetNewTarget();

            //Maradtak-e m�g rangeben?
            if (targets.Count > 0) Shoot();
        }
        if (attackSpeedTimer > 0) attackSpeedTimer -= Time.deltaTime;
    }

    protected override void GetNewTarget()
    {
        if (targeting == 6) 
        {
            if (targets.Count > 0) return;
        }

        var osszes_enemy = WaveManager.instance.Enemies;
        var valid_enemy = new List<Enemy>();
        foreach (var item in osszes_enemy)
        {
            
            if (Vector3.Distance(transform.position, item.transform.position) <= range)
            {
                valid_enemy.Add(item);
            }
        }

        if (!upgradeType)
        {
            targets.Clear();

            if (valid_enemy.Count > 0)
            {
                for (int i = 0; i < maxTargets; i++)
                {
                    targets.Add(UseTargeting(valid_enemy.ToArray()));
                    valid_enemy.Remove(targets[i]);
                    if (valid_enemy.Count == 0) break;
                }
            }
        }
        else //Ha ide jutottunk, akkor a single targetes �gon van fejlesztve.
        { 
            Enemy ogTarget = null;
            if (targets.Count != 0) ogTarget = targets[0]; //Jegyezz�k meg az eredeti c�lpontot.

            Enemy enemyToAdd;
            if (targeting == 6) enemyToAdd = UseTargeting(valid_enemy.ToArray(), Targeting.Strong);
            else enemyToAdd = UseTargeting(valid_enemy.ToArray()); //N�zz�k meg, ki lenne az ide�lis c�lpont

            if (ogTarget == enemyToAdd) //Ha ugyan az, akkor csak t�rj�nk vissza - ne legyen resetelve a dmg rampup.
                //Pap�ron ezt k�ne csin�lnia, gyakorlatban meg semennyire nem m�k�dik. 
            {
                return;
            }
            else //Ha az �j c�lpont nem egyezik meg az eddigivel, akkor...
            {
                targets.Remove(ogTarget); //Vegy�k ki a r�gi c�lpontot
                targets.Add(enemyToAdd); //Jegyezz�k fel az �jat
                dmgIncTicksLeft = dmgIncTicksNeeded; //Resetelj�k a dmg rampupot.
                damage = startDmg;
            }
        }
    }

    protected override void Shoot()
    {
        if (upgradeType) 
        {
            dmgIncTicksLeft--;

            if (dmgIncTicksLeft <= 0 && damage < maxDmg)
            { 
                damage += dmgIncrease;
                if (damage > maxDmg) damage = maxDmg; //Ha v�letlen�l rosszul lenne sz�molva, �s t�lmenne a maximumon.
                dmgIncTicksLeft = dmgIncTicksNeeded;
            }
        } 
        attackSpeedTimer = attackSpeed;
        foreach (var item in targets)
        {
            item.Damage(damage, damageType);
        }
    }

    public override void Upgrade(bool upgradeType)
    {
        this.upgradeType = upgradeType;
        if (maxLevel())
        {
            Debug.Log("Max szint!");
            return;
        }

        if (hasEnoughMoney())
        {
            if (!upgradeType) //A t�bbir�nyba l�v� �g... 
            {
                switch (upgradeLvl)
                {
                    case 1:
                        MaxTargets = 3;
                        attackSpeed -= 0.05f;
                        break;
                    case 2:
                        attackSpeed -= 0.10f;
                        MaxTargets = 4;
                        break;
                    case 3:
                        attackSpeed -= 0.05f;
                        MaxTargets = 5;
                        damage += 1;
                        break;
                }
            }
            else
            {
                switch (upgradeLvl) //A single-target �g.
                {
                    case 1:
                        dmgIncrease = 3;
                        dmgIncTicksNeeded = 3;
                        dmgIncTicksLeft = 3;
                        maxDmg = 32;
                        break;
                    case 2:
                        dmgIncrease += 2;
                        maxDmg = 52;
                        range += 1;
                        break;
                    case 3:
                        dmgIncrease += 2;
                        maxDmg = 72;
                        startDmg = 4;
                        attackSpeed -= 0.05f;
                        break;
                }
            }
        }
    }

    int MaxTargets
    {
        set
        {
            List<LineRenderer> newLines = lines.ToList();
            for (int i = 0; i < value-maxTargets; i++)
            {
                newLines.Add(Instantiate(laserPrefab, transform).GetComponent<LineRenderer>());
            }
            lines = newLines.ToArray();
            maxTargets = value;
        }
    }
}
