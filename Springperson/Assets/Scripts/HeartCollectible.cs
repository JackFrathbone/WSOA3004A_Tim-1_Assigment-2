using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollectible : MonoBehaviour
{
    [SerializeField] float spinSpd = 50f;

    HeartSpawner heartSpawner;

    public HeartSpawner HeartSpawner { get => heartSpawner; set => heartSpawner = value; }

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
        switch(other.tag){
            case "Player":
                if(GameManager.instance.PlayerHealth < 3){
                    GameManager.instance.PlayerAddHealth();
                    heartSpawner.Empty = true;
                    if(GameManager.instance.PlayerHealth < 3){
                        GameManager.instance.SpawnHeart(GameManager.instance.MaxHearts);
                    }
                    Destroy(this.gameObject);
                }
            break;
        }
    }
}
