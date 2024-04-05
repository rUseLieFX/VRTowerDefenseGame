using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Statisztik�k"), Tooltip("Az ellens�g statisztik�i.")]
    [SerializeField] float speed;
    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] int damage;
    [SerializeField] int armor;
    [SerializeField] int moneyGiven;
    private bool dead = false;

    [Header("Damage types")]
    [Tooltip("Sebezhetetlens�g")]
    [SerializeField] DamageType[] inv;

    [Tooltip("Ellen�ll�s")]
    [SerializeField] DamageType[] resist;

    [Tooltip("�rz�kenys�g")]
    [SerializeField] DamageType[] vuln;

    [Header("Targeting asset")]
    [SerializeField] float pathLength; //Mennyi van m�g h�tra az �tj�b�l.
    [SerializeField] float startWeight; //Mennyi a kiindul� vesz�ly �rt�ke (ezt nem kell megadni).
    [SerializeField] float weight; //A targeting-ben mennyire legyen vesz�lyesnek �t�lve.
    [SerializeField] float weightChange; //HP alapj�n mennyit v�ltozzon a s�lya.

    public float PathLength { get { return pathLength; } }
    public float Weight { get { return weight; } }
    public bool IsDead { get { return dead; } }

    [Header("Debug")]
    [SerializeField] int pathToTake; //Melyik �tvonalon menjen.
    public int PathToTake { set { pathToTake = value; } }
    [SerializeField] int target = 1; //Hanyadik waypontot keresi.
    [SerializeField] Vector3 targetPos;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float slowPercent = 1; //Igaz�b�l ez egy sebess�g szorz�, az 1 a 100% mozg�s.
    [SerializeField] float slowTimer;

    [Header("Sz�ps�g")]
    [SerializeField] Slider slider;




    void Start()
    {
        slowPercent = 1;
        WaveManager.instance.AddEnemy(this); //Jelentkezzen be a nyilv�ntart�sba.
        waypoints = WaypointManager.instance.GetWaypoints(pathToTake); //Szerezze meg a haszn�land� waypontokat.
        pathLength = WaypointManager.instance.GetPathLength(pathToTake); //Szerezze meg, hogy mekkora t�vot kell megtennie.
        targetPos = waypoints[target].position + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
        //Szerezze meg, hogy hova kell menni + legyen egy kis RNG benne, m�gse egy pontra menjen az �SSZES ellens�g.

        transform.position = waypoints[0].position; //Ott jelenjen meg, ahol a legels� waypoint van.
        health = maxHealth;
        weight = startWeight;
        if (slider != null) slider.value = 1;
    }



    void Update()
    {
        TakeStep();
    }

    void TakeStep()
    {
        Vector3 pos = transform.position;
        transform.position = Vector3.MoveTowards(pos, targetPos, slowPercent * speed * Time.deltaTime);
        pathLength -= slowPercent * speed * Time.deltaTime;

        if (pos == targetPos) //Ha el�rt oda, ahova mennie kellett...
        {
            if (target < waypoints.Length - 1)
            {
                target++;
                targetPos = waypoints[target].position;

                if (target != waypoints.Length - 1) targetPos += new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
                //Ha nem az utols� waypointra megy, akkor legyen egy kicsit random.
            }
            else
            {
                Debug.Log("Be�rt!");
                BaseScript.instance.Damage(damage);
                Death();
                return;
            }
        }

        if (slowTimer > 0) slowTimer -= Time.deltaTime;
        else slowPercent = 1; 
    }

    public void Damage(int damage, DamageType dmgtype)
    {
        //Ha az adott sebz�s t�pusra sebezhetetlen, akkor csak return.
        List<DamageType> invu = inv.ToList();
        if (invu.Contains(dmgtype))
        {
            return;
        }

        //Ha az adott sebz�s t�pusra resistes, fele akkora sebz�st kapjon.
        List<DamageType> res = resist.ToList();
        if (res.Contains(dmgtype))
        {
            damage /= 2;
        }


        List<DamageType> vul = vuln.ToList();
        if (vul.Contains(dmgtype))
        {
            float multipliedDamage = (float)damage * 1.5f;
            damage = Mathf.RoundToInt(multipliedDamage);
        }

        damage -= armor;
        health -= (damage > 1) ? damage : 1; //Minimum 1 pontnyi s�r�l�st kapjon be.
        
        weight = startWeight - weightChange * (1f - ((float)health / (float)maxHealth)); 
        //Ha v�ltoznia kell a weight-nek az alapj�n hogy mennyi a max HP-ja, az itt van kisz�molva, hogy mennyivel cs�kkenjen.

        if (health <= 0)
        {
            if (!dead) Death();
            else Debug.Log("Jaj, pont �gy kaptam s�r�l�st, hogy m�r meghaltam mire j�nne �jabb adag!");
        }
        else
        {
            if (slider != null) slider.value = (float)health / (float)maxHealth;
        }
    }

    public void Slow(float slowPerc, float slowTime) 
    {
        if (slowPerc <= slowPercent) //Ha egy eddigin�l er�sebb lass�t�s van alkalmazva vagy ugyan akkora, akkor �rja fel�l az eddigit.
        {
            slowPercent = slowPerc;
            slowTimer = slowTime;
        }
        else Debug.Log($"Megpr�b�ltak lass�tani, de van m�r rajtam egy er�sebb hat�s! ({slowPerc} > {slowPercent})");
    }

    void Death()
    {
        dead = true;
        BuildManager.instance.Money(moneyGiven); //Adja oda a p�nzt, amennyit �r.
        WaveManager.instance.RemoveEnemy(this); //Jelentse be, hogy meghalt.
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < waypoints.Length-1; i++)
        {
            Debug.DrawLine(waypoints[i].position, waypoints[i+1].position);
            Debug.DrawLine(transform.position, targetPos, Color.yellow);
        }
    }
}

public enum DamageType
{
    Pierce = 0,
    Explosion = 1,
    Magic = 2,
    Laser = 3
}
