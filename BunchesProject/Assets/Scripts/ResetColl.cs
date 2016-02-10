using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetColl : MonoBehaviour {

    public bool tutorial;
    //public Transform player;

    public int enemiesKilled;
    public UnityEngine.UI.Text ekText;
    public UnityEngine.UI.Text ekrText;

    public UnityEngine.UI.Text highEKText;
    public UnityEngine.UI.Text highRatText;

    public int highEK;
    public float highEKRat;

    float timeA;
    float timeB;

    float ektRatio;
    public AdScript ad;
    public float lifeAdTime;

    void Start ()
    {
        timeA = Time.time;
        //player = GameObject.Find("Player").transform;

        //Debug.Log(GameObject.Find("PlayerCanvas"));

        StartCoroutine(CanvasStart());
        StartCoroutine(ShowHighScore());
    }

    void Update ()
    {
        timeB = Time.time - timeA;
        //transform.position = player.position;
    }

    IEnumerator CanvasStart()
    {
        yield return new WaitForEndOfFrame();

        GameObject pCanvasObj = GameObject.Find("PlayerCanvas");  // might be problem if scores don't show
        //Debug.Log(pCanvasObj);

        if (pCanvasObj != null)
        {
            Transform playerCanvas = pCanvasObj.transform;
            ekText = playerCanvas.GetChild(9).GetComponent<UnityEngine.UI.Text>();
            ekrText = playerCanvas.GetChild(10).GetComponent<UnityEngine.UI.Text>();

            highEKText = playerCanvas.GetChild(11).GetComponent<UnityEngine.UI.Text>();
            highRatText = playerCanvas.GetChild(12).GetComponent<UnityEngine.UI.Text>();

            highEK = PlayerPrefs.GetInt("Score");
            highEKRat = PlayerPrefs.GetFloat("Ratio");
        }
    }

    IEnumerator ShowHighScore ()
    {
        yield return new WaitForEndOfFrame();

        if (tutorial)
        {
            Destroy(ekText);
            Destroy(ekrText);
            Destroy(highEKText);
            Destroy(highRatText);
        }
        else
        {
            yield return new WaitForSeconds(3);

            highEKText.text = "" + highEK;
            highRatText.text = "" + highEKRat;
        }
    }

	void OnTriggerEnter (Collider coll)
    {
        if (coll.name == "Player")
        {
            if (ad != null)
            {
                if (timeB > lifeAdTime)
                {
                    StartCoroutine(ad.ShowAd(1));
                }
                else
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (coll.tag == "Enemy")
        {
            StartCoroutine(EnemyKilled(coll.gameObject));
        }
    }

    IEnumerator EnemyKilled (GameObject obj)
    {
        enemiesKilled++;

        if (!tutorial)
        {
            ekText.text = "" + enemiesKilled;

            ektRatio = enemiesKilled / timeB;
            ektRatio = Mathf.Round(ektRatio * 100) / 100;

            ekrText.text = "" + ektRatio;

            if (enemiesKilled > highEK)
            {
                PlayerPrefs.SetInt("Score", enemiesKilled);
                PlayerPrefs.SetFloat("Ratio", ektRatio);
            }
        }

        obj.transform.GetChild(0).GetComponent<Animator>().SetTrigger("FallTrigger");

        yield return new WaitForSeconds(2.5f);
        Destroy(obj);
    }
}
