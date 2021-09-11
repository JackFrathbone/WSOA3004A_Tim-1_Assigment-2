using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPt;
    [SerializeField] GameObject heart;
    [SerializeField] float spawnRate = 3; // delay between spawning new hearts in seconds

    bool empty;

    public bool Empty { get => empty; set => empty = value; }

    // Start is called before the first frame update
    void Start()
    {
        empty = true;
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
        //StartCoroutine(SpawnHearts(spawnRate));
    }
    public void SpawnHeart(){
        StartCoroutine(SpawnDelay(spawnRate));
    }

    IEnumerator SpawnDelay(float time){
        GameObject go = Instantiate(heart, spawnPt.position, Quaternion.identity);
        HeartCollectible heartCollectible = go.GetComponent<HeartCollectible>();
        heartCollectible.HeartSpawner = GetComponent<HeartSpawner>();
        empty = false;
        yield return new WaitForSeconds(time);
    }
    
}
