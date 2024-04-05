using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] float waitTimeBetweenWaves;
    //Ellenségek - ez azért fontos, mert az összes torony ez alapján nézi az ellenségeket, nem pedig végignézi az ÖSSZES gameobjectet. 
    //10 torony esetében, 200 gameobjecttel (particle is annak számít, a különbõzõ wavek is), egy keresésnél 2000 keresést csinálna.
    //Így tudja pontosan, hogy mely objectek minõsülnek ellenségeknek. 
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
        timeCountdown = waitTimeBetweenWaves; //Teljen el egy kis idõ a játék elején az elsõ wave indítása elõtt.
    }
    void Update()
    {
        if (BetweenWaves)
        {
            if (timeCountdown > 0) timeCountdown -= Time.deltaTime;
            else
            {
                //Tehát lejárt a wavek közötti idõ...

                if (currentWaveID+1 > Waves) 
                {
                    Debug.Log("GG! Összes wave elfogyott.");
                    //TODO: jövõben itt következõ map / menü.
                }
                else
                {
                    currentWaveID++;
                    transform.GetChild(0).gameObject.SetActive(true);
                    currentWave = transform.GetChild(0).GetComponent<Wave>();
                    Debug.Log("Következõ wave betöltve.");
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

    //Ez azért fontos eljárás, mert az összes torony a WaveManager.enemies alapján nézi az ellenségeket, nem pedig végignézi az összes gameobjectet, hogy az-e.
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
