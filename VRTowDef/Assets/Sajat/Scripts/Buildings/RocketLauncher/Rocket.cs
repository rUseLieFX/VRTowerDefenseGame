using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public int dmg;
    public float radius;
    public float turnSpeed;
    public float speed;
    public Enemy target;
    public Vector3 targetPosition;
    public DamageType damageType;

    [SerializeField] GameObject boomPart;

    void Update()
    {
        if (target != null) targetPosition = target.transform.position;
        Vector3 targetDir = targetPosition - transform.position;
        float singleStep = turnSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0f);

        transform.rotation = Quaternion.LookRotation(newDir);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f || transform.position.y < 0f)
        {
            Instantiate(boomPart, transform.position, Quaternion.identity);
            foreach (var item in WaveManager.instance.Enemies)
            {
                if (Vector3.Distance(item.transform.position,transform.position) < radius)
                {
                    item.Damage(dmg, damageType);
                }
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Debug.DrawLine(transform.position, targetPosition);
    }
}
