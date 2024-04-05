using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VR_Weapon_Radial_Option : MonoBehaviour
{
    //Ez m�g nagyon proof of concept k�d, m�g nincs teljesen kital�lva, hogy hogyan legyen megoldva a weapon radial, emiatt nincsenek kiszedve olyan sorok
    //amik jelenleg nincsenek haszn�lva.
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
