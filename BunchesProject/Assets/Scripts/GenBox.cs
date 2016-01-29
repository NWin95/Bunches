using UnityEngine;
using System.Collections;

public class GenBox : MonoBehaviour {

    public GameObject enemyObj;
    public Vector3 dimmensions;
    public Vector3[] startPoints;
    public Vector3[] endPoints;
    public Vector3[] spawnPoints;
    public float[] spawnRots;

    public bool spawnedBox;

    void Start ()
    {
        Spawn();
    }

    void Spawn()
    {
        int spawnInt = 0;

        foreach (Vector3 point in spawnPoints)
        {
            Vector3 nr = point;
            nr.y += 1;

            Vector3 spawnPos = spawnPoints[spawnInt];
            spawnPos = transform.TransformPoint(spawnPos);

            Vector3 spawnEuler = Vector3.zero;
            spawnEuler.y = spawnRots[spawnInt];
            Quaternion spawnRot = Quaternion.Euler(spawnEuler);

            //GameObject spawnedEnemy = GameObject.Instantiate(enemyObj, spawnPos, spawnRot) as GameObject;
            Instantiate(enemyObj, spawnPos, spawnRot);
            spawnInt++;
        }
    }

    void OnTriggerEnter (Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!spawnedBox)
            {
                spawnedBox = true;
                transform.parent.GetComponent<GenBoxSpawner>().SpawnGenBox(/*gameObject*/);
                //Debug.Log("Collided");
            }
        }
    }
}
