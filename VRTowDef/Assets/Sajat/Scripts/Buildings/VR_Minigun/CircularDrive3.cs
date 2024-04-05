using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CircularDrive3 : MonoBehaviour
{
    private bool grabbed;
    private Hand hand;
    private GrabTypes grabbedWithType;

    private Quaternion _delta;

    private void Start()
    {
        grabbed = false;
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (hand == this.hand || !grabbed)
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabbingWithType(grabbedWithType) == false;

            if (grabbedWithType == GrabTypes.None && startingGrabType != GrabTypes.None) //Ha eddig nem volt megfogva...
            {
                grabbedWithType = startingGrabType;

                grabbed = true;
                this.hand = hand;
                //Akkor jegyezz�k le hogy melyik k�zzel van fogva.


                var lookAt = Quaternion.LookRotation(hand.hoverSphereTransform.position - transform.position);

                _delta = Quaternion.Inverse(lookAt) * transform.rotation;

            }

            else if (grabbedWithType != GrabTypes.None && isGrabEnding) //Ha el van engedve
            {
                grabbed = false;
                grabbedWithType = GrabTypes.None;
                this.hand = null;

            }
        }

    }
    private void Update()
    {
        if (grabbed) //Forogjon amerre c�loz a k�z.
        {
            transform.rotation = Quaternion.LookRotation(hand.hoverSphereTransform.position - transform.position) * _delta;
        }
    }

}
