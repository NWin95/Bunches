using UnityEngine;
using System.Collections;

public class BreakableGlass : MonoBehaviour {

    public float breakAmount;
    public GameObject broken;

	void OnTriggerEnter (Collider coll)
    {
        if (coll.GetComponent<Rigidbody>())
        {
            Rigidbody rig = coll.GetComponent<Rigidbody>();
            float velMag = rig.velocity.magnitude;
            float amount = 0.5f * rig.mass * (velMag * velMag);

            if (amount > breakAmount)
            {
                GameObject spawnedGlass = GameObject.Instantiate(broken, transform.position, transform.rotation) as GameObject;
                spawnedGlass.transform.localScale = transform.localScale;
                spawnedGlass.transform.forward = transform.right;
                Destroy(gameObject);
            }
        }
    }
}
