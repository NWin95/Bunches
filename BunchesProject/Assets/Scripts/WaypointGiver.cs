using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointGiver : MonoBehaviour {

    public List<Vector3> waypoints = new List<Vector3>();
    public int mode;    //0 = Point     1 = Patrol      2 = Sentry      3 = Stationary
    public int airSkill;

    void Start ()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        tag = "WaypointGiver";
        GetComponent<Renderer>().enabled = false;
    }
}
