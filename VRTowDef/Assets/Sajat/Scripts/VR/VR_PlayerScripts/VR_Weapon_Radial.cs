using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VR_Weapon_Radial : MonoBehaviour
{
    //Nem haszn�lt k�d jelenleg.
    public void ChosenWeapon(int chosen, Hand choosingHand) 
    {
        VR_Weapons.instance.ChangeWeapon(chosen, choosingHand);
    }
}
