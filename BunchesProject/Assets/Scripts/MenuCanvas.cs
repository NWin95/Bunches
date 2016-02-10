using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour {

    public Vector2 screenSize;
    public UnityEngine.UI.Button[] buttonsA;
    public GameObject credits;
    bool creditsOpen;

	void Start () {
        screenSize = new Vector2(Screen.width, Screen.height);
	}

    public void levelButton (GameObject buttonObj)
    {
        SceneManager.LoadScene(buttonObj.name);
    }

    public void Credits ()
    {
        creditsOpen = !creditsOpen;
        credits.SetActive(!credits.activeSelf);
    }

    public void MusicMuteToggle ()
    {
        AudioSource source = GameObject.Find("MusicA").GetComponent<AudioSource>();
        source.mute = !source.mute;
    }
}
