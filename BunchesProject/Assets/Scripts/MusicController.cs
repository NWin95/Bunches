using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{

    public AudioSource source;
    public AudioClip[] clips;
    public float songTime;

    void Start()
    {
        if (GameObject.Find("MusicA"))
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            gameObject.name = "MusicA";

            if (Application.isEditor)
            {
                source.clip = clips[0];
                source.Play();
            }
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            source.clip = clips[0];
            source.Play();
            StopCoroutine("SongChange");
        }
        else if (level == 1)
        {
            if (source.clip == clips[0])
                StartCoroutine("SongChange");
        }
        else if (level == 2)
        {
            source.Stop();
        }
    }

    IEnumerator SongChange()
    {
        int rand = Random.Range(1, clips.Length - 1);

        source.clip = clips[rand];
        songTime = source.clip.length;
        source.Play();


        yield return new WaitForSeconds(songTime);

        StartCoroutine("SongChange");
    }
}
