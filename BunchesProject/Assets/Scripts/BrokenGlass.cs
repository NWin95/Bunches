using UnityEngine;
using System.Collections;

public class BrokenGlass : MonoBehaviour {

    public float time;

    void Start ()
    {
        StartCoroutine(LifeTime());
    }

    IEnumerator LifeTime ()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
