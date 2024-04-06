using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Mivel ez csak egy "particle" konkrétan, csak annyi van megadva neki hogy haladjon elõre, és ha a pálya alatt van, akkor törlõdjön.
public class VR_Minigun_Bullet : MonoBehaviour
{
    [SerializeField] float speed;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (transform.position.y < -2) Destroy(gameObject);

    }
}
