using UnityEngine;
using System.Collections;

public class Movement_Player : MonoBehaviour {

    public float speed;
    Vector3 endPos;
    Rigidbody rig;
    public LayerMask groundMask;
    public bool gRay;
    public bool gColl;
    public float maxAngle;
    public float jumpHeight;
    public bool grounded;
    public Animator anim;
    float horVel;

	void Start () {
        rig = GetComponent<Rigidbody>();
        endPos = transform.position;
	}
	
	void Update () {
        RayGround();
        GroundFunc();
        Jump();
        Animation();
	}

    void FixedUpdate ()
    {
        Move();
        SpeedCheck();
    }

    void Animation ()
    {
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("Velocity", horVel);
    }

    void Jump ()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            float jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.magnitude) * jumpHeight) + 0.25f;

            Vector3 vel = rig.velocity;
            vel.y = jumpVel;
            rig.velocity = vel;
        }
    }

    void GroundFunc ()
    {
        grounded = false;

        if (gRay || gColl)
            grounded = true;
    }

    void OnCollisionStay (Collision coll)
    {
        foreach (ContactPoint cp in coll)
        {
            if (Vector3.Angle(cp.normal, transform.up) < maxAngle)
                gColl = true;
        }

        //Debug.Log(gColl);
    }

    void OnCollisionExit ()
    {
        gColl = false;
    }

    void RayGround ()
    {
        gRay = Physics.Raycast(transform.position, -transform.up, 1, groundMask);
    }

    void Move ()
    {
        Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inputs = Vector3.ClampMagnitude(inputs, 1);
        inputs = transform.TransformDirection(inputs);

        Vector3 move = inputs * speed * Time.fixedDeltaTime;
        rig.MovePosition(move + rig.position);
    }

    void SpeedCheck ()
    {
        Vector3 startPos = transform.position;
        Vector3 difVec = startPos - endPos;
        endPos = transform.position;

        difVec -= rig.velocity;
        difVec.y = 0;
        horVel = difVec.magnitude / Time.fixedDeltaTime;
    }
}
