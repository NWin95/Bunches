using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutTrigger : MonoBehaviour {

    public List<GameObject> enemies = new List<GameObject>();
    public List<Vector3> spawnPos = new List<Vector3>();
    public Vector3 checkPos;
    public GameObject gate;
    public int stepInt;
    public GameObject tutObj;

    void Start ()
    {
        if (transform.childCount > 0)
            gate = transform.GetChild(0).gameObject;
        foreach (GameObject enemy in enemies)
            spawnPos.Add(enemy.transform.position);
    }

    void OnTriggerEnter (Collider coll)
    {
        if (coll.tag == "Player")
        {
            TutorialScript ts = tutObj.GetComponent<TutorialScript>();
            ts.RunStep(stepInt);
            ts.checkPos = checkPos;
            Destroy(gate);

            ts.enemies = enemies;
            ts.spawnPos = spawnPos;
        }
    }
}
