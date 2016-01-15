using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour {

    public Vector2 screenSize;
    public UnityEngine.UI.Button[] buttonsA;

	void Start () {
        screenSize = new Vector2(Screen.width, Screen.height);
        Debug.Log(screenSize);

        Vector2 bASize = new Vector2(screenSize.x * 0.15f, screenSize.y * 0.05f);
        float bAPosX = screenSize.x * 0.0175f;

        foreach (UnityEngine.UI.Button button in buttonsA)
        {
            button.GetComponent<RectTransform>().sizeDelta = bASize;
            Vector2 pos = button.GetComponent<RectTransform>().anchoredPosition;
            pos.x = bAPosX;
            button.GetComponent<RectTransform>().anchoredPosition = pos;
        }
	}
	
	void Update () {
	
	}

    public void levelButton (GameObject buttonObj)
    {
        SceneManager.LoadScene(buttonObj.name);
    }
}
