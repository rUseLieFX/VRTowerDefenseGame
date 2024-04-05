using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] float waitTimeBetweenWaves;
    //Ellens�gek - ez az�rt fontos, mert az �sszes torony ez alapj�n n�zi az ellens�geket, nem pedig v�gign�zi az �SSZES gameobjectet. 
    //10 torony eset�ben, 200 gameobjecttel (particle is annak sz�m�t, a k�l�nb�z� wavek is), egy keres�sn�l 2000 keres�st csin�lna.
    //�gy tudja pontosan, hogy mely objectek min�s�lnek ellens�geknek. 
    [SerializeField] Enemy[] enemies;
    public Enemy[] Enemies { get { return enemies; } }


    public static WaveManager instance;
    [Header("Debug")]
    [SerializeField] int Waves;


    [SerializeField] int currentWaveID = -1;
    public int CurrentWaveID {  get { return currentWaveID; } }

    [SerializeField] Wave currentWave;
    public Wave CurrentWave { get { return currentWave; } }



    [SerializeField] float timeCountdown;
    [SerializeField] bool BetweenWaves;

    private void Awake()
    {
        instance = this;

        foreach (Transform tf in transform)
        {
            tf.gameObject.SetActive(false);
        }

    }

    void Start()
    {
        currentWave = transform.GetChild(0).GetComponent<Wave>();
        Waves = transform.childCount;
        timeCountdown = waitTimeBetweenWaves; //Teljen el egy kis id� a j�t�k elej�n az els� wave ind�t�sa el�tt.
    }
    void Update()
    {
        if (BetweenWaves)
        {
            if (timeCountdown > 0) timeCountdown -= Time.deltaTime;
            else
            {
                //Teh�t lej�rt a wavek k�z�tti id�...

                if (currentWaveID+1 > Waves) 
                {
                    Debug.Log("GG! �sszes wave elfogyott.");
                    //TODO: j�v�ben itt k�vetkez� map / men�.
                }
                else
                {
                    currentWaveID++;
                    transform.GetChild(0).gameObject.SetActive(true);
                    currentWave = transform.GetChild(0).GetComponent<Wave>();
                    Debug.Log("K�vetkez� wave bet�ltve.");
                }
                BetweenWaves = false;
            }
        }
    }

    public void WaveFinished()
    {
        BetweenWaves = true;
        timeCountdown = waitTimeBetweenWaves;
    }

    //Ez az�rt fontos elj�r�s, mert az �sszes torony a WaveManager.enemies alapj�n n�zi az ellens�geket, nem pedig v�gign�zi az �sszes gameobjectet, hogy az-e.
    public void AddEnemy(Enemy enemy)
    {
        List<Enemy> list = new List<Enemy>();
        Array.ForEach(enemies,list.Add);
        list.Add(enemy);
        enemies = list.ToArray();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        List<Enemy> list = new List<Enemy>();
        Array.ForEach(enemies, list.Add);
        list.Remove(enemy);
        enemies = list.ToArray();
    }

    


}
