using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

// Ez intézi a fegyverváltást a VR játékosnak.
// Elég erõsen proof of concept kód, nem hibátlan - még nem is az volt a cél, hanem az, hogy lássam hogy meg tudom-e csinálni.
// Amint elégedett vagyok az opciókkal, tisztítani fogom a kódot.
public class VR_Weapons : MonoBehaviour
{
    [SerializeField] private int weaponId; //Kiválasztott fegyver ID-je.
    [SerializeField] private Hand weaponHand; //A fegyvert használó kéz
    [SerializeField] private VR_Weapon[] weapons; //A használható fegyverek tömbje
    [SerializeField] GameObject weaponRadial; //A weapon radial menü.

    [Header("Beállítandók")]

    [SerializeField] private Hand[] hands;
    [SerializeField] SteamVR_ActionSet weaponActionSet;
    [SerializeField] SteamVR_Action_Boolean openRadial;
    public static VR_Weapons instance;
    public Hand WeaponHand {  get { return weaponHand; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Betöltéskor egy fegyver se legyen aktív.
        weapons = GetComponentsInChildren<VR_Weapon>();
        foreach (var item in weapons)
        {
            item.gameObject.SetActive(false);
        }


    }

    void Update()
    {
        if (weaponId != 0) //A 0-s ID a kéz, ha nem arra akarunk váltani, akkor fegyverre.
        {
            weaponHand.AttachObject(weapons[weaponId - 1].gameObject,GrabTypes.Scripted);
        }

        foreach (var hand in hands)
        {
            if (openRadial.GetStateDown(hand.handType)) 
            {
                OpenRadial(hand); //Ha valamelyik kézzel megnyomjuk az Open Radial gombot, akkor annál a kéznél jelenjen meg.
            }
        }
       
        
    }

    void OpenRadial(Hand hand)
    {
        weaponRadial.SetActive(true); //Legyen bekapcsolva.
        Vector3 directonSpawn = (-hand.transform.up * 5 + hand.transform.forward * 2).normalized/4; //Milyen irányba spawnoljon (a kéztõl egyenesen el)
        Vector3 spawnPosition = hand.transform.position + directonSpawn; //Hol jelenjen meg - a kézhez képest a kis offset-tel együtt.
        weaponRadial.transform.position = spawnPosition;
        weaponRadial.transform.rotation = Quaternion.LookRotation(hand.transform.position - spawnPosition);
    }

    public void ChangeWeapon(int chosen, Hand newWeaponHand) 
    {
        Debug.Log($"Megpróbálok váltani! Errõl: {weaponId} Erre: {chosen}");
        weaponRadial.SetActive(false);
        if (weaponId != chosen) //Ha új fegyverre próbálunk váltani.
        {
            if (weaponId != 0) //Ha eddig nem kéz volt a kezünkben.
            {
                weaponHand.DetachObject(weapons[weaponId - 1].gameObject, true); //Vegyük ki a kézben lévõ fegyvert.
                weapons[weaponId - 1].gameObject.SetActive(false); //Kapcsoljuk ki.
            }
            else //Ha eddig kéz volt a kezünkben...
            {
                weaponActionSet.Activate(newWeaponHand.handType, 10,false); // Akkor aktiváljuk a fegyveres control layoutout.
            }

            if (chosen != 0) //Ha nem kézre akarunk váltani.
            {
                weapons[chosen - 1].gameObject.SetActive(true); //Kapcsoljuk be.
                newWeaponHand.AttachObject(weapons[chosen - 1].gameObject, GrabTypes.Scripted); //Rakjuk bele a kézbe.
            }
            else //Kézre akarunk váltani
            {
                weaponActionSet.Deactivate(weaponHand.handType); // Deaktiváljuk a fegyveres control layoutot.
            }
        }

        weaponId = chosen;
        weaponHand = newWeaponHand;
    }
}
