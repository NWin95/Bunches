using UnityEngine;
using System.Collections;

public class WallRun : MonoBehaviour {

    public Vector2 wallJumpVel;
    public LayerMask rayMask;
    public Movement_Player moveScript;
    public CamScript camScript;
    Rigidbody rig;
    RaycastHit rayHit;
    bool wallRayed;
    Vector3 velDir;
    bool wallRun;
    Vector3 endPos;
    Vector3 moveVel;
    Vector2 moveRes;
    public Transform camTrans;
    Vector3 wallNormal;
    public Animator anim;
    Dash_Player dashScript;
    public Transform vis;
    public Transform forTrans;
    Vector3 animVel;
    Vector3 resVec;

    public float speed;

	void Start () {
        endPos = transform.position;
        rig = GetComponent<Rigidbody>();
        dashScript = GetComponent<Dash_Player>();
        //camTrans = Camera.main.transform;
	}
	
    void FixedUpdate ()
    {
        MoveVel();
        WallRay();
        Move();
        CheckVel();
        //WallRay();
    }

	void Update () {
        MoveVel();
        RayCheck();
        ForTrans();
        //CheckVel();
        Animation();
	}

    void ForTrans()
    {
        Vector3 lookVec = transform.forward;
        lookVec.y = 0;
        lookVec = lookVec.normalized;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        forTrans.rotation = lookRot;
    }

    void WallRay()
    {
        if (wallRun)
        {
            if (!Physics.Raycast(transform.position, -wallNormal, 3, rayMask))
            {
                WallExit();
            }
        }
    }

    void Animation ()
    {
        if (wallRun)
            anim.SetFloat("MoveResX", animVel.x);
    }

    public void WallJump ()
    {
        if (wallRun)
        {
            Vector3 vel = wallNormal * wallJumpVel.x;
            vel.y = wallJumpVel.y;
            rig.velocity = vel;
            anim.SetTrigger("JumpTrigger");
        }
    }

    void CheckVel()
    {
        Vector3 startPos = transform.position;
        animVel = (startPos - endPos) / Time.fixedDeltaTime;
        endPos = transform.position;

        animVel -= rig.velocity;
        animVel.y = 0;

        //Debug.Log(animVel);

        //animVel = forTrans.TransformDirection(animVel);
        //Debug.Log(animVel);
    }

    void MoveVel()
    {
        Vector3 startPos = transform.position;
        moveVel = (startPos - endPos) / Time.fixedDeltaTime;
        endPos = transform.position;

        moveVel -= rig.velocity;
        moveVel.y = 0;

        //moveVel = transform.TransformDirection(moveVel);
        //Debug.Log(moveVel.magnitude);
    }

    void RayCheck ()
    {
        if (!wallRun)
        {
            velDir = moveVel.normalized;
            Vector3 pos = transform.position;

            if (Physics.Raycast(pos, velDir, out rayHit, 3, rayMask))
            {
                wallNormal = rayHit.normal;
                wallRayed = true;
            }
            else
            {
                wallRayed = false;
            }
        }
    }

    void OnCollisionEnter (Collision coll)
    {
        //Debug.Log("Collided");

        Vector3 velDirB = velDir;
        velDirB.y = 0;
        velDirB = velDirB.normalized;

        //Debug.Log("WallRayed: " + wallRayed + "  " + "Grounded: " + moveScript.grounded);

        if (wallRayed && !moveScript.grounded)
        {
            Vector3 pos = transform.position + (Vector3.up * 1f);
            if (Physics.Raycast(pos, velDirB, 3, rayMask))
            {
                pos = transform.position - (Vector3.up * 1f);
                if (Physics.Raycast(pos, velDirB, 3, rayMask))
                {
                    if (coll.contacts[0].point.y < (transform.position.y + 0.5f))
                        WallAttach();
                }
                else
                {
                    StartCoroutine("WallPop");
                }
            }
            else
            {
                StartCoroutine("WallPop");
            }
        }

        if (!wallRayed && !moveScript.grounded)
        {
            Vector3 pos = transform.position - (Vector3.up * 0.63f);
            if (Physics.Raycast(pos, velDirB, 3, rayMask))
            {
                StartCoroutine("CraneUp");
            }
        }
    }

