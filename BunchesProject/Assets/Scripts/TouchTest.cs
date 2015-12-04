using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour {

    int moveTouchInt = 52;
    Vector2 screenSize;

    Vector2 moveTouchStart;
    Vector2 moveTouchEnd;

    void Start ()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        Debug.Log(screenSize);
    }

	void Update () {
	    if (Input.touchCount > 0)
        {
            foreach (Touch touchTemp in Input.touches)
            {
                if (touchTemp.phase == TouchPhase.Began)
                {
                    Vector2 touchPos = touchTemp.position;

                    if ((touchPos.x < screenSize.x * 0.25f) && (touchPos.y < screenSize.y * 0.333f))    //Bottom Left
                    {
                        moveTouchInt = touchTemp.fingerId;
                    }
                }
            }

            if (moveTouchInt != 52)
            {
                //Debug.Log(Input.GetTouch(moveTouchInt).phase);
                Debug.Log(Input.GetTouch(moveTouchInt).deltaPosition);
            }
        }
	}
}
