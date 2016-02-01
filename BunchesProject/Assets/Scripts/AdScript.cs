using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdScript : MonoBehaviour {

    public IEnumerator ShowAd(int reset)
    {
        //0  None, 1  Same Scene, 2  Menu Scene

        while (!Advertisement.IsReady())
        {
            yield return null;
        }
        Advertisement.Show();

        if (reset == 1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else if (reset == 2)
            SceneManager.LoadScene(0);
    }
}
