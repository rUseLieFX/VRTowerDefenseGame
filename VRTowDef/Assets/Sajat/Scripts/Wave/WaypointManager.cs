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
    //Betöltéskor az összes waypointot feljegyzi magának.
    void Start()
    {
        //Valamiért a Unity jobbnak találta azt, hogy a childokat ne úgy lehessen megszerezni, hogy GetComponent, hanem úgy, hogy foreach-be van rakva a transform.
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
    /// Megadja az összes waypointot egy adott útvonalban.
    /// </summary>
    /// <param name="id">A kívánt útvonal ID-je</param>
    public Transform[] GetWaypoints(int id)
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform t in waypoints) //Átnézi a lementett waypointokat...
        {
            int waypointID = t.gameObject.GetComponent<Waypoint>().ID;
            if (waypointID == id) list.Add(t); //Ha a nézett waypoint azé az útvonalé, amelyik lett kérve, legyen eltárolva.
        }
        Transform[] vissza = list.ToArray();

        return vissza; //Visszaküldeni már csak azokat a waypointokat küldi vissza, amely útvonalét akarja.
    }

    /// <summary>
    /// Milyen hosszú az útvonal.
    /// </summary>
    /// <param name="id">Az útvonal ID-je.</param>
    public float GetPathLength(int id) 
    {
        return pathLengths[id];
    }
}
