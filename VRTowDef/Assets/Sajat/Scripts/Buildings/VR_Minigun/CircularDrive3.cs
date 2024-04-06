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

    //SteamVR API saj�t elj�r�sa - valami�rt nem jelzi, hogy haszn�lva van. Kell, hogy interakt�l�sn�l valami saj�t k�d letudjon futni.
    private void HandHoverUpdate(Hand hand)
    {
        if (hand == this.hand || !grabbed) //Ha nincs megfogva VAGY azzal a k�zzel �r�nk hozz�, amivel m�r meg lett fogva...
        {
            GrabTypes startingGrabType = hand.GetGrabStarting(); //Jegyezz�k meg, milyen m�don van fogva.
            bool isGrabEnding = hand.IsGrabbingWithType(grabbedWithType) == false; //Ha ugyan azt a gombot megnyomja, amivel elkezdte megfogni, akkor elakarja engedni.

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
