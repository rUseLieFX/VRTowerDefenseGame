using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Statisztikák"), Tooltip("Az ellenség statisztikái.")]
    [SerializeField] float speed;
    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] int damage;
    [SerializeField] int armor;
    [SerializeField] int moneyGiven;
    private bool dead = false;

    [Header("Damage types")]
    [Tooltip("Sebezhetetlenség")]
    [SerializeField] DamageType[] inv;

    [Tooltip("Ellenállás")]
    [SerializeField] DamageType[] resist;

    [Tooltip("Érzékenység")]
    [SerializeField] DamageType[] vuln;

    [Header("Targeting asset")]
    [SerializeField] float pathLength; //Mennyi van még hátra az útjából.
    [SerializeField] float startWeight; //Mennyi a kiinduló veszély értéke (ezt nem kell megadni).
    [SerializeField] float weight; //A targeting-ben mennyire legyen veszélyesnek ítélve.
    [SerializeField] float weightChange; //HP alapján mennyit változzon a súlya.

    public float PathLength { get { return pathLength; } }
    public float Weight { get { return weight; } }
    public bool IsDead { get { return dead; } }

    [Header("Debug")]
    [SerializeField] int pathToTake; //Melyik útvonalon menjen.
    public int PathToTake { set { pathToTake = value; } }
    [SerializeField] int target = 1; //Hanyadik waypontot keresi.
    [SerializeField] Vector3 targetPos;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float slowPercent = 1; //Igazából ez egy sebesség szorzó, az 1 a 100% mozgás.
    [SerializeField] float slowTimer;

    [Header("Szépség")]
    [SerializeField] Slider slider;




    void Start()
    {
        slowPercent = 1;
        WaveManager.instance.AddEnemy(this); //Jelentkezzen be a nyilvántartásba.
        waypoints = WaypointManager.instance.GetWaypoints(pathToTake); //Szerezze meg a használandó waypontokat.
        pathLength = WaypointManager.instance.GetPathLength(pathToTake); //Szerezze meg, hogy mekkora távot kell megtennie.
        targetPos = waypoints[target].position + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
        //Szerezze meg, hogy hova kell menni + legyen egy kis RNG benne, mégse egy pontra menjen az ÖSSZES ellenség.

        transform.position = waypoints[0].position; //Ott jelenjen meg, ahol a legelsõ waypoint van.
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

        if (pos == targetPos) //Ha elért oda, ahova mennie kellett...
        {
            if (target < waypoints.Length - 1)
            {
                target++;
                targetPos = waypoints[target].position;

                if (target != waypoints.Length - 1) targetPos += new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
                //Ha nem az utolsó waypointra megy, akkor legyen egy kicsit random.
            }
            else
            {
                Debug.Log("Beért!");
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
        //Ha az adott sebzés típusra sebezhetetlen, akkor csak return.
        List<DamageType> invu = inv.ToList();
        if (invu.Contains(dmgtype))
        {
            return;
        }

        //Ha az adott sebzés típusra resistes, fele akkora sebzést kapjon.
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
        health -= (damage > 1) ? damage : 1; //Minimum 1 pontnyi sérülést kapjon be.
        
        weight = startWeight - weightChange * (1f - ((float)health / (float)maxHealth)); 
        //Ha változnia kell a weight-nek az alapján hogy mennyi a max HP-ja, az itt van kiszámolva, hogy mennyivel csökkenjen.

        if (health <= 0)
        {
            if (!dead) Death();
            else Debug.Log("Jaj, pont úgy kaptam sérülést, hogy már meghaltam mire jönne újabb adag!");
        }
        else
        {
            if (slider != null) slider.value = (float)health / (float)maxHealth;
        }
    }

    public void Slow(float slowPerc, float slowTime) 
    {
        if (slowPerc <= slowPercent) //Ha egy eddiginél erõsebb lassítás van alkalmazva vagy ugyan akkora, akkor írja felül az eddigit.
        {
            slowPercent = slowPerc;
            slowTimer = slowTime;
        }
        else Debug.Log($"Megpróbáltak lassítani, de van már rajtam egy erõsebb hatás! ({slowPerc} > {slowPercent})");
    }

    void Death()
    {
        dead = true;
        BuildManager.instance.Money(moneyGiven); //Adja oda a pénzt, amennyit ér.
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
