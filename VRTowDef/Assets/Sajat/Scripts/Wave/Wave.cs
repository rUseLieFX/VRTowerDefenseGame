using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Wave : MonoBehaviour
{

    /* A wave tárolja azt, hogy:
     * 
     * - Editoron belül, hogy mikor minek kell történnie. 
     * - Mennyi a módosítatlan idõ az ellenségek spawnolása között.
     * - Melyik bejárathoz spawnolhatnak.
     * 
     * A legelején ki lehet - sõt érdemes - kitölteni az információkat a Setup részben.
     */


    [Header("Setup")]
    [SerializeField] float waitTimeBetweenSpawns;
    [SerializeField] float timeRemaining;
    [SerializeField] bool[] spawnTo;

    [Header("Debug")]
    [SerializeField] GameObject[] events;
    [SerializeField] int eventPos; // Hol járunk az eventeknél?
    [SerializeField] GameObject[] enemiesToSpawn;
    [SerializeField] bool spawningEnemies = false;
    [SerializeField] int spawningEnemiesCount = 0;
    private void Awake()
    {
        eventPos = 0;
        List<GameObject> list = new List<GameObject>();
        foreach (Transform item in transform)
        {
            list.Add(item.gameObject);
            item.gameObject.SetActive(false);
        }
        events = list.ToArray();
    }

    private void Start()
    {
        timeRemaining = 0;
    }

    public void WaitTime(float waitTime)
    {
        timeRemaining = waitTime;
    }

    public void ChangeSpawnTime(float waitTime)
    {
        waitTimeBetweenSpawns = waitTime;
    }
    public void ChangeSpawns(bool[] spawns)
    {
        spawnTo = spawns;
    }
    public void ChangeNextEnemies(GameObject[] enemies, int enemyCount)
    {
        enemiesToSpawn = enemies;
        spawningEnemies = true;
        timeRemaining = waitTimeBetweenSpawns;
        spawningEnemiesCount = enemyCount;
    }

    void SpawnEnemies()
    {
        List<int> availableSpawns = new List<int>();


        for (int i = 0; i < spawnTo.Length; i++)
        {
            if (spawnTo[i]) availableSpawns.Add(i);
            //Ha az adott spawnra spawnolhatunk, adjuk hozzá, hogy melyik számúra spawnolhat.
            //Például, ha olyan sort kap, hogy (0,1,1,0)
            //Akkor tudjuk hogy az 1-es illetve a 2-es számú spawnokra lehet spawnolni.
        }

        if (availableSpawns.Count < enemiesToSpawn.Length)
        {
            Debug.LogError("Kevesebb szabad spawnlehetõség van, mint amennyi ellenfelet kéne spawnolni!");
        }



        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            //Ha bármi miatt több spawn van "feloldva", mint amennyi ellenséget akarunk spawnolni, akkor fog ez lefutni.
            //Ha 3 spawn van feloldva, de csak kettõt kaptunk, akkor az alapján, hogy hol "üres", tudjuk hogy hova akarunk spawnolni ellenséget.

            //Igen, emiatt a maga módján a Spawn feloldás redundáns, de nem baj - több lehetõség van arra, hogy hogyan spawnolhatjuk különbözõ helyekre az ellenségeket.
            if (enemiesToSpawn[i] == null) continue;

            GameObject enemy = Instantiate(enemiesToSpawn[i], Vector3.zero, Quaternion.identity);
            //Hozzuk létre az ellenséget, mindegy hogy hol, mivel a kódja azonnal a bejárathoz teleportálja.
            enemy.GetComponent<Enemy>().PathToTake = availableSpawns[i];
            //Adjuk át neki, hogy hova kell spawnolni.
        }
        if (spawningEnemiesCount <= 0) spawningEnemies = false;
        else
        {
            timeRemaining = waitTimeBetweenSpawns;
            spawningEnemiesCount--;
        }
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            //Nincs aktív idõzítõ / várakozási idõ.

            if (spawningEnemies)
            {
                SpawnEnemies();
            }
            if (spawningEnemiesCount <= 0) GetNextEvent();
        }
    }

    void GetNextEvent()
    {
        //Ha már nem maradt semmi hátra.

        if (eventPos == events.Length)
        {
            Debug.Log("Véget ért a Wave!");
            WaveManager.instance.WaveFinished();
            Destroy(gameObject);
            return;
        }
        else 
        if (eventPos != 0) { events[eventPos-1].SetActive(false); }
        events[eventPos].SetActive(true);
        eventPos++;


        
    }
}
