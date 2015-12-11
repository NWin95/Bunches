using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement_Enemy : MonoBehaviour {

    public Animator anim;
    public bool point;
    public bool patrol;
    public bool sentry;
    public bool stationary;

    Rigidbody rig;
    public float speed;
    float horVel;
    Vector3 endPos;

    public List<Vector3> waypoints = new List<Vector3>();
    int waypointInt;
    public float waypointAllowence;

    void Start ()
    {
        rig = GetComponent<Rigidbody>();
        endPos = transform.position;

        anim.SetBool("Grounded", true);
        if (point || patrol)
            LookWaypoint();
    }

	void Update () {
        Animation();
	}

    void FixedUpdate ()
    {
        if (point || patrol)
            WaypointFollow();
        SpeedCheck();
    }

    void Animation ()
    {
        anim.SetFloat("Velocity", horVel);
    }

    void SpeedCheck ()
    {
        Vector3 startPos = transform.position;
        Vector3 difVec = startPos - endPos;
        endPos = transform.position;

        difVec -= rig.velocity;
        difVec.y = 0;
        horVel = difVec.magnitude / Time.fixedDeltaTime;
        //Debug.Log(horVel);
    }

    void WaypointFollow ()
    {
        Vector3 pos = transform.position;
        Vector3 dir = waypoints[waypointInt] - transform.position;
        float dis = dir.magnitude;
        dir.y = 0;
        dir = dir.normalized;
        Vector3 res = dir * speed * Time.fixedDeltaTime;

        rig.MovePosition(pos + res);

        if (dis < waypointAllowence)
        {
            if (waypointInt + 1 == waypoints.Count)
                EndWaypoint();
            else
                NextWaypoint();
        }
    }

    void LookWaypoint ()
    {
        Vector3 lookVec = waypoints[waypointInt] - transform.position;
        lookVec.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;
    }

    void NextWaypoint ()
    {
        waypointInt++;
        LookWaypoint();
    }

    void EndWaypoint ()
    {
        if (point)
            point = !point;
        if (patrol)
            waypointInt = 0;
    }
}
