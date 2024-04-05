using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

//Ezt a classt használja az ÖSSZES building. Alap dolgokat tartalmaz, pl cost, upgrade eljárás.
public class Building : MonoBehaviour
{
    public int cost;
    public int[] upgradeCost;
    public int[] secUpgradeCost;
    [SerializeField] protected bool upgradeType;
    [SerializeField] protected int upgradeLvl;

    public bool UpgradeType { get { return upgradeType; } set {  upgradeType = value; } }
    public int UpgradeLvl { get { return upgradeLvl; } }

    public virtual void Sell()
    {
        Destroy(gameObject);
    }

    public virtual void Upgrade()
    {
        Upgrade(false);
    }

    public virtual void Upgrade(bool upgradeType) 
    {
        //Mivel tornyonként változik hogy az adott fejlesztésnél mi történik, ezt tornyonként override-olni kell majd. 
        Debug.LogWarning("Az épületet fejleszteni kéne, de nincs megcsinálva!");
        if (maxLevel())
        {
            Debug.Log("Max fejlesztésen vagyunk.");
            return;
        }

        if (hasEnoughMoney())
        {
            //Ide jönnek a torony specifikus dolgok.
        }
    }

    protected bool maxLevel()
    {
        //Feltételezzük, hogy mindkét ágon ugyan annyi fejlesztés van.
        return upgradeLvl >= upgradeCost.Length;
    }

    public bool MaxLevel
    {
        get { return maxLevel(); }
    }

    /// <summary>
    /// Autómatikusan levonja a pénzt, és növeli a torony szintjét.
    /// </summary>
    /// <returns></returns>
    protected bool hasEnoughMoney()
    {
        if (!upgradeType) //Ha az elsõ ágat fejlesztjük, ez a rész fut le.
        {
            if (upgradeCost[upgradeLvl] <= BuildManager.instance.Money())
            {
                BuildManager.instance.Money(-upgradeCost[upgradeLvl]);
                upgradeLvl++;
                return true;
            }
        }
        else 
            if (secUpgradeCost[upgradeLvl] <= BuildManager.instance.Money())
            {
                upgradeType = true;
                BuildManager.instance.Money(-secUpgradeCost[upgradeLvl]);
                upgradeLvl++;
                return true;
            }
        Debug.Log("Nincs elég pénz fejlesztésre!");
        return false;
    }

    public bool HasEnoughMoney
    {
        get
        {
            return hasEnoughMoney();
        }
    }
}
