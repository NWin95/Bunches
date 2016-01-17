using UnityEngine;
using System.Collections;

public class Attack_Enemy : MonoBehaviour {

    public bool inRange;
    public Transform player;
    public LayerMask aimMask;
    public Animator anim;
    public float throwAnimTimeA;
    bool throwing;
    public bool canThrow;
    bool grounded;
    public GameObject zapBall;
    public float throwSpeed;

    void Update ()
    {
        RangeCheck();
    }

    public void GroundStart ()
    {
        grounded = true;
        //Debug.Log("HitGround");
    }

    public void GroundExit ()
    {
        grounded = false;
        //Debug.Log("LeftGround");
    }

    void RangeCheck ()
    {
        if (inRange)
        {
            if (!Physics.Linecast(transform.position, player.position, aimMask))
            {
                if (!throwing && canThrow && grounded)
                    StartCoroutine("ThrowTimeCount");
            }
            else
            {
                if (throwing)
                {
                    StopCoroutine("ThrowTimeCount");
                    throwing = false;
                    GetComponent<Movement_Enemy>().shouldLook = false;
                }
            }

            if (throwing)
            {
                LookAtPlayer();
            }
        }
    }

    IEnumerator ThrowTimeCount ()
    {
        throwing = true;
        Movement_Enemy me = GetComponent<Movement_Enemy>();
        me.shouldLook = false;
        me.canMove = false;

        anim.SetTrigger("ThrowTrigger");

        yield return new WaitForSeconds(throwAnimTimeA);
        Throw();

        yield return new WaitForSeconds(0.75f);
        me.shouldLook = true;
        yield return new WaitForSeconds(0.25f);
        me.canMove = true;
    }

    void Kicked ()
    {
        //Debug.Log("Ouch");

        anim.SetTrigger("KickedTrigger");
        StopCoroutine("ThrowTimeCount");
        throwing = false;
        GetComponent<Movement_Enemy>().shouldLook = false;
        canThrow = false;
    }

    void Throw ()
    {
        //Debug.Log("Throw");

        Vector3 pos = transform.position + (Vector3.up * 0.25f) + (transform.forward * 0.5f);
        GameObject thrownBall = GameObject.Instantiate(zapBall, pos, transform.rotation) as GameObject;

        Vector3 dir = (player.position - transform.position).normalized;
        Vector3 vel = dir * throwSpeed;
        thrownBall.GetComponent<Rigidbody>().velocity = vel;
    }

    void LookAtPlayer ()
    {
        Vector3 lookVec = player.position - transform.position;
        lookVec.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;
    }
}
