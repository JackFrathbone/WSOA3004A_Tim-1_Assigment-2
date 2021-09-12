using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPt;
    [SerializeField] Transform powerUpSpawnPt;

    [SerializeField] GameObject heart;
    [SerializeField] GameObject powerUp;
    [SerializeField] float spawnDelay = 3; // delay between spawning new hearts in seconds
    [SerializeField] float powerUpSpawnDelay = 3;

    bool empty;
    bool powerUpSpawned;
    public bool Empty { get => empty; set => empty = value; }
    public bool PowerUpSpawned { get => powerUpSpawned; set => powerUpSpawned = value; }

    // Start is called before the first frame update
    void Start()
    {
        empty = true;
        powerUpSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.GameOver){
            StopAllCoroutines();
        }
    }
    private void OnDisable() {
        StopAllCoroutines();
    }
    private void OnEnable() {
        //StartCoroutine(SpawnHearts(spawnDelay));
    }
    public void SpawnHeart(){
        empty = false;
        StartCoroutine(SpawnDelay(spawnDelay));
    }

    public void SpawnPowerUp(){
        empty = false;
        powerUpSpawned = true;
        StartCoroutine(PowerUpDelay(powerUpSpawnDelay));
    }

    IEnumerator SpawnDelay(float time){
        GameObject go = Instantiate(heart, spawnPt.position, Quaternion.identity);
        go.transform.SetParent(this.transform);
        HeartCollectible heartCollectible = go.GetComponent<HeartCollectible>();
        heartCollectible.HeartSpawner = GetComponent<HeartSpawner>();
        //empty = false;
        yield return new WaitForSeconds(time);
    }

    IEnumerator PowerUpDelay(float time){
        GameObject go = Instantiate(powerUp, powerUpSpawnPt.position, Quaternion.identity);
        go.transform.SetParent(this.transform);
        PowerUp power = go.GetComponent<PowerUp>();
        power.Spawner = GetComponent<HeartSpawner>();
        yield return new WaitForSeconds(time);

    }
    
}
