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

    public float speed;

	void Start () {
        endPos = transform.position;
        rig = GetComponent<Rigidbody>();
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
                wallRayed = true;
            }
            else
            {
                wallRayed = false;
            }
        }
    }

    void OnCollisionEnter ()
    {
        Vector3 velDirB = velDir;
        velDirB.y = 0;
        velDirB = velDirB.normalized;

        if (wallRayed && !moveScript.grounded)
        {
            Vector3 pos = transform.position + (Vector3.up * 0.5f);
            if (Physics.Raycast(pos, velDirB, 3, rayMask))
            {
                pos = transform.position - (Vector3.up * 0.5f);
                if (Physics.Raycast(pos, velDirB, 3, rayMask))
                {
                    WallAttach();
                }
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

    void WallExit()
    {
        moveScript.canMove = true;
        camScript.playerTurn = true;

        wallRun = false;
        rig.useGravity = true;

        Vector3 vel = wallNormal * 0.125f;
        vel.y = 0.125f;
        rig.velocity = vel;

        wallNormal = Vector3.zero;
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
        }
    }
}
