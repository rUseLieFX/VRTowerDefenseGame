using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Wave : MonoBehaviour
{

    /* A wave t�rolja azt, hogy:
     * 
     * - Editoron bel�l, hogy mikor minek kell t�rt�nnie. 
     * - Mennyi a m�dos�tatlan id� az ellens�gek spawnol�sa k�z�tt.
     * - Melyik bej�rathoz spawnolhatnak.
     * 
     * A legelej�n ki lehet - s�t �rdemes - kit�lteni az inform�ci�kat a Setup r�szben.
     */


    [Header("Setup")]
    [SerializeField] float waitTimeBetweenSpawns;
    [SerializeField] float timeRemaining;
    [SerializeField] bool[] spawnTo;

    [Header("Debug")]
    [SerializeField] GameObject[] events;
    [SerializeField] int eventPos; // Hol j�runk az eventekn�l?
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
            //Ha az adott spawnra spawnolhatunk, adjuk hozz�, hogy melyik sz�m�ra spawnolhat.
            //P�ld�ul, ha olyan sort kap, hogy (0,1,1,0)
            //Akkor tudjuk hogy az 1-es illetve a 2-es sz�m� spawnokra lehet spawnolni.
        }

        if (availableSpawns.Count < enemiesToSpawn.Length)
        {
            Debug.LogError("Kevesebb szabad spawnlehet�s�g van, mint amennyi ellenfelet k�ne spawnolni!");
        }



        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            //Ha b�rmi miatt t�bb spawn van "feloldva", mint amennyi ellens�get akarunk spawnolni, akkor fog ez lefutni.
            //Ha 3 spawn van feloldva, de csak kett�t kaptunk, akkor az alapj�n, hogy hol "�res", tudjuk hogy hova akarunk spawnolni ellens�get.

            //Igen, emiatt a maga m�dj�n a Spawn felold�s redund�ns, de nem baj - t�bb lehet�s�g van arra, hogy hogyan spawnolhatjuk k�l�nb�z� helyekre az ellens�geket.
            if (enemiesToSpawn[i] == null) continue;

            GameObject enemy = Instantiate(enemiesToSpawn[i], Vector3.zero, Quaternion.identity);
            //Hozzuk l�tre az ellens�get, mindegy hogy hol, mivel a k�dja azonnal a bej�rathoz teleport�lja.
            enemy.GetComponent<Enemy>().PathToTake = availableSpawns[i];
            //Adjuk �t neki, hogy hova kell spawnolni.
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
            //Nincs akt�v id�z�t� / v�rakoz�si id�.

            if (spawningEnemies)
            {
                SpawnEnemies();
            }
            if (spawningEnemiesCount <= 0) GetNextEvent();
        }
    }

    void GetNextEvent()
    {
        //Ha m�r nem maradt semmi h�tra.

        if (eventPos == events.Length)
        {
            Debug.Log("V�get �rt a Wave!");
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
