using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VR_Weapon_Radial_Option : MonoBehaviour
{
    //Ez még nagyon proof of concept kód, még nincs teljesen kitalálva, hogy hogyan legyen megoldva a weapon radial, emiatt nincsenek kiszedve olyan sorok
    //amik jelenleg nincsenek használva.
    [SerializeField] int Id;
    [SerializeField] VR_Weapon_Radial radial;

    private void Start()
    {
        radial = GetComponentInParent<VR_Weapon_Radial>();
    }

    private void HandHoverUpdate(Hand hand)
    {
        if (hand.GetGrabStarting() != GrabTypes.None) 
        {
            Debug.Log("megfogta!");
            //radial.ChosenWeapon(Id, hand);
            VR_Weapons.instance.ChangeWeapon(Id, hand);
        }
    }
}
