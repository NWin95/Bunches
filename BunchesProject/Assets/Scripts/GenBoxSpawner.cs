using UnityEngine;
using System.Collections;

public class GenBoxSpawner : MonoBehaviour {

    public GameObject[] boxes;

    Vector3 basePoint = new Vector3(0, -1000, -1000);

    void Start ()
    {
        SpawnGenBox();
        SpawnGenBox();
        SpawnGenBox();
        SpawnGenBox();
    }

    public void SpawnGenBox (/* GameObject box */)
    {
        if (transform.childCount > 0)
        {
            Transform lastChild = transform.GetChild(transform.childCount - 1);
            //Debug.Log(lastChild.name);

            int randInt = Random.Range(0, boxes.Length);
            Debug.Log(randInt);

            GameObject spawnedBox = GameObject.Instantiate(boxes[randInt], basePoint, Quaternion.identity) as GameObject;
            spawnedBox.transform.parent = transform;

            Vector3 spawnPos = lastChild.position + lastChild.GetComponent<GenBox>().endPoints[0];
            spawnPos -= spawnedBox.GetComponent<GenBox>().startPoints[0];

            spawnedBox.transform.position = spawnPos;
        }
        else
        {
            int randInt = Random.Range(0, boxes.Length);
            //Debug.Log(randInt);

            GameObject spawnedBox = GameObject.Instantiate(boxes[randInt], basePoint, Quaternion.identity) as GameObject;
            spawnedBox.transform.parent = transform;

            Vector3 spawnPos = new Vector3(0, 0, 6);
            spawnPos -= spawnedBox.GetComponent<GenBox>().startPoints[0];

            spawnedBox.transform.position = spawnPos;
        }
    }
}
