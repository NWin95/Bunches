using UnityEngine;
using System.Collections;

public class EnemyJumpPoint : MonoBehaviour {

    public float jumpHeight;

	void Start () {
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;
        bc.size = new Vector3(0.5f, 0.5f, 0.5f);
        GetComponent<Renderer>().enabled = false;
	}
	
    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            Jump(col);
    }

    void Jump (Collider col)
    {
        Rigidbody rig = col.gameObject.GetComponent<Rigidbody>();

        Vector3 vel = rig.velocity;
        vel.y = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.magnitude) * jumpHeight) + 0.25f;
        rig.velocity = vel;
    }
}
