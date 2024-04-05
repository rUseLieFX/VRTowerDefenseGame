using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VR_MinigunScript : MonoBehaviour
{
    [SerializeField] SteamVR_ActionSet minigunSet;
    [SerializeField] SteamVR_Action_Boolean minigunFireAction;

    [SerializeField] int damage;
    [SerializeField] float attackTime;

    [SerializeField] Transform minigunGunPoint;
    [SerializeField] BoxCollider handleBc;
    [SerializeField] GameObject bulletPrefab;


    float attackTimer;

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
        //Ne tudjuk mindkét kézzel megfogni
        if (hand == this.hand || !grabbed)
        {
            /*
            //Neten talált, mûködõ kód, amiatt van itt, hogy ha bármi gond lenne, lehessen mibõl segítséget szerezni.
            GrabTypes startingGrabType = hand.GetGrabStarting();
            Debug.Log(startingGrabType);
            bool isGrabEnding = hand.IsGrabbingWithType(grabbedWithType) == false;

            if (grabbedWithType == GrabTypes.None && startingGrabType != GrabTypes.None)
            {
                grabbedWithType = startingGrabType;

                grabbed = true;
                this.hand = hand;

                var lookAt = Quaternion.LookRotation(hand.hoverSphereTransform.position - transform.position);

                _delta = Quaternion.Inverse(lookAt) * transform.rotation;
            }

            else if (grabbedWithType != GrabTypes.None && isGrabEnding)
            {
                minigunSet.Deactivate(hand.handType);
                grabbed = false;
                grabbedWithType = GrabTypes.None;
                this.hand = null;

                transform.rotation = transform.rotation * Quaternion.Euler(1, 1, 0);

            }//*/

            
            GrabTypes grabType = hand.GetGrabStarting();
            
            
            //Ha valamivel ténylegesen megpróbáljuk megfogni - és még nincsen megfogva.
            if (grabType != GrabTypes.None && !grabbed)
            {
                Debug.Log(grabType);
                Vector3 dir = hand.hoverSphereTransform.position - transform.position;
                var lookAt = Quaternion.LookRotation(dir);
                _delta = Quaternion.Inverse(lookAt) * transform.rotation;

                grabbed = true;
                this.hand = hand;

                minigunSet.Activate(hand.handType, 10); //Legyen aktiválva a minigun controller layout a fogó kézen.
                handleBc.size *= 2; //Legyen megnövelve a handle hitbox-a, hogy messzebb lehessen vinni a kezet anélkül hogy autómatikusan elengedné.
            }
            else if (grabType != GrabTypes.None && grabbed) //Ha megint megpróbálunk ráfogni, de már meg volt fogva.
            {
                Ungrab();
            }
            
            
        }

    }

    void OnHandHoverEnd(Hand hand) 
    {
        if (hand == this.hand && grabbed) Ungrab(); 
    }

    void Ungrab()
    {
        minigunSet.Deactivate(hand.handType); //Deaktiváljuk a minigun controller layoutot.
        grabbed = false;
        hand = null;

        transform.rotation = transform.rotation * Quaternion.Euler(1, 1, 0);
        handleBc.size /= 2;


    }
    private void FixedUpdate()
    {
        if (grabbed)
        {
            Vector3 dir = hand.hoverSphereTransform.position - transform.position;

            transform.rotation = Quaternion.LookRotation(dir) * _delta;
        }
    }

    private void Update()
    {
        if (grabbed) 
        {
            
            if (minigunFireAction.GetState(hand.handType))
            {
                if (attackTimer <= 0)
                {
                    Shoot();
                }
            }
        }

        if (attackTimer > 0) attackTimer -= Time.deltaTime;
    }

    void Shoot()
    {
        attackTimer = attackTime;


        RaycastHit outhit; 
        if (Physics.Raycast(minigunGunPoint.position, transform.forward, out outhit))
        {
            if (outhit.transform.GetComponent<Enemy>() != null)
            {
                outhit.transform.GetComponent<Enemy>().Damage(damage, DamageType.Pierce);
            }
        }


        GameObject bullet = Instantiate(bulletPrefab,minigunGunPoint.position,transform.rotation);
        Destroy(bullet, 1f);

        //Maga a lövés az hitscan, de legyen valami szép effektje, emiatt létrehozunk egy nagyon gyorsan mozgó lövedéket.
    }
}
