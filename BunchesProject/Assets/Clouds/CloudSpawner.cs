using UnityEngine;
using System.Collections;

public class CloudSpawner : MonoBehaviour {

    public GameObject cloud;

    public Vector2 amountRange;
    public Vector2 disRange;
    Quaternion rot;

	void Start () {
        SpawnClouds();
	}
	
	void SpawnClouds ()
    {
        rot = Quaternion.Euler(-90, 0, 0);
        float amount = Random.Range(amountRange.x, amountRange.y);

        while (amount > 0)
        {
            float dis = Random.Range(disRange.x, disRange.y);
            Vector3 offset = Random.insideUnitSphere * dis;
            offset.y = offset.y * 0.01f;

            Vector3 pos = offset + transform.position;

            GameObject spawnedCloud = Instantiate(cloud, pos, rot) as GameObject;
            spawnedCloud.transform.SetParent(transform);
            amount--;
        }
    }
}
