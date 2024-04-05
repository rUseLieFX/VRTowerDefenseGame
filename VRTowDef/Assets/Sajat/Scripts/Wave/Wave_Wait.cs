using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Wait : MonoBehaviour
{
    [SerializeField] float WaitTime;

    private void Start()
    {
        WaveManager.instance.CurrentWave.WaitTime(WaitTime);
    }
}
