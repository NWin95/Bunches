using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour {

    public Vector2 screenSize;
    public UnityEngine.UI.Button[] buttonsA;

	void Start () {
        screenSize = new Vector2(Screen.width, Screen.height);
	}
	
	void Update () {
	
	}

    public void levelButton (GameObject buttonObj)
    {
        SceneManager.LoadScene(buttonObj.name);
    }
}
