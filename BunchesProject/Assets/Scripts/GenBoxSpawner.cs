using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GenBoxSpawner : MonoBehaviour {

    public int startSpawnNumber;
    public GameObject[] boxes;

    Vector3 basePoint = new Vector3(0, -1000, -1000);
    public Transform player;
    public float fallY;
    public Transform resetColl;
//    public Light directionalLight;

    void Start ()
    {
        player = GameObject.Find("Player").transform;

        int count = startSpawnNumber;

        while (count > 0)
        {
            SpawnGenBox();
            count--;
        }
    }

    void Update ()
    {
        if (player.position.y < fallY)
            Reset();
    }

    void Reset ()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SpawnGenBox (/* GameObject box */)
    {
        Transform lastChild = transform.GetChild(transform.childCount - 1);
        int randInt = Random.Range(0, boxes.Length);

        GameObject spawnedBox = GameObject.Instantiate(boxes[randInt], basePoint, Quaternion.identity) as GameObject;
        StaticBatchingUtility.Combine(spawnedBox);
        spawnedBox.transform.parent = transform;

        Vector3 spawnPos = lastChild.position + lastChild.GetComponent<GenBox>().endPoints[0];
        spawnPos -= spawnedBox.GetComponent<GenBox>().startPoints[0];

        spawnedBox.transform.position = spawnPos;

        resetColl.position = player.position;

        if (transform.childCount > 10)
            Destroy(transform.GetChild(0).gameObject);
    }
}
