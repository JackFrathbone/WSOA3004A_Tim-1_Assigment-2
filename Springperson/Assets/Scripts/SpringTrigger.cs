using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringTrigger : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] float springJumpForce = 100f; // jump force when player shoots ground
    [SerializeField] float enemyRecoilForce = 5f; // blow back force when player shoots an enemy
    [SerializeField] float wallJumpForce = 10f; // blow back force when player shoots wall
    [SerializeField] float enemyUpwardMod = 1f; //modifies the amount of updward force applied to enemies from spring
    [SerializeField] float enemyHitForce = 10f; // force applied to enemies when colliding with spring
    [SerializeField] Transform back;
    // Start is called before the first frame update
    void Start()
    {
        
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
        
        if(weapon.fired){
            switch(other.tag){
                    case "Ground":
                    weapon.collided = true;
                    playerRb.AddForce(dir * springJumpForce, ForceMode.Impulse);
                    break;

                    case "Enemy":
                    weapon.collided = true;
                    //[Jack] just letting the enemy know they have been hit and start the ragdoll
                    other.GetComponent<EnemyFollow>().TurnOnRagdoll();
                    playerRb.AddForce(dir * enemyRecoilForce, ForceMode.Impulse);
                    Rigidbody enemyRb = other.GetComponent<Rigidbody>();
                    Vector3 enemyDir = (-1*dir) + (Vector3.up * enemyUpwardMod);
                    enemyRb.AddForce(enemyDir * enemyHitForce, ForceMode.Impulse);

                    
                    break;

                    case "Wall":
                    weapon.collided = true;
                    weapon.collided = true;
                    playerRb.AddForce(dir * wallJumpForce, ForceMode.Impulse);
                    break;
                }
        }
        
    }
}
