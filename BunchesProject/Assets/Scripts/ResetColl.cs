using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetColl : MonoBehaviour {

	void OnTriggerEnter (Collider coll)
    {
        if (coll.name == "Player")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
