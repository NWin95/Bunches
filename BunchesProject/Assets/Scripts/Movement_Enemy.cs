using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement_Enemy : MonoBehaviour {

    public Animator anim;
    public int mode;    //0 = Point     1 = Patrol      2 = Sentry      3 = Stationary

    public int airskill;

    Rigidbody rig;
    public float speed;
    float horVel;
    Vector3 endPos;
    public LayerMask groundMask;
    bool gRay;
    bool gColl;

    public List<Vector3> waypoints = new List<Vector3>();
    int waypointInt;
    public float waypointAllowence;
    public bool grounded;
    public float maxAngle;
    public bool shouldLook;
    bool groundedHold;
    public bool canMove;

    void Start ()
    {
        anim.SetInteger("AirSkill", airskill);
        rig = GetComponent<Rigidbody>();
        endPos = transform.position;

        if (mode == 0 || mode == 1)
            LookWaypoint();

        groundedHold = grounded;
    }

	void Update () {
        RayGround();
        GroundFunc();
        Animation();
	}

    void FixedUpdate ()
    {
        if (mode == 0 || mode == 1)
            WaypointFollow();
        SpeedCheck();
    }

    void RayGround()
    {
        gRay = Physics.Raycast(transform.position, -transform.up, 1, groundMask);
    }

    void GroundFunc()
    {
        grounded = false;

        if (gRay || gColl)
            grounded = true;

        if (grounded && !groundedHold)
            GetComponent<Attack_Enemy>().GroundStart();
        if (!grounded && groundedHold)
            GetComponent<Attack_Enemy>().GroundExit();

        groundedHold = grounded;
    }

    void Animation ()
    {
        anim.SetFloat("Velocity", horVel);
        anim.SetBool("Grounded", grounded);
    }

    void OnCollisionStay(Collision coll)
    {
        foreach (ContactPoint cp in coll)
        {
            if (Vector3.Angle(cp.normal, transform.up) < maxAngle)
                gColl = true;
        }

        //Debug.Log(gColl);
    }

    void OnCollisionExit()
    {
        gColl = false;
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.tag == "WaypointGiver")
        {
            if (waypoints.Count == 0)
            {
                waypoints = col.GetComponent<WaypointGiver>().waypoints;
                mode = col.GetComponent<WaypointGiver>().mode;
                airskill = col.GetComponent<WaypointGiver>().airSkill;
                anim.SetInteger("AirSkill", airskill);
            }
        }
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
        if (canMove)
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
    }

    void LookWaypoint ()
    {
        if (shouldLook)
        {
            Vector3 lookVec = waypoints[waypointInt] - transform.position;
            lookVec.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(lookVec);
            transform.rotation = lookRot;
        }
    }

    void NextWaypoint ()
    {
        waypointInt++;
        LookWaypoint();
    }

    void EndWaypoint ()
    {
        if (mode == 0)
            mode = -1;
        if (mode == 1)
            waypointInt = 0;
    }
}
