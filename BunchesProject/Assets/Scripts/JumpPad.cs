using UnityEngine;
using System.Collections;

public class JumpPad : MonoBehaviour {

    public float jumpHeight;

	void OnTriggerEnter (Collider coll)
    {
        string tagString = coll.tag;
        if (tagString == "Player")
        {
            Rigidbody rig = coll.GetComponent<Rigidbody>();

            float jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.magnitude) * jumpHeight) + 0.25f;

            Vector3 vel = rig.velocity;
            vel.y = jumpVel;
            rig.velocity = vel;
        }
        else if (tagString == "Enemy")
        {
            Rigidbody rig = coll.GetComponent<Rigidbody>();

            float jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.magnitude) * jumpHeight) + 0.25f;

            Vector3 vel = rig.velocity;
            vel.y = jumpVel;
            rig.velocity = vel;

            coll.GetComponent<Attack_Enemy>().KickBoosted();
        }

    }
}
