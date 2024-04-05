using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
[RequireComponent (typeof(AllowTeleportWhileAttachedToHand))]
public class VR_Weapon : MonoBehaviour
{
    [SerializeField] SteamVR_Action_Boolean firingAction;
    [SerializeField] float reloadTime;
    [SerializeField] int damage;
    float reloadTimer;
    private void Update()
    {
        if (reloadTimer <= 0)
        {
            if (firingAction.GetState(VR_Weapons.instance.WeaponHand.handType)) //Ha megnyomják a gombot, amivel lehet lõni...
            {
                Debug.Log("Lõttem egyet!");
                RaycastHit outhit;
                if (Physics.Raycast(transform.position, transform.forward, out outhit))
                {
                    if (outhit.transform.GetComponent<Enemy>() != null)
                    {
                        outhit.transform.GetComponent<Enemy>().Damage(damage, DamageType.Pierce);
                    }
                }

                reloadTimer = reloadTime;
            }
        }
        else reloadTimer -= Time.deltaTime;
        
    }
}
