using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    public static BaseScript instance;
    [SerializeField] int maxHp, health;
    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("Több BaseScript fut!");
            Destroy(gameObject);
        }
        instance = this;
    }

    void Start()
    {
        health = maxHp;
    }
    
    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0) 
        {
            Debug.Log("Meghaltunk, common L!");
        }
    }
}
