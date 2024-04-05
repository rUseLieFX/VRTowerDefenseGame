using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DPSTurret : Building
{
    [Header("Stats")]
    [SerializeField] protected float range;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected DamageType damageType;
    [Tooltip("0 - First   1 - Last\n2 - Closest   3- Farthest\n4- Strongest  5- Weakest")]
    [SerializeField] protected int targeting;

    [Header("Debug")]
    [SerializeField] protected Enemy target;
    [SerializeField] protected float attackSpeedTimer = 0;

    /// <summary>
    /// Keres egy �j ellens�get a torony hat�t�vj�n bel�l.
    /// </summary>
    protected virtual void GetNewTarget()
    {
        var osszes_enemy = WaveManager.instance.Enemies;
        var close_Enemy = new List<Enemy>();
        foreach (var item in osszes_enemy)
        {
            if (Vector3.Distance(transform.position, item.transform.position) <= range)
            {
                close_Enemy.Add(item);
            }
        }

        target = UseTargeting(close_Enemy.ToArray());

    }

    /// <summary>
    /// A targeting alapj�n megmondja a j� c�lpontot.
    /// </summary>
    /// <param name="valid_enemy">Az �sszes c�lozhat� ellens�g t�mbben.</param>
    /// <returns></returns>
    protected Enemy UseTargeting(Enemy[] valid_enemy)
    {
        return UseTargeting(valid_enemy, (Targeting)targeting);
    }

    protected Enemy UseTargeting(Enemy[] valid_enemy, Targeting targeting)
    {
        Enemy enemy = null;


        if (valid_enemy.Length > 1) //Ha t�bb ellens�g van
        {
            int id = 0;
            switch (targeting)
            {
                //Az, hogy kivan legel�l az alapj�n van eld�ntve, hogy kinek mennyit kell m�g s�t�lnia a b�zisig.
                case Targeting.First:
                    float closestDistance = valid_enemy[0].PathLength;
                    for (int i = 1; i < valid_enemy.Length; i++)
                    {
                        float distance = valid_enemy[i].PathLength;
                        if (distance < closestDistance)
                        {
                            id = i;
                            closestDistance = distance;
                        }
                    }
                    break;

                case Targeting.Last:
                    float farthestDistance = valid_enemy[0].PathLength;
                    for (int i = 1; i < valid_enemy.Length; i++)
                    {
                        float distance = valid_enemy[i].PathLength;
                        if (distance > farthestDistance)
                        {
                            id = i;
                            farthestDistance = distance;
                        }
                    }
                    break;

                case Targeting.Closest:
                    closestDistance = Vector3.Distance(transform.position, valid_enemy[0].transform.position);
                    for (int i = 1; i < valid_enemy.Length; i++)
                    {
                        float distance = Vector3.Distance(transform.position, valid_enemy[i].transform.position);
                        if (distance < closestDistance)
                        {
                            id = i;
                            closestDistance = distance;
                        }
                    }
                    break;

                case Targeting.Farthest:
                    closestDistance = Vector3.Distance(transform.position, valid_enemy[0].transform.position);
                    for (int i = 1; i < valid_enemy.Length; i++)
                    {
                        float distance = Vector3.Distance(transform.position, valid_enemy[i].transform.position);
                        if (distance > closestDistance)
                        {
                            id = i;
                            closestDistance = distance;
                        }
                    }
                    break;

                case Targeting.Strong:
                    float highestWeight = valid_enemy[0].Weight;
                    for (int i = 1; i < valid_enemy.Length; i++)
                    {
                        float weight = valid_enemy[i].Weight;
                        if (highestWeight < weight)
                        {
                            id = i;
                            highestWeight = weight;
                        }
                    }
                    break;

                case Targeting.Weak:
                    float lowestWeight = valid_enemy[0].Weight;
                    for (int i = 1; i < valid_enemy.Length; i++)
                    {
                        float weight = valid_enemy[i].Weight;
                        if (lowestWeight > weight)
                        {
                            id = i;
                            lowestWeight = weight;
                        }
                    }
                    break;
            }
            enemy = valid_enemy[id];

        }
        //Ha csak egy ellens�g van, akkor nincs �rtelme a targetinggel gondolkozni.
        else if (valid_enemy.Length == 1)
        {
            enemy = valid_enemy[0];
        }

        return enemy;
    }

    protected virtual void Shoot()
    {
        attackSpeedTimer = attackSpeed;
        target.Damage(damage, damageType);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
        if (target != null)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
        }
    }

    protected enum Targeting
    {
        First = 0,
        Last = 1,
        Closest = 2,
        Farthest = 3,
        Strong = 4,
        Weak = 5
    }
}
