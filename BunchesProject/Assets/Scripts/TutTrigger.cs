using UnityEngine;
using System.Collections;

public class TutTrigger : MonoBehaviour {

    public int stepInt;
    public GameObject tutObj;

    void OnTriggerEnter (Collider coll)
    {
        if (coll.tag == "Player")
            tutObj.GetComponent<TutorialScript>().RunStep(stepInt);
    }
}
