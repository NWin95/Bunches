using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerVital : MonoBehaviour {

    public Animator anim;
    public CamScript camScript;
    public float deathTime;
    public AdScript ad;

    void HitByZapball ()
    {
        anim.SetBool("Zapped", true);

        GetComponent<WallRun>().WallExit();
        GetComponent<Movement_Player>().StopSlide();

        GetComponent<Movement_Player>().enabled = false;
        GetComponent<Dash_Player>().enabled = false;
        GetComponent<WallRun>().enabled = false;

        Rigidbody rig = GetComponent<Rigidbody>();
        camScript.playerTurn = false;

        rig.constraints = RigidbodyConstraints.None;
        rig.angularVelocity = transform.TransformVector(-50, 10, 0);

        StartCoroutine("DeathClock");
    }

    public void MusicMuteToggle()
    {
        AudioSource source = GameObject.Find("MusicA").GetComponent<AudioSource>();
        source.mute = !source.mute;
    }

    IEnumerator DeathClock ()
    {
        yield return new WaitForSeconds(deathTime);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (ad != null)
        {
                StartCoroutine(ad.ShowAd(1));
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuButton ()
    {
        SceneManager.LoadScene(0);
    }
}
