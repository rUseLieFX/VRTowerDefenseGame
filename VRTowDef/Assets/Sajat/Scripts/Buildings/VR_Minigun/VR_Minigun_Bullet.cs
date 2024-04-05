using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Minigun_Bullet : MonoBehaviour
{
    [SerializeField] float speed;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (transform.position.y < -2) Destroy(gameObject);

    }
}
