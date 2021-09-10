using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoard : Singleton<SoundBoard>
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource enemySrc;
    [SerializeField] AudioSource gunSrc;
    [SerializeField] AudioSource gameSrc;
    [SerializeField] AudioSource playerSrc;

    [Header("Audio Clips")]
    [SerializeField] AudioClip springSound;
    [SerializeField] AudioClip enemyHitSound;
    [SerializeField] AudioClip scoreUp;
    [SerializeField] AudioClip takeDamage;
    [SerializeField] AudioClip healUp;
    [SerializeField] AudioClip death;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnemyHitSound(){
        enemySrc.clip = enemyHitSound;
        enemySrc.Play();
    }
    public void SpringSound(){
        gunSrc.clip = springSound;
        gunSrc.Play();
    }
    public void ScoreUp(){
        gameSrc.clip = scoreUp;
        gameSrc.Play();
    }

    public void DeathSound(){
        gameSrc.clip = death;
        gameSrc.Play();
    }

    public void DamageSound(){
        gameSrc.clip = takeDamage;
        gameSrc.Play();
    }

    public void HealSound(){
        gameSrc.clip = healUp;
        gameSrc.Play();
    }
}
