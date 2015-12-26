using UnityEngine;
using System.Collections;

public class Enemy_Vital : MonoBehaviour {

    public float range;
    public GameObject rangeLight;
    Transform player;

    void Start ()
    {
        player = GameObject.Find("Player").transform;
    }

	void Update () {
        Range();
	}

    void Range ()
    {
        if (Vector3.Distance(player.position, transform.position) < range)
            rangeLight.SetActive(true);
        else
            rangeLight.SetActive(false);
    }
}
