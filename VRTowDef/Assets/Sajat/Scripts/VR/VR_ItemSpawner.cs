using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_ItemSpawner : MonoBehaviour
{
    //Ez itt nagyon-nagyon prototipus k�d, jelenleg m�g csak proof of concept, a j�v�ben nagy val�sz�n�s�ggel szinte 0-r�l lenne �jra�rva.
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
        if (item == null && hasItem) //Ha m�r nincs n�la az item, de m�g �gy gondolja hogy igen, akkor el lett v�ve -> kezd�dj�n el a respawn.
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
