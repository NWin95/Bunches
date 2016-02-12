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

    void BackToLife ()
    {
        anim.SetBool("Zapped", false);

        GetComponent<Movement_Player>().enabled = true;
        GetComponent<Dash_Player>().enabled = true;
        GetComponent<WallRun>().enabled = true;

        Rigidbody rig = GetComponent<Rigidbody>();
        camScript.playerTurn = true;

        rig.constraints = RigidbodyConstraints.FreezeRotation;
        rig.angularVelocity = Vector3.zero;
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
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                GameObject.Find("TutorialObj").GetComponent<TutorialScript>().Reset(GetComponent<CapsuleCollider>());
                BackToLife();
            }
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void MenuButton ()
    {
        SceneManager.LoadScene(0);
    }
}
