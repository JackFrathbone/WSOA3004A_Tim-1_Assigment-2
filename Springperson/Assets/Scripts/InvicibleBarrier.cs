using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvicibleBarrier : MonoBehaviour
{
    [SerializeField] float enemyHitPower;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] PlayerController player;
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
        
    }
}
