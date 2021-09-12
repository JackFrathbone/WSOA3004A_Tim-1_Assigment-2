using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float spinSpd = 50f;
    HeartSpawner spawner;

    public HeartSpawner Spawner { get => spawner; set => spawner = value; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,1 * Time.deltaTime * spinSpd,0, Space.Self);
    }
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            
            GameManager.instance.StartPowerUp();
            spawner.Empty = true;
            spawner.PowerUpSpawned = false;
            Destroy(this.gameObject);

        }
    }
}
