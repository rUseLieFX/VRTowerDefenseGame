using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_ItemSpawner : MonoBehaviour
{
    //Ez itt nagyon-nagyon prototipus kód, jelenleg még csak proof of concept, a jövõben nagy valószínûséggel szinte 0-ról lenne újraírva.
    [SerializeField] GameObject itemPrefab;
    [SerializeField] float respawnTime;
    [SerializeField] float timer;
    [SerializeField] bool hasItem;
    [SerializeField] GameObject item;

    void Start()
    {
        SpawnItem();
    }

    void SpawnItem()
    {
        item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        hasItem = true;
    }

    void Update()
    {
        if (item == null && hasItem) //Ha már nincs nála az item, de még úgy gondolja hogy igen, akkor el lett véve -> kezdõdjön el a respawn.
        {
            timer = respawnTime;
            hasItem = false;
        }
        if (!hasItem)
        {
            timer -= Time.deltaTime;
            if (timer <= 0) SpawnItem();
        }
    }

    private void OnDestroy()
    {
        if (item != null)
        {
            if (!item.GetComponent<VR_Item>().InHands) Destroy(item);
        }
    }
}
