using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] int count = 1;

    private void Start()
    {
        WaveManager.instance.CurrentWave.ChangeNextEnemies(enemies, count);
    }
}
