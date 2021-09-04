using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoard : Singleton<SoundBoard>
{
    [SerializeField] AudioSource enemySrc;
    [SerializeField] AudioSource gunSrc;

    [SerializeField] AudioClip springSound;
    [SerializeField] AudioClip enemyHitSound;
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
}
