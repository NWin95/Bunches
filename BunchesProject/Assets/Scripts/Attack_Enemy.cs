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

    void Update ()
    {
        RangeCheck();
    }

    void RangeCheck ()
    {
        if (inRange)
        {
            if (!Physics.Linecast(transform.position, player.position, aimMask))
            {
                if (!throwing && canThrow)
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
        GetComponent<Movement_Enemy>().shouldLook = false;

        anim.SetTrigger("ThrowTrigger");
        yield return new WaitForSeconds(throwAnimTimeA);
        Throw();
    }

    void Kicked ()
    {
        Debug.Log("Ouch");

        anim.SetTrigger("KickedTrigger");
        StopCoroutine("ThrowTimeCount");
        throwing = false;
        GetComponent<Movement_Enemy>().shouldLook = false;
        canThrow = false;
    }

    void Throw ()
    {
        Debug.Log("Throw");
    }

    void LookAtPlayer ()
    {
        Vector3 lookVec = player.position - transform.position;
        lookVec.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;
    }
}
