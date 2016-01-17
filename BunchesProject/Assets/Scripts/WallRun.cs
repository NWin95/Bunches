using UnityEngine;
using System.Collections;

public class WallRun : MonoBehaviour {

    public Vector2 wallJumpVel;
    public LayerMask rayMask;
    public Movement_Player moveScript;
    public CamScript camScript;
    Rigidbody rig;
    RaycastHit rayHit;
    Vector3 normal;
    bool wallRayed;
    Vector3 velDir;
    bool wallRun;
    Vector3 endPos;
    Vector3 moveVel;
    Vector2 moveRes;
    Transform camTrans;
    Vector3 wallNormal;
    public Animator anim;
    Dash_Player dashScript;

    public float speed;

	void Start () {
        endPos = transform.position;
        rig = GetComponent<Rigidbody>();
        dashScript = GetComponent<Dash_Player>();
        camTrans = Camera.main.transform;
	}
	
    void FixedUpdate ()
    {
        MoveVel();
        Move();
        WallRay();
    }

	void Update () {
        //MoveVel();
        RayCheck();
        Animation();
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
            anim.SetFloat("MoveResX", moveRes.x);
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

    void MoveVel()
    {
        Vector3 startPos = transform.position;
        moveVel = (startPos - endPos) / Time.fixedDeltaTime;
        endPos = transform.position;

        moveVel -= rig.velocity;
        moveVel.y = 0;

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
                normal = rayHit.normal;
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
        Vector3 velDirB = velDir;
        velDirB.y = 0;
        velDirB = velDirB.normalized;

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
    }

    void Move()
    {
        if (wallRun)
        {
            moveRes = moveScript.moveRes;
            moveRes.x *= 2;
            Vector3 inputs = new Vector3(moveRes.x * 0.5f, 0, moveRes.y);
            inputs = Vector3.ClampMagnitude(inputs, 1);
            inputs = transform.TransformDirection(inputs);

            inputs.y = 0;
            Vector3 move = inputs * speed * Time.fixedDeltaTime;
            rig.MovePosition(move + rig.position);
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

    public void WallExit()
    {
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
        if (!wallRun)
        {
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

            anim.SetTrigger("WallRunTrigger");

            dashScript.StopCoroutine("Dash");
        }
    }
}
