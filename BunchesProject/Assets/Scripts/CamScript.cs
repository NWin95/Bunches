using UnityEngine;
using System.Collections;

public class CamScript : MonoBehaviour {

    public int mode;

    public bool invertY;
    public Transform head;
    public Transform camTrans;
    Transform player;
    Camera camCam;
    public Vector2 sensitivity;
    public float lerpTime;
    float lerpHold;

    float holdFov;

    public Vector3[] parTargPos;
    public Vector3[] targPos;
    public Vector3[] targRot;

    Vector3 inputAxes;
	
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        camCam = camTrans.GetComponent<Camera>();
        player = transform.parent;
        holdFov = camCam.fieldOfView;
        transform.SetParent(null);
        lerpHold = lerpTime;
    }

	void Update () {
        Turn();
        Mode();
        Lerp();
	}

    void Turn ()
    {
        Vector3 locEuler = transform.localEulerAngles;
        Vector3 inputs;
        //  Yaw  Pitch   Roll
        inputs = new Vector3(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);

        locEuler.y += inputs.x * sensitivity.x * Time.deltaTime;

        if (!invertY)
            locEuler.x += inputs.y * sensitivity.y * Time.deltaTime;
        else
            locEuler.x += -inputs.y * sensitivity.y * Time.deltaTime;
        locEuler.z = 0;

        transform.localEulerAngles = locEuler;

        Vector3 rotVec = transform.forward;
        rotVec.y = 0;

        if (rotVec != Vector3.zero)
            player.forward = rotVec;
    }

    void Mode ()
    {
        if (Input.GetButtonDown("ModeNext"))
        {
            if (mode + 1 == targPos.Length)
                mode = 0;
            else
                mode++;

            Vector3 euler = transform.localEulerAngles;
            euler.x = 0;
            transform.localEulerAngles = euler;

            if (mode == 2)
            {
                //transform.SetParent(head);
                camCam.fieldOfView = holdFov * 1.5f;
                lerpTime = 0;
            }
            else
            {
                //transform.SetParent(null);
                camCam.fieldOfView = holdFov;
                lerpTime = lerpHold;
            }
        }
    }

    void Lerp ()
    {
        Vector3 curTargPos = Vector3.zero;
        if (mode ==2)
            curTargPos = head.position + head.TransformDirection(parTargPos[mode]);
        else
            curTargPos = player.position + parTargPos[mode];

        transform.position = curTargPos;

        //Vector3 parTargLerp = Vector3.Lerp(transform.position, curTargPos, Time.deltaTime / lerpTime);
        //transform.position = parTargLerp;

        Vector3 targPosLerp = Vector3.Lerp(camTrans.localPosition, targPos[mode], Time.deltaTime / lerpTime);
        Vector3 targRotLerp = Vector3.Slerp(camTrans.localEulerAngles, targRot[mode], Time.deltaTime / lerpTime);

        camTrans.localPosition = targPosLerp;
        camTrans.localEulerAngles = targRotLerp;
    }
}
