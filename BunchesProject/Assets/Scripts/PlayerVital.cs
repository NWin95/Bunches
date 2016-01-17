﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerVital : MonoBehaviour {

    public Animator anim;
    public CamScript camScript;
    public float deathTime;

    void HitByZapball ()
    {
        anim.SetBool("Zapped", true);

        GetComponent<WallRun>().WallExit();

        GetComponent<Movement_Player>().enabled = false;
        GetComponent<Dash_Player>().enabled = false;
        GetComponent<WallRun>().enabled = false;

        Rigidbody rig = GetComponent<Rigidbody>();
        camScript.playerTurn = false;

        rig.constraints = RigidbodyConstraints.None;
        rig.angularVelocity = transform.TransformVector(-50, 10, 0);

        StartCoroutine("DeathClock");
    }

    IEnumerator DeathClock ()
    {
        yield return new WaitForSeconds(deathTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}