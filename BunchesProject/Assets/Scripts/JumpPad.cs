using UnityEngine;
using System.Collections;

public class JumpPad : MonoBehaviour {

    public float jumpHeight;

	void OnTriggerEnter (Collider coll)
    {
        if (coll.tag == "Player" || coll.tag == "Enemy")
        {
            Rigidbody rig = coll.GetComponent<Rigidbody>();

            float jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.magnitude) * jumpHeight) + 0.25f;

            Vector3 vel = rig.velocity;
            vel.y = jumpVel;
            rig.velocity = vel;
        }
    }
}
