using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Spring Force Constants")]
    [SerializeField] float range;
    [SerializeField] float springSpd;

    [SerializeField] Transform fwdPt;
    [SerializeField] Rigidbody playerRb;
    Camera cam;

    [Header("Spring Force Constants")]
    [SerializeField] float springJumpForce = 100f; // jump force when player shoots ground
    [SerializeField] float enemyRecoilForce = 5f; // blow back force when player shoots an enemy
    [SerializeField] float wallJumpForce = 10f; // blow back force when player shoots wall
    [SerializeField] float enemyUpwardMod = 1f; //modifies the amount of updward force applied to enemies from spring
    [SerializeField] float enemyHitForce = 10f; // force applied to enemies when colliding with spring
    public GameObject springEnd;
    Coroutine extension = null;
    

    // Start is called before the first frame update
    void Start()
    {
        

        
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(springEnd.transform.position, (fwdPt.position - springEnd.transform.position).normalized, Color.red, 0.2f);
        if(Input.GetMouseButtonDown(0) && extension == null){
            extension = StartCoroutine(Extend(springSpd, range));
        }
    }
    IEnumerator Extend(float period, float amplitude){
        float w = (1/period) * 2 * Mathf.PI;
        float time = 0;
        Vector3 startPos = springEnd.transform.localPosition;
        bool collided = false;
        while(true){
            time += Time.deltaTime;
            float d = Mathf.Abs(amplitude * Mathf.Sin(w * time));
            Debug.Log(amplitude);
            RaycastHit hit;
            
            if(Physics.Raycast(springEnd.transform.position, (fwdPt.position - springEnd.transform.position).normalized, out hit, 0.1f) && !collided){
                collided = true;
                Vector3 dir = -(hit.point - springEnd.transform.position).normalized;
                amplitude = (springEnd.transform.position - startPos).magnitude;
                time = period/4;

                switch(hit.collider.tag){
                    case "Ground":
                    playerRb.AddForce(dir * springJumpForce, ForceMode.Impulse);
                    break;

                    case "Enemy":
                    playerRb.AddForce(dir * enemyRecoilForce, ForceMode.Impulse);
                    Rigidbody enemyRb = hit.collider.GetComponent<Rigidbody>();
                    Vector3 enemyDir = (-1*dir) + (Vector3.up * enemyUpwardMod);
                    enemyRb.AddForce(enemyDir * enemyHitForce, ForceMode.Impulse);

                    //[Jack] just letting the enemy know they have been hit and start the ragdoll
                    hit.collider.GetComponent<EnemyFollow>().TurnOnRagdoll();
                    break;

                    case "Wall":
                    playerRb.AddForce(dir * wallJumpForce, ForceMode.Impulse);
                    break;
                }

                Debug.Log("Jump");
            }
            if(time >= period/2 || springEnd.transform.position == startPos){
                springEnd.transform.localPosition = startPos;
                extension = null;
                break;
            }
            else{
                
                springEnd.transform.localPosition = startPos + Vector3.forward * d;
                yield return null;
            }
        }

    }

}
