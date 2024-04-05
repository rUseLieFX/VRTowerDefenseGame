using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_MinigunModel : MonoBehaviour
{
    //Ez csak azért kell, hogy ne az egész csövet lehessen megfogni, hanem csak a fogót. Ezzel tartjuk egyben a "modellt".
    [SerializeField] Transform hinge;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = hinge.position;
        transform.rotation = hinge.rotation;
    }
}
