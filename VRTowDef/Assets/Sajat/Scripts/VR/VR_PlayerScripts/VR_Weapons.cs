using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

// Ez int�zi a fegyverv�lt�st a VR j�t�kosnak.
// El�g er�sen proof of concept k�d, nem hib�tlan - m�g nem is az volt a c�l, hanem az, hogy l�ssam hogy meg tudom-e csin�lni.
// Amint el�gedett vagyok az opci�kkal, tiszt�tani fogom a k�dot.
public class VR_Weapons : MonoBehaviour
{
    [SerializeField] private int weaponId; //Kiv�lasztott fegyver ID-je.
    [SerializeField] private Hand weaponHand; //A fegyvert haszn�l� k�z
    [SerializeField] private VR_Weapon[] weapons; //A haszn�lhat� fegyverek t�mbje
    [SerializeField] GameObject weaponRadial; //A weapon radial men�.

    [Header("Be�ll�tand�k")]

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
        //Bet�lt�skor egy fegyver se legyen akt�v.
        weapons = GetComponentsInChildren<VR_Weapon>();
        foreach (var item in weapons)
        {
            item.gameObject.SetActive(false);
        }


    }

    void Update()
    {
        if (weaponId != 0) //A 0-s ID a k�z, ha nem arra akarunk v�ltani, akkor fegyverre.
        {
            weaponHand.AttachObject(weapons[weaponId - 1].gameObject,GrabTypes.Scripted);
        }

        foreach (var hand in hands)
        {
            if (openRadial.GetStateDown(hand.handType)) 
            {
                OpenRadial(hand); //Ha valamelyik k�zzel megnyomjuk az Open Radial gombot, akkor ann�l a k�zn�l jelenjen meg.
            }
        }
       
        
    }

    void OpenRadial(Hand hand)
    {
        weaponRadial.SetActive(true); //Legyen bekapcsolva.
        Vector3 directonSpawn = (-hand.transform.up * 5 + hand.transform.forward * 2).normalized/4; //Milyen ir�nyba spawnoljon (a k�zt�l egyenesen el)
        Vector3 spawnPosition = hand.transform.position + directonSpawn; //Hol jelenjen meg - a k�zhez k�pest a kis offset-tel egy�tt.
        weaponRadial.transform.position = spawnPosition;
        weaponRadial.transform.rotation = Quaternion.LookRotation(hand.transform.position - spawnPosition);
    }

    public void ChangeWeapon(int chosen, Hand newWeaponHand) 
    {
        Debug.Log($"Megpr�b�lok v�ltani! Err�l: {weaponId} Erre: {chosen}");
        weaponRadial.SetActive(false);
        if (weaponId != chosen) //Ha �j fegyverre pr�b�lunk v�ltani.
        {
            if (weaponId != 0) //Ha eddig nem k�z volt a kez�nkben.
            {
                weaponHand.DetachObject(weapons[weaponId - 1].gameObject, true); //Vegy�k ki a k�zben l�v� fegyvert.
                weapons[weaponId - 1].gameObject.SetActive(false); //Kapcsoljuk ki.
            }
            else //Ha eddig k�z volt a kez�nkben...
            {
                weaponActionSet.Activate(newWeaponHand.handType, 10,false); // Akkor aktiv�ljuk a fegyveres control layoutout.
            }

            if (chosen != 0) //Ha nem k�zre akarunk v�ltani.
            {
                weapons[chosen - 1].gameObject.SetActive(true); //Kapcsoljuk be.
                newWeaponHand.AttachObject(weapons[chosen - 1].gameObject, GrabTypes.Scripted); //Rakjuk bele a k�zbe.
            }
            else //K�zre akarunk v�ltani
            {
                weaponActionSet.Deactivate(weaponHand.handType); // Deaktiv�ljuk a fegyveres control layoutot.
            }
        }

        weaponId = chosen;
        weaponHand = newWeaponHand;
    }
}
