using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetColl : MonoBehaviour {

    public bool tutorial;

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

    void Start ()
    {
        timeA = Time.time;
        Transform playerCanvas = GameObject.Find("PlayerCanvas").transform;
        ekText = playerCanvas.GetChild(9).GetComponent<UnityEngine.UI.Text>();
        ekrText = playerCanvas.GetChild(10).GetComponent<UnityEngine.UI.Text>();

        highEKText = playerCanvas.GetChild(11).GetComponent<UnityEngine.UI.Text>();
        highRatText = playerCanvas.GetChild(12).GetComponent<UnityEngine.UI.Text>();

        highEK = PlayerPrefs.GetInt("Score");
        highEKRat = PlayerPrefs.GetFloat("Ratio");

        StartCoroutine(ShowHighScore());
    }

    void Update ()
    {
        timeB = Time.time - timeA;
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
