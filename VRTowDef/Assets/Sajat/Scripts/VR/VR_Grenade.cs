using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class VR_Grenade : VR_Item
{
    [SerializeField] float range;
    [SerializeField] int dmg;
    [SerializeField] bool boomboom; //Robbanhat-e?
    [SerializeField] GameObject particle;
    public bool BoomBoom { set { boomboom = value; } }


    void Update()
    {
        if (transform.position.y < -2)
        {
            Destroy(gameObject);
        }   
    }

    public void Thrown()
    {
        //Ha el van dobva, akkor onnantÛl kezdve robbanjon ha b·rmihez is hozz·Èr, Ès tudjon szabadjon forogni, mozogni.
        boomboom = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Ha b·rmihez hozz·Èrt, Ès m·r robbanhat...
        if (boomboom)
        {
            Instantiate(particle, transform.position,Quaternion.identity);
            Enemy[] enemies = WaveManager.instance.Enemies;
            foreach (var item in enemies)
            {
                if (Vector3.Distance(item.transform.position, transform.position) <= range) item.Damage(dmg,DamageType.Explosion); 
            }


            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
