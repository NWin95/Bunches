using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndBox : MonoBehaviour {

	void OnTriggerEnter (Collider collider)
    {
        if (collider.gameObject.tag == "Player")
            SceneManager.LoadScene("Menu");
    }
}
