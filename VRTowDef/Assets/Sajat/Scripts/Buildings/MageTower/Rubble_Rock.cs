using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rubble_Rock : MonoBehaviour
{
    public Vector3 targetPos;
    public int dmg;
    public DamageType damageType;
    [SerializeField] float range;
    [SerializeField] float speed;
    [SerializeField] GameObject[] particleEffects;
    [SerializeField] GameObject effectPrefab;

    public float[] effectData;
    public int rockType = -1; //Mivel a lvl 0-ás katapultnak még nincsen különleges hatása, a kõnek se legyen.


    private void Start()
    {
        if (rockType > -1)
        {
            //Ha 0-s a rocktype, akkor a tüzes effekt lesz aktiválva.
            //Ha 1-es a rocktype, akkor a második lesz.
            particleEffects[rockType].SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (transform.position == targetPos) Boom();
    }

    void Boom()
    {

        var osszes_enemy = WaveManager.instance.Enemies;
        foreach (var item in osszes_enemy)
        {
            if (Vector3.Distance(transform.position, item.transform.position) <= range)
            {
                item.Damage(dmg, damageType);
            }
        }

        if (rockType > -1) //A -1-es rocktype az a semleges.
        {
            Vector3 effectSpawn = new Vector3(transform.position.x, -0.5f, transform.position.z);
            Rubble_Rock_Effect rockEffect = Instantiate(effectPrefab, effectSpawn, Quaternion.identity).GetComponent<Rubble_Rock_Effect>();

            rockEffect.effectData = effectData;
            rockEffect.type = (rockType == 0) ? false : true; //Tûz vagy méreg 
        }
        //Debug.Log("kaboom");
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);   
    }
}
