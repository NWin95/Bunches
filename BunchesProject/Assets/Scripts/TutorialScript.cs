using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialScript : MonoBehaviour {

    public Vector3 checkPos;
    public Vector2 screenSize;
    public GameObject[] stepObjs;
    public GameObject tutCanvas;
    public GameObject playerCanvas;
    public UnityEngine.UI.Image continueImage;
    public GameObject enemy;
    public List<Vector3> spawnPos = new List<Vector3>();
    public List<GameObject> enemies = new List<GameObject>();

    void Start ()
    {
        tutCanvas.SetActive(true);
        screenSize = new Vector2(Screen.width, Screen.height);
        //continueImage.rectTransform.sizeDelta = new Vector2(screenSize.y * 0.333f, screenSize.y * 0.15f);
        GameObject.Find("ResetColl").GetComponent<ResetColl>().tutorial = true;
        RunStep(9);

        Destroy(GameObject.Find("Mute"));
    }

    void Update ()
    {
        MobileInput();
    }

    void OnTriggerEnter (Collider coll)
    {
        if (coll.tag == "Player")
        {
            Reset(coll);
        }
    }

    public void Reset(Collider coll)
    {
        coll.transform.position = checkPos;
        coll.GetComponent<Rigidbody>().velocity = Vector3.zero;

        foreach (GameObject enemyG in enemies)
        {
            Destroy(enemyG);
        }
        foreach (Vector3 pos in spawnPos)
        {
            GameObject instEnemy = Instantiate(enemy, pos, Quaternion.identity) as GameObject;
            enemies.Add(instEnemy);
        }
    }

    public void RunStep (int stepInt)
    {
        //string funcString = "Step" + stepInt;


        StartCoroutine(PlayerCanvas());
        Pause();
        //playerCanvas.SetActive(false);

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
        //playerCanvas.SetActive(false);
    }

    public void CloseMessage ()
    {
        foreach (GameObject obj in stepObjs)
        {
            obj.SetActive(false);
        }

        Unpause();
        //playerCanvas.SetActive(true);
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
