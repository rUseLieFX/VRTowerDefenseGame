using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_MinigunModel : MonoBehaviour
{
    //Ez csak az�rt kell, hogy ne az eg�sz cs�vet lehessen megfogni, hanem csak a fog�t. Ezzel tartjuk egyben a "modellt".
    [SerializeField] Transform hinge;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = hinge.position;
        transform.rotation = hinge.rotation;
    }
}
