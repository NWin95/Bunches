using UnityEngine;
using System.Collections;

public class SettingsCanvas : MonoBehaviour {

    public UnityEngine.UI.Text invertYMark;

    void Start ()
    {
        if (PlayerPrefs.GetInt("InvY") == 0)
            invertYMark.text = "O";
        else if (PlayerPrefs.GetInt("InvY") == 1)
            invertYMark.text = "X";
    }

	public void InvertY ()
    {
        if (PlayerPrefs.GetInt("InvY") == 0)
        {
            PlayerPrefs.SetInt("InvY", 1);
            invertYMark.text = "X";
        }
        else if (PlayerPrefs.GetInt("InvY") == 1)
        {
            PlayerPrefs.SetInt("InvY", 0);
            invertYMark.text = "O";
        }
    }
}
