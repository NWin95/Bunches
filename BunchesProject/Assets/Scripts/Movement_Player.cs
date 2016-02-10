using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Make Wall Run

public class Movement_Player : MonoBehaviour {

    public GameObject canvasObj;
    public UnityEngine.UI.Image moveImage;
    public UnityEngine.UI.Image moveImageDot;
    public UnityEngine.UI.Image jumpButton;

    public bool canMove;
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
    public int airSkill;

    Vector2 screenSize;
    public int moveTouchInt = 52;
    Vector2 moveStartPos;
    float moveSlideDiv;
    public Vector2 moveRes;
    public bool sliding;
    public CamScript camScript;
    public PhysicMaterial playerMat;
    public PhysicMaterial slideMat;
    public Transform resetColl;

	void Start () {
        anim.SetInteger("AirSkill", airSkill);
        screenSize = new Vector2(Screen.width, Screen.height);
        rig = GetComponent<Rigidbody>();
        endPos = transform.position;

        StartCoroutine(CanvasTop());

        moveSlideDiv = screenSize.y * 0.333f * 0.4f;
        resetColl = GameObject.Find("ResetColl").transform;
    }

    IEnumerator CanvasTop ()
    {
        yield return new WaitForEndOfFrame();

        canvasObj.transform.SetParent(null);
        canvasObj.SetActive(true);
        canvasObj.transform.SetAsFirstSibling();
    }
	
	void Update () {
        MobileInput();
        RayGround();
        GroundFunc();
        Sliding();
        Animation();
        ResetCollFunc();
	}

    void FixedUpdate ()
    {
        Move();
        SpeedCheck();
    }

    void ResetCollFunc ()
    {
        if (grounded)
            resetColl.position = transform.position;
    }

    void MobileInput ()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touchTemp in Input.touches)
            {
                if (touchTemp.phase == TouchPhase.Began)
                {
                    if (Time.timeScale != 0)
                    {
                        Vector2 touchPos = touchTemp.position;

                        if ((touchPos.x > (screenSize.x * 0.25f) && touchPos.x < (screenSize.x * 0.75f) && touchPos.y < (screenSize.y * 0.25f)))
                            MobileJump();

                        if ((touchPos.x < screenSize.x * 0.25f) && (touchPos.y < screenSize.y * 0.333f))    //Bottom Left
                        {
                            moveTouchInt = touchTemp.fingerId;
                            moveStartPos = Input.GetTouch(moveTouchInt).position;

                            moveImageDot.rectTransform.position = Input.GetTouch(moveTouchInt).position;

                            moveImage.gameObject.SetActive(false);
                            moveImageDot.gameObject.SetActive(true);
                        }
                    }
                }
            }

            if (moveTouchInt != 52)     
            {
                MoveTouch();
            }
        }
    }

    void MoveTouch ()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == moveTouchInt)
            {
                TouchPhase tPhase = touch.phase;
                if (tPhase == TouchPhase.Ended)
                {
                    moveTouchInt = 52;
                    moveStartPos = Vector2.zero;
                    moveRes = Vector2.zero;

                    moveImage.gameObject.SetActive(true);
                    moveImageDot.gameObject.SetActive(false);
                }

                if (tPhase == TouchPhase.Moved || tPhase == TouchPhase.Stationary)
                {
                    moveImageDot.rectTransform.position = touch.position;

                    Vector2 dif = (touch.position - moveStartPos);
                    float difMag = dif.magnitude;
                    difMag = Mathf.Clamp(difMag, 0, moveSlideDiv);

                    float div = difMag / moveSlideDiv;
                    moveRes = div * dif.normalized;
                }
            }
        }
    }

    void Animation ()
    {
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("Velocity", horVel);
        anim.SetBool("Slide", sliding);
    }

    void Sliding()
    {
        if (sliding)
        {
            Vector3 lookVec = rig.velocity;
            if (lookVec != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(lookVec);
                transform.rotation = lookRot;
            }
        }
    }

    public void MobileJump ()
    {
        if (grounded)   // used to have && canMove
        {
            float jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.magnitude) * jumpHeight) + 0.25f;

            Vector3 vel = rig.velocity;
            vel.y = jumpVel;
            rig.velocity = vel;

            anim.SetTrigger("JumpTrigger");
        }

        GetComponent<WallRun>().WallJump();
    }

    void Jump ()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            float jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.magnitude) * jumpHeight) + 0.25f;

            Vector3 vel = rig.velocity;
            vel.y = jumpVel;
            rig.velocity = vel;

            anim.SetTrigger("JumpTrigger");
        }
    }

    void GroundFunc ()
    {
        grounded = false;

        if (gRay || gColl)
            grounded = true;
    }

    void OnCollisionEnter (Collision coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Slide"))
        {
            StartCoroutine("StartSlide");
        }
    }

    IEnumerator StartSlide ()
    {
        canMove = false;
        GetComponent<CapsuleCollider>().material = slideMat;
        if (camScript != null)
            camScript.playerTurn = false;

        //yield return new WaitForSeconds(0.25f);
        yield return new WaitForEndOfFrame();
        sliding = true;
    }

    public void StopSlide ()
    {
        sliding = false;
        canMove = true;
        GetComponent<CapsuleCollider>().material = playerMat;
        if (camScript != null)
            camScript.playerTurn = true;
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

        if (sliding)
        {
            StopSlide();
        }
    }

    void RayGround ()
    {
        gRay = Physics.Raycast(transform.position, -transform.up, 1, groundMask);
    }

    void Move ()
    {
        if (canMove)
        {
            Vector3 inputs = new Vector3(moveRes.x * 0.5f, 0, moveRes.y);
            inputs = Vector3.ClampMagnitude(inputs, 1);
            inputs = transform.TransformDirection(inputs);

            Vector3 move = inputs * speed * Time.fixedDeltaTime;
            rig.MovePosition(move + rig.position);
        }
    }
    /*
    public void ResetLevel ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }   */

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
}
