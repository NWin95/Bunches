using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetColl : MonoBehaviour {

    public int enemiesKilled;

	void OnTriggerEnter (Collider coll)
    {
        if (coll.name == "Player")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (coll.tag == "Enemy")
        {
            StartCoroutine(EnemyKilled(coll.gameObject));
        }
    }

    IEnumerator EnemyKilled (GameObject obj)
    {
        enemiesKilled++;
        obj.transform.GetChild(0).GetComponent<Animator>().SetTrigger("FallTrigger");

        yield return new WaitForSeconds(2.5f);
        Destroy(obj);
    }
}
