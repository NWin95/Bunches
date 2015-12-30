using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour {

    ParticleSystem ps;
    public Vector2 scaleRange;
    float scale;
    float scale2;
	
	void Start () {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();

        RandSize();
        ps.Play();
        //StartCoroutine(SetTime());
	}

    void RandSize ()
    {
        scale = Random.Range(scaleRange.x, scaleRange.y);
        scale2 = Random.Range(scale * 0.75f, scale * 1.333f);

        Vector3 scaleInit = transform.localScale;
        scaleInit *= scale;
        transform.localScale = scaleInit;

        ps.startSize = ps.startSize * scale2;
    }

    IEnumerator SetTime()
    {
        Time.timeScale = 100;
        yield return new WaitForSeconds(100010);
        Debug.Log("times up");
        Time.timeScale = 1;
    }
}
