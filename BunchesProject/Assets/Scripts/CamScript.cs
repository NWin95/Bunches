using UnityEngine;
using System.Collections;

public class CamScript : MonoBehaviour {

    public bool touchMode;
    public int mode;

    public bool playerTurn;
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

    Vector2 screenSize;
    public int turnTouchInt = 25;
    Vector2 turnStartPos;
    float turnSlideDiv;
    public UnityEngine.UI.Image turnImage;
    public UnityEngine.UI.Image turnImageDot;
    Vector2 turnRes;

    Vector3 inputAxes;
    public Vector2 ambientIntensityRange;
    public UnityEngine.UI.Slider lightSlider;
    public Light sun;
	
    void Start ()
    {
        if (PlayerPrefs.GetInt("InvY") == 0)
            invertY = false;
        else if (PlayerPrefs.GetInt("InvY") == 1)
            invertY = true;

        if (lightSlider != null)
        {
            //lightSlider.value = RenderSettings.ambientIntensity;
            lightSlider.minValue = ambientIntensityRange.x;
            lightSlider.maxValue = ambientIntensityRange.y;
            lightSlider.value = sun.intensity;
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        screenSize = new Vector2(Screen.width, Screen.height);

        if (touchMode)
        {
            //turnImage.rectTransform.sizeDelta = new Vector2(screenSize.y * 0.333f, screenSize.y * 0.333f);
            //turnImageDot.rectTransform.sizeDelta = new Vector2(screenSize.x * 0.25f / 4, screenSize.x * 0.25f / 4);
            turnSlideDiv = screenSize.y * 0.333f * 0.4f;
        }
        else
        {
            turnImage.gameObject.SetActive(false);
            turnImageDot.gameObject.SetActive(false);
        }

        camCam = camTrans.GetComponent<Camera>();
        player = transform.parent;
        holdFov = camCam.fieldOfView;
        transform.SetParent(null);
        lerpHold = lerpTime;
    }

	void Update () {
        if (touchMode)
            MobileInput();

        Turn();
        Mode();
        Lerp();
	}

    public void IntensityChange ()
    {
        //RenderSettings.ambientIntensity = lightSlider.value;
        sun.intensity = lightSlider.value;
    }

    void MobileInput()
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

                        if ((touchPos.x > screenSize.x * 0.75f) && (touchPos.y < screenSize.y * 0.333f))    //Bottom Right
                        {
                            turnTouchInt = touchTemp.fingerId;
                            turnStartPos = Input.GetTouch(turnTouchInt).position;

                            turnImageDot.rectTransform.position = Input.GetTouch(turnTouchInt).position;

                            turnImage.gameObject.SetActive(false);
                            turnImageDot.gameObject.SetActive(true);
                        }
                    }
                }
            }

            if (turnTouchInt != 25)
            {
                TurnTouch();
            }
        }
    }

    void TurnTouch ()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == turnTouchInt)
            {
                TouchPhase tPhase = touch.phase;
                if (tPhase == TouchPhase.Ended)
                {
                    turnTouchInt = 25;
                    turnStartPos = Vector2.zero;
                    turnRes = Vector2.zero;

                    turnImage.gameObject.SetActive(true);
                    turnImageDot.gameObject.SetActive(false);
                }

                if (tPhase == TouchPhase.Moved || tPhase == TouchPhase.Stationary)
                {
                    turnImageDot.rectTransform.position = touch.position;

                    Vector2 dif = (touch.position - turnStartPos);
                    float difMag = dif.magnitude;
                    difMag = Mathf.Clamp(difMag, 0, turnSlideDiv);

                    float div = difMag / turnSlideDiv;
                    turnRes = div * dif.normalized;
                }
            }
        }
    }

    void Turn ()
    {
        Vector3 locEuler = transform.localEulerAngles;
        Vector3 inputs;
        //  Yaw  Pitch   Roll

        if (touchMode)
            inputs = new Vector3(turnRes.x, -turnRes.y, 0);
        else
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

        if (playerTurn)
        {
            if (rotVec != Vector3.zero)
                player.forward = rotVec;
        }
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
