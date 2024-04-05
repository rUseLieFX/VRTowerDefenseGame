using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_ChangeSpawns : MonoBehaviour
{
    [SerializeField] bool[] openForSpawn;

    private void Start()
    {
        WaveManager.instance.CurrentWave.ChangeSpawns(openForSpawn);
    }
}
