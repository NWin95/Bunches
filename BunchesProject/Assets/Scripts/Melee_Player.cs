using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Melee_Player : MonoBehaviour {

    public Animator anim;
    public CamScript camScript;
    public Movement_Player moveScript;
    public Transform vis;
    public Vector3 leanPosOffset;
    public Vector3 leanRotOffset;
    public float leanDeadZone;
    public int leanInt;
    float lean;
    public LayerMask fightMask;
    public float range;
    Vector2 screenSize;
    public int leftTouchInt  = -1;
    public int rightTouchInt = -1;
    Vector2 leftStartPos;
    Vector2 rightStartPos;
    public float swipeDisRat;
    float swipeDis;
    public float swipeSpeed;
    public int attackInt;
    public Transform targ;
    public float angleAllowence;
    public Transform hitColl;

    void Start () {
        screenSize = new Vector2(Screen.width, Screen.height);
        swipeDis = screenSize.y * swipeDisRat;
        //Debug.Log(swipeDis);
    }
	
	void Update () {
        MobileInput();
        Dodge();
        StrikeTouch();
	}

    void SphereCheck ()
    {
        Collider[] colliders;
        colliders = Physics.OverlapSphere(transform.position, range, fightMask);

        float dis = range + 1;
        foreach (Collider col in colliders)
        {
            Vector3 dif = col.transform.position - transform.position;
            if (Vector3.Angle(dif, transform.forward) < angleAllowence)
            {
                float disB = dif.magnitude;
                if (disB < dis)
                {
                    targ = col.transform;
                    dis = disB;
                }
            }
        }
    }

    void MobileInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touchTemp in Input.touches)
            {
                if (touchTemp.phase == TouchPhase.Began)
                {
                    Vector2 touchPos = touchTemp.position;

                    if ((touchPos.x < screenSize.x * 0.5f) && (touchPos.y > screenSize.y * 0.333f))    //LeftSide
                    {
                        leftTouchInt = touchTemp.fingerId;
                        leftStartPos = touchTemp.position;
                    }
                    if ((touchPos.x > screenSize.x * 0.5f) && (touchPos.y > screenSize.y * 0.333f))    //RightSide
                    {
                        rightTouchInt = touchTemp.fingerId;
                        rightStartPos = touchTemp.position;
                    }
                }
            }

            if (leftTouchInt != -1 || rightTouchInt != -1)
            {
                StrikeTouch();
            }
        }
    }

    void StrikeTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == leftTouchInt)
            {
                TouchPhase tPhase = touch.phase;
                if (tPhase == TouchPhase.Ended)
                {
                    leftTouchInt = -1;
                }
                else
                {
                    Vector2 dif = touch.position - leftStartPos;
                    float speed = touch.deltaPosition.magnitude / touch.deltaTime;
                    //Debug.Log(speed);

                    if (dif.magnitude > swipeDis && speed > swipeSpeed && attackInt == 0)
                    {
                        if (dif.x > 0 || dif.y > 0)
                        {
                            if (Mathf.Abs(dif.x) > Mathf.Abs(dif.y))
                                LeftHor();
                            else
                                LeftVert();

                            StopCoroutine("Strike");
                            StartCoroutine("Strike");
                            leftTouchInt = -1;
                        }
                    }

                }
            }
            if (touch.fingerId == rightTouchInt)
            {
                TouchPhase tPhase = touch.phase;
                if (tPhase == TouchPhase.Ended)
                {
                    rightTouchInt = -1;
                }
                else
                {
                    Vector2 dif = touch.position - rightStartPos;
                    float speed = touch.deltaPosition.magnitude / touch.deltaTime;
                    //Debug.Log(speed);

                    if (dif.magnitude > swipeDis && speed > swipeSpeed && attackInt == 0)
                    {
                        if (dif.x < 0 || dif.y > 0)
                        {
                            if (Mathf.Abs(dif.x) > Mathf.Abs(dif.y))
                                RightHor();
                            else
                                RightVert();

                            StopCoroutine("Strike");
                            StartCoroutine("Strike");
                            rightTouchInt = -1;
                        }
                    }
                }
            }
        }
    }
    
    void LookTarg ()
    {
        if (targ != null)
        {
            Vector3 dif = targ.position - transform.position;
            dif.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(dif);
            transform.rotation = lookRot;

            camScript.playerTurn = false;
            moveScript.canMove = false;
        }
    }

    IEnumerator Strike ()
    {
        SphereCheck();
        LookTarg();

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds((anim.GetCurrentAnimatorStateInfo(0).length * 0.75f) - Time.deltaTime);

        attackInt = 0;
        anim.SetInteger("AttackInt", attackInt);
        targ = null;

        camScript.playerTurn = true;
        moveScript.canMove = true;
    }

    void LeftVert ()
    {
        attackInt = 1;
        anim.SetTrigger("StrikeTrigger");
        anim.SetInteger("AttackInt", attackInt);
    }

    void LeftHor ()
    {
        attackInt = 3;
        anim.SetTrigger("StrikeTrigger");
        anim.SetInteger("AttackInt", attackInt);
    }

    void RightVert ()
    {
        attackInt = 2;
        anim.SetTrigger("StrikeTrigger");
        anim.SetInteger("AttackInt", attackInt);
    }

    void RightHor ()
    {
        attackInt = 4;
        anim.SetTrigger("StrikeTrigger");
        anim.SetInteger("AttackInt", attackInt);
    }

    void Dodge ()
    {
        if (attackInt == 0)
        {
            lean = Input.acceleration.x;
            if (lean < -leanDeadZone)       //Left
            {
                LeanLeft();
            }
            else if (lean > leanDeadZone)       //Right
            {
                LeanRight();
            }
            else
            {
                LeanNone();
            }
        }
        else
        {
            LeanNone();
        }
    }

    void LeanLeft ()
    {
        if (leanInt != -1)
        {
            leanInt = -1;
            Vector3 offset = leanPosOffset;
            offset.x = -leanPosOffset.x;
            vis.localPosition = offset;

            Vector3 rotSet = leanRotOffset;
            rotSet.z = -leanRotOffset.z;
            vis.localEulerAngles = rotSet;

            Vector3 offsetB = offset;
            offsetB.x *= 1.25f;
            offsetB.y = 0;
            offsetB.z = 0;
            hitColl.localPosition = offsetB;
        }
    }

    void LeanRight ()
    {
        if (leanInt != 1)
        {
            leanInt = 1;
            Vector3 offset = leanPosOffset;
            offset.x = leanPosOffset.x;
            vis.localPosition = offset;

            Vector3 rotSet = leanRotOffset;
            rotSet.y = -leanRotOffset.y;
            vis.localEulerAngles = rotSet;

            Vector3 offsetB = offset;
            offsetB.x *= 1.25f;
            offsetB.y = 0;
            offsetB.z = 0;
            hitColl.localPosition = offsetB;
        }
    }

    void LeanNone ()
    {
        if (leanInt != 0)
        {
            leanInt = 0;
            Vector3 offset = new Vector3(0, -0.9f, 0);
            vis.localPosition = offset;

            Vector3 rotSet = Vector3.zero;
            vis.localEulerAngles = rotSet;

            hitColl.localPosition = Vector3.zero;
        }
    }
}
