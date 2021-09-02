using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Spring Force Constants")]
    [SerializeField] float range;
    [SerializeField] float springSpd;

    [SerializeField] Transform fwdPt;
    [SerializeField] Transform backPt;
    [SerializeField] Rigidbody playerRb;
    Camera cam;

    [Header("Spring Force Constants")]
    [SerializeField] float springJumpForce = 100f; // jump force when player shoots ground
    [SerializeField] float enemyRecoilForce = 5f; // blow back force when player shoots an enemy
    [SerializeField] float wallJumpForce = 10f; // blow back force when player shoots wall
    [SerializeField] float enemyUpwardMod = 1f; //modifies the amount of updward force applied to enemies from spring
    [SerializeField] LayerMask hitLayers;
    [SerializeField] float enemyHitForce = 10f; // force applied to enemies when colliding with spring
    public GameObject springEnd;

    public bool fired = false;
    public bool collided = false;
    Coroutine extension = null;
    [SerializeField] Vector3 sphereCastOffset = new Vector3(0,1,0);
    

    // Start is called before the first frame update
    void Start()
    {
        collided = false;
        fired = false;
  
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.DrawRay(springEnd.transform.position, (fwdPt.position - springEnd.transform.position).normalized, Color.red, 0.2f);
        if(Input.GetMouseButtonDown(0) && extension == null){
            extension = StartCoroutine(Extend(springSpd, range));
        }
    }

    
    IEnumerator Extend(float period, float amplitude){
        fired = true;
        float w = (1/period) * 2 * Mathf.PI;
        float time = 0;
        Vector3 startPos = springEnd.transform.localPosition;
        collided = false;
        while(true){
            time += Time.deltaTime;
            float d = Mathf.Abs(amplitude * Mathf.Sin(w * time));
            Debug.Log(amplitude);
            
            if(collided){
                fired = false;
                collided = false;
                amplitude = (springEnd.transform.position - startPos).magnitude;
                time = period/4;
                Debug.Log("Jump");
            }
            if(time >= period/2 || d < 0.1f){
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
