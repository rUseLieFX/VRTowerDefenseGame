using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

//Ezt a classt haszn�lja az �SSZES building. Alap dolgokat tartalmaz, pl cost, upgrade elj�r�s.
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
        //Mivel tornyonk�nt v�ltozik hogy az adott fejleszt�sn�l mi t�rt�nik, ezt tornyonk�nt override-olni kell majd. 
        Debug.LogWarning("Az �p�letet fejleszteni k�ne, de nincs megcsin�lva!");
        if (maxLevel())
        {
            Debug.Log("Max fejleszt�sen vagyunk.");
            return;
        }

        if (hasEnoughMoney())
        {
            //Ide j�nnek a torony specifikus dolgok.
        }
    }

    protected bool maxLevel()
    {
        //Felt�telezz�k, hogy mindk�t �gon ugyan annyi fejleszt�s van.
        return upgradeLvl >= upgradeCost.Length;
    }

    public bool MaxLevel
    {
        get { return maxLevel(); }
    }

    /// <summary>
    /// Aut�matikusan levonja a p�nzt, �s n�veli a torony szintj�t.
    /// </summary>
    /// <returns></returns>
    protected bool hasEnoughMoney()
    {
        if (!upgradeType) //Ha az els� �gat fejlesztj�k, ez a r�sz fut le.
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
        Debug.Log("Nincs el�g p�nz fejleszt�sre!");
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
