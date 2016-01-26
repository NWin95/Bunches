using UnityEngine;
using System.Collections;

//Adjust Size of Enemy Collider by Distance

public class Dash_Player : MonoBehaviour {

    Vector2 screenSize;
    int dashTouchInt;
    public UnityEngine.UI.Image dashTouchImage;
    Camera camCam;
    public LayerMask dashMask;
    public float range;
    RaycastHit rayHit;
    public float dashDis;
    public Animator anim;
    Rigidbody rig;
    public Vector2 dashBack;
    public Vector2 kickVel;
    public CamScript camScript;
    //public Movement_Player moveScript;

    void Start () {
        rig = GetComponent<Rigidbody>();
        screenSize = new Vector2(Screen.width, Screen.height);
        dashTouchImage.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.25f / 2, screenSize.x * 0.25f / 2);
        camCam = Camera.main;
    }
	
	void Update () {
        MobileInput();
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

                    if (touchPos.y > screenSize.y * 0.333f)   //AboveJoysticks
                    {
                        dashTouchInt = touchTemp.fingerId;

                        dashTouchImage.rectTransform.position = Input.GetTouch(dashTouchInt).position;
                        dashTouchImage.gameObject.SetActive(true);
                    }
                }
            }

            if (dashTouchInt != -1)
            {
                DashTouch();
            }
        }
    }

    void DashTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == dashTouchInt)
            {
                TouchPhase tPhase = touch.phase;

                if (tPhase == TouchPhase.Began)
                {
                    Vector3 pos = touch.position;
                    Ray ray = camCam.ScreenPointToRay(pos);
                    if (Physics.Raycast(ray, out rayHit, range, dashMask))
                    {
                        GameObject hitObj = rayHit.collider.gameObject;
                        if (hitObj.layer == LayerMask.NameToLayer("EnemyDashCollider"))
                        {
                            StartCoroutine ("Dash",(hitObj));
                        }
                    }
                }

                if (tPhase == TouchPhase.Ended)
                {
                    dashTouchImage.gameObject.SetActive(false);
                    dashTouchInt = -1;
                }
            }
        }
    }

    IEnumerator Dash(GameObject targObj)
    {
        GetComponent<Movement_Player>().StopSlide();

        Transform targTrans = targObj.transform.parent;
        targTrans.BroadcastMessage("Kicked", SendMessageOptions.DontRequireReceiver);
        //Debug.Log("Kick");

        Vector3 dif = targTrans.position - transform.position;
        Vector3 pos = targTrans.position + (-dif.normalized * dashDis) + (targTrans.up * 0.75f);
        transform.position = pos;

        anim.SetTrigger("DashTrigger");

        Vector3 difB = dif;
        difB.y = 0;
        Vector3 vel = -difB.normalized * dashBack.x;
        vel.y = dashBack.y;

        Quaternion lookRot = Quaternion.LookRotation(difB);
        transform.rotation = lookRot;
        camScript.playerTurn = false;
        targTrans.GetComponent<Movement_Enemy>().mode = -1;

        rig.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.2f);

        rig.velocity = vel;
        Vector3 velB = difB.normalized * kickVel.x;
        velB.y = kickVel.y;
        targTrans.GetComponent<Rigidbody>().velocity = velB;

        Quaternion lookRotB = Quaternion.LookRotation(-difB);
        targTrans.rotation = lookRotB;

        yield return new WaitForSeconds(0.25f);

        camScript.playerTurn = true;
    }
}
