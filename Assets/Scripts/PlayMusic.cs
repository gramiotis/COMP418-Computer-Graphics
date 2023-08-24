using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioSource mainSource;
    public AudioClip mainClip;
    public AudioClip soldierDeath;
    public AudioClip soldierAttack;
    public AudioClip plagueAttack;
    public AudioClip bossMusic;
    public AudioClip mining;
    public AudioClip cutting;

    // Start is called before the first frame update
    void Start()
    {
        mainSource.PlayOneShot(mainClip);
        mainSource.PlayScheduled(AudioSettings.dspTime + mainClip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBossMusic()
    {
        mainSource.Stop();

        mainSource.PlayOneShot(bossMusic);
        mainSource.PlayScheduled(AudioSettings.dspTime + bossMusic.length);
    }

    public void SoldierDeath()
    {
        mainSource.PlayOneShot(soldierDeath);
    }

    public void SoldierAttack()
    {
        mainSource.PlayOneShot(soldierAttack);
    }

    public void PlagueAttack()
    {
        mainSource.PlayOneShot(plagueAttack);
    }

    public void Mining()
    {
        mainSource.PlayOneShot(mining);
    }

    public void Cutting()
    {
        mainSource.PlayOneShot(cutting);
    }
}
