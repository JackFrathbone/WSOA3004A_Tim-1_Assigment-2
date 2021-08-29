using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringCollisionManager : MonoBehaviour
{
    [SerializeField] float springJumpForce = 10f;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] Transform gun;
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
        /*
        Debug.Log("collision");
        switch(other.tag){
            case "ground":
            Debug.Log("Jump");
            Vector3 dir = transform.position - gun.position;
            dir = -dir.normalized;
            playerRb.AddForce(dir * springJumpForce, ForceMode.Impulse);
            Debug.Log(dir);
            break;

        }
        */
    }
}
