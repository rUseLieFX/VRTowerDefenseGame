using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager instance;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float[] pathLengths;

    public Transform[] Waypoints { get { return waypoints; } }

    private void Awake()
    {
        instance = this;
    }
    //Bet�lt�skor az �sszes waypointot feljegyzi mag�nak.
    void Start()
    {
        //Valami�rt a Unity jobbnak tal�lta azt, hogy a childokat ne �gy lehessen megszerezni, hogy GetComponent, hanem �gy, hogy foreach-be van rakva a transform.
        List<Transform> list = new List<Transform>();
        foreach (Transform item in transform)
        {
            list.Add(item);
        }
        waypoints = list.ToArray();


        int lastId = list[list.Count - 1].GetComponent<Waypoint>().ID;
        pathLengths = new float[lastId+1];

        for (int i = 0; i <= lastId; i++)
        {
            Transform[] path = GetWaypoints(i);
            float distance = 0;
            distance += Vector3.Distance(path[0].position, path[1].position);
            for (int j = 1; j < path.Length-1; j++)
            {
                distance += Vector3.Distance(path[j].position, path[j + 1].position);
            }
            pathLengths[i] = distance;
        }
        
    }

    /// <summary>
    /// Megadja az �sszes waypointot egy adott �tvonalban.
    /// </summary>
    /// <param name="id">A k�v�nt �tvonal ID-je</param>
    public Transform[] GetWaypoints(int id)
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform t in waypoints) //�tn�zi a lementett waypointokat...
        {
            int waypointID = t.gameObject.GetComponent<Waypoint>().ID;
            if (waypointID == id) list.Add(t); //Ha a n�zett waypoint az� az �tvonal�, amelyik lett k�rve, legyen elt�rolva.
        }
        Transform[] vissza = list.ToArray();

        return vissza; //Visszak�ldeni m�r csak azokat a waypointokat k�ldi vissza, amely �tvonal�t akarja.
    }

    /// <summary>
    /// Milyen hossz� az �tvonal.
    /// </summary>
    /// <param name="id">Az �tvonal ID-je.</param>
    public float GetPathLength(int id) 
    {
        return pathLengths[id];
    }
}
