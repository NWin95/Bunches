using UnityEngine;
using System.Collections;

public class ZapballScript : MonoBehaviour {

    public float lifeTime;

	void Start () {
        StartCoroutine("LifeClock");
	}
	
	IEnumerator LifeClock ()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter (Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            StartCoroutine("HitPlayer", coll);
        }
    }

    IEnumerator HitPlayer (Collider col)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        ParticleSystem ps = GetComponent<ParticleSystem>();

        ps.startColor = Color.red;
        ps.startSpeed = 20;

        col.gameObject.BroadcastMessage("HitByZapball", SendMessageOptions.DontRequireReceiver);

        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
