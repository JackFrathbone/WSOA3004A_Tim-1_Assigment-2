using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    bool reversed;

    public bool Reversed { get => reversed; set => reversed = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        reversed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reverse(){
        if((rb.velocity.magnitude > 0)){
            Vector3 velocity = rb.velocity;
            velocity *= -1;
            rb.velocity = velocity;
        }
    }
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    
}
