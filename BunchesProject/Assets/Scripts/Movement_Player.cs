using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement_Player : MonoBehaviour {

    public GameObject canvasObj;
    public UnityEngine.UI.Image moveImage;
    public UnityEngine.UI.Image moveImageDot;
    public UnityEngine.UI.Image jumpButton;

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

    Vector2 screenSize;
    public int moveTouchInt = 52;
    Vector2 moveStartPos;
    float moveSlideDiv;
    Vector2 moveRes;

	void Start () {
        screenSize = new Vector2(Screen.width, Screen.height);
        rig = GetComponent<Rigidbody>();
        endPos = transform.position;

        canvasObj.transform.SetParent(null);
        canvasObj.SetActive(true);

        moveImage.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.25f, screenSize.x * 0.25f);
        moveImageDot.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.25f /4, screenSize.x * 0.25f / 4);
        moveSlideDiv = screenSize.y * 0.333f * 0.4f;

        jumpButton.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.5f, screenSize.y * 0.25f);
    }
	
	void Update () {
        MobileInput();
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

    void MobileInput ()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touchTemp in Input.touches)
            {
                if (touchTemp.phase == TouchPhase.Began)
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
    }

    public void MobileJump ()
    {
        if (grounded)
        {
            float jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.magnitude) * jumpHeight) + 0.25f;

            Vector3 vel = rig.velocity;
            vel.y = jumpVel;
            rig.velocity = vel;
        }
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
        //Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 inputs = new Vector3(moveRes.x, 0, moveRes.y);
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
        //Debug.Log(horVel);
    }
}
