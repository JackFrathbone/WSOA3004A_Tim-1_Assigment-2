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
    [SerializeField] AudioClip springJump;
    [SerializeField] AudioClip jump;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable() {
        StopAllCoroutines();
    }
    public void EnemyHitSound(){
        enemySrc.clip = enemyHitSound;
        enemySrc.Play();
        
        
    }
    public void SpringSound(){
        gunSrc.clip = springSound;
        gunSrc.Play();
    }

    public void SpringJump(){
        gunSrc.clip = springJump;
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
        playerSrc.clip = healUp;
        playerSrc.Play();
    }
    public void JumpSound(){
        playerSrc.clip = jump;
        playerSrc.Play();
    }

    IEnumerator PlayAndDestroy(AudioSource src, AudioClip clip){
        src.clip = clip;
        src.Play();
        while(true){
            if(!src.isPlaying){
                Destroy(src.gameObject);
                break;
            }
            else{
                yield return null;
            }

        }

    }
}
