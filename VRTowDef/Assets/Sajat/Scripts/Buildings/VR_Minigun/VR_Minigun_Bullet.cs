using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Mivel ez csak egy "particle" konkr�tan, csak annyi van megadva neki hogy haladjon el�re, �s ha a p�lya alatt van, akkor t�rl�dj�n.
public class VR_Minigun_Bullet : MonoBehaviour
{
    [SerializeField] float speed;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (transform.position.y < -2) Destroy(gameObject);

    }
}
