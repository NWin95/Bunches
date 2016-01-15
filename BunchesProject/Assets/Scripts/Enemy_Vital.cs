using UnityEngine;
using System.Collections;

public class Enemy_Vital : MonoBehaviour {

    public float range;
    public GameObject rangeLight;
    Transform player;
    Material baseMat;
    public GameObject mesh;
    public Material rangeMat;
    bool inRange;

    void Start ()
    {
        player = GameObject.Find("Player").transform;
        GetComponent<Attack_Enemy>().player = player;
        baseMat = mesh.GetComponent<Renderer>().material;
    }

	void Update () {
        Range();
	}

    void Range ()
    {
        if (Vector3.Distance(player.position, transform.position) < range)
        {
            if (!inRange)
            {
                inRange = true;
                mesh.GetComponent<Renderer>().material = rangeMat;
                GetComponent<Attack_Enemy>().inRange = true;
            }
            //rangeLight.SetActive(true);
        }
        else
        {
            if (inRange)
            {
                inRange = false;
                mesh.GetComponent<Renderer>().material = baseMat;
                GetComponent<Attack_Enemy>().inRange = false;
            }
            //rangeLight.SetActive(false);
        }
    }
}
