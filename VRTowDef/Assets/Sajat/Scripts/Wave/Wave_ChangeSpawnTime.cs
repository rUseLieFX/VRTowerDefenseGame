using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_ChangeSpawnTime : MonoBehaviour
{
    [SerializeField] float spawnTime;
    void Start()
    {
        WaveManager.instance.CurrentWave.ChangeSpawnTime(spawnTime);
    }
}