    void Move()
    {
        if (wallRun)
        {
            //Debug.Log("OnWall");
            /*
            Vector3 moveVec = camTrans.right;
            moveVec.y = 0;
            moveVec = forTrans.TransformDirection(moveVec);
            moveVec.x = 0;
            moveVec = -moveVec;
            moveVec *= 2;
            moveVec = Vector3.ClampMagnitude(moveVec, 1);

            Debug.DrawRay(transform.position, moveVec, Color.red);

            moveRes = moveScript.moveRes;

            Vector3 inputs = moveVec * moveRes.y;
            inputs = Vector3.ClampMagnitude(inputs, 1);

            Vector3 move = inputs * speed * Time.fixedDeltaTime;
            rig.MovePosition(move + rig.position);
            */

            Vector3 moveVec = camTrans.forward;
            moveVec.y = 0;
            float dot = Vector3.Dot(moveVec, transform.right);

            resVec = new Vector3(dot, 0, 0);
            resVec = transform.TransformDirection(resVec);
            resVec *= 2;
            resVec = Vector3.ClampMagnitude(resVec, 1);

            Debug.DrawRay(transform.position, resVec * 2, Color.yellow);

            moveRes = moveScript.moveRes;

            Vector3 inputs = resVec * moveRes.y;
            inputs = Vector3.ClampMagnitude(inputs, 1);

            Vector3 move = inputs * speed * Time.fixedDeltaTime;
            rig.MovePosition(move + rig.position);

            //moveVec = forTrans.InverseTransformDirection(moveVec);
            //moveVec = forTrans.TransformDirection(moveVec);

            //moveVec.z = moveVec.x;
            //moveVec.x = 0;

            //moveVec.x = -moveVec.z;
            //moveVec.z = 0;
        }
    }

    IEnumerator WallPop ()
    {
        moveScript.canMove = false;
        camScript.playerTurn = false;

        Vector3 lookVec = -wallNormal;
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;
        anim.SetTrigger("WallPopTrigger");

        rig.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.2f);

        Vector3 vel = Vector3.up * 6;
        vel += -transform.forward * 6;
        rig.velocity = vel;

        yield return new WaitForSeconds(0.2f);

        vel += transform.forward * 6.5f;
        rig.velocity = vel;

        yield return new WaitForSeconds(0.2f);

        moveScript.canMove = true;
        camScript.playerTurn = true;
    }

    IEnumerator CraneUp()
    {
        moveScript.canMove = false;
        camScript.playerTurn = false;

        Vector3 lookVec = -wallNormal + (Vector3.up * 0.001f);
        Quaternion lookRot = Quaternion.LookRotation(lookVec);
        transform.rotation = lookRot;
        anim.SetTrigger("CraneUpTrigger");

        rig.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.2f);

        Vector3 vel = Vector3.up * 5;
        vel += -transform.forward * 3;
        rig.velocity = vel;

        yield return new WaitForSeconds(0.2f);

        vel += transform.forward * 4;
        rig.velocity = vel;

        yield return new WaitForSeconds(0.2f);

        moveScript.canMove = true;
        camScript.playerTurn = true;
    }

    public void WallExit()
    {
        Debug.Log("Wall Exit");
        moveScript.canMove = true;
        camScript.playerTurn = true;

        wallRun = false;
        rig.useGravity = true;

        Vector3 vel = wallNormal * 0.125f;
        vel.y = 0.125f;
        rig.velocity = vel;

        wallNormal = Vector3.zero;

        anim.SetTrigger("WallExitTrigger");
    }

    void WallAttach ()
    {
        //Debug.Log("Should Attacg but Bool is Wrong");
        if (!wallRun)
        {
            //Debug.Log("Attach");
            wallNormal = rayHit.normal;
            moveScript.canMove = false;
            camScript.playerTurn = false;

            wallRun = true;
            rig.velocity = Vector3.zero;
            rig.useGravity = false;

            transform.position = rayHit.point + rayHit.normal;

            Vector3 lookVec = Vector3.up + (-wallNormal * 0.001f);
            Quaternion lookRot = Quaternion.LookRotation(lookVec);

            transform.rotation = lookRot;
            //vis.rotation = lookRot;

            anim.SetTrigger("WallRunTrigger");

            dashScript.StopCoroutine("Dash");
        }
    }
}
