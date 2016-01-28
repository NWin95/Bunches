using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour {

    public Vector2 screenSize;
    public GameObject[] stepObjs;
    public GameObject tutCanvas;
    public GameObject playerCanvas;
    public UnityEngine.UI.Image continueImage;

    void Start ()
    {
        tutCanvas.SetActive(true);
        screenSize = new Vector2(Screen.width, Screen.height);
        continueImage.rectTransform.sizeDelta = new Vector2(screenSize.y * 0.333f, screenSize.y * 0.15f);
        RunStep(1);
    }

    void Update ()
    {
        MobileInput();
    }

    public void RunStep (int stepInt)
    {
        string funcString = "Step" + stepInt;


        StartCoroutine(PlayerCanvas());
        Pause();
        playerCanvas.SetActive(false);

        stepObjs[stepInt - 1].SetActive(true);
        continueImage.gameObject.SetActive(true);

        //Invoke(funcString,0);
    }

    public void Pause ()
    {
        Time.timeScale = 0;
    }

    public void Unpause ()
    {
        Time.timeScale = 1;
    }

    IEnumerator PlayerCanvas ()
    {
        yield return new WaitForEndOfFrame();
        //transform.SetParent(playerCanvas.transform);
        transform.SetSiblingIndex(0);
        playerCanvas.SetActive(false);
    }

    public void CloseMessage ()
    {
        foreach (GameObject obj in stepObjs)
        {
            obj.SetActive(false);
        }

        Unpause();
        playerCanvas.SetActive(true);
        continueImage.gameObject.SetActive(false);
    }

    void MobileInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touchTemp in Input.touches)
            {
                Vector2 touchPos = touchTemp.position;
                if (touchTemp.phase == TouchPhase.Began)
                {
                    if (Time.timeScale == 0)
                    {
                        if ((touchPos.x > (screenSize.x * 0.333f) && touchPos.x < (screenSize.x * 0.666f) && touchPos.y > (screenSize.y * 0.85f)))
                            CloseMessage();
                    }
                }
            }
        }
    }
}
