using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdScript : MonoBehaviour {

    public float timeAllowence;

    public IEnumerator ShowAd(int reset)
    {
        //0  None, 1  Same Scene, 2  Menu Scene

        float timeA = Time.time;

        while (!Advertisement.IsReady())
        {
            float timeB = Time.time;
            if ((timeB - timeA) > timeAllowence)
            {
                Debug.Log("Took To Long");
                if (reset == 1)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                else if (reset == 2)
                    SceneManager.LoadScene(0);
                yield break;
            }
            Debug.Log(timeB - timeA);
            yield return null;
        }
        Advertisement.Show();

        if (reset == 1)
        {
            Debug.Log("load level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (reset == 2)
            SceneManager.LoadScene(0);
    }
}
