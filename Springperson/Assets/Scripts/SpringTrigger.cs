using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringTrigger : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] float minSpringJumpForce = 40f; // minimum jump force when player shoots ground

    [SerializeField] float maxSpringJumpForce = 50f; // jump force when player shoots ground
    [SerializeField] float enemyRecoilForce = 5f; // blow back force when player shoots an enemy
    [SerializeField] float wallJumpForce = 10f; // blow back force when player shoots wall
    [SerializeField] float enemyUpwardMod = 1f; //modifies the amount of updward force applied to enemies from spring
    [SerializeField] float maxEnemyHitForce = 10f; // force applied to enemies when colliding with spring
    [SerializeField] float minEnemyhitForce = 3f; // minimum force to be applied to enemy (must be lower than enemy hit force)
    [SerializeField] float maxEnemyHitForcePowerUp = 15f;
    [SerializeField] float minEnemyHitForcePowerUp = 15f;

    [SerializeField] Transform back;
    


    float enemyHitDiff;
    float jumpDiff;
    float enemyHitDiffPowerUp;
    // Start is called before the first frame update
    void Start()
    {
        if(maxEnemyHitForce < minEnemyhitForce){
            Debug.Log("enemy hit force higher than minimum");
        }
        enemyHitDiff = maxEnemyHitForce - minEnemyhitForce;
        jumpDiff = maxSpringJumpForce - minEnemyhitForce;
        enemyHitDiffPowerUp = maxEnemyHitForcePowerUp - minEnemyHitForcePowerUp;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter");
        Vector3 dir = -(transform.position - back.position).normalized;
        
        if(weapon.getFired() && weapon.getVelocity() > 0){
            switch(other.tag){
                    case "Ground":
                    if(!weapon.collided){
                        SoundBoard.instance.SpringJump();
                        weapon.setCollided(true);
                        playerRb.AddForce(dir * (minSpringJumpForce + (jumpDiff * weapon.getVelocity())), ForceMode.Impulse);
                    }
                    
                    break;

                    case "Enemy":
                    weapon.collided = true;
                    SoundBoard.instance.EnemyHitSound();
                    //[Jack] just letting the enemy know they have been hit and start the ragdoll
                    if(other.GetComponent<EnemyFollow>() != null)
                    {
                        other.GetComponent<EnemyFollow>().TurnOnRagdoll();
                    } else if(other.GetComponent<EnemyRanged>() != null)
                    {
                        other.GetComponent<EnemyRanged>().TurnOnRagdoll();
                    }
              
                    playerRb.AddForce(dir * enemyRecoilForce, ForceMode.Impulse);
                    Rigidbody enemyRb = other.GetComponent<Rigidbody>();
                    Vector3 enemyDir = Vector3.one;
                    enemyDir = (-1*dir) + (Vector3.up * enemyUpwardMod);
                    if(GameManager.instance.IsPoweredUp){
                        enemyRb.AddForce(enemyDir * (minEnemyHitForcePowerUp + (enemyHitDiffPowerUp*weapon.getVelocity())), ForceMode.Impulse);
                    }
                    else{
                        enemyRb.AddForce(enemyDir * (minEnemyhitForce + (enemyHitDiff*weapon.getVelocity())), ForceMode.Impulse);
                    }
                    
                    weapon.setCollided(true);

                    
                    break;

                    case "Wall":
                    weapon.setCollided(true);
                    playerRb.AddForce(dir * wallJumpForce, ForceMode.Impulse);
                    break;
                }
        }
        
    }
}
