using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPt;
    [SerializeField] GameObject heart;
    [SerializeField] float spawnRate;
    [SerializeField] float randomChance = 5;

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
        StartCoroutine(SpawnHearts(spawnRate));
    }
    public void SpawnHeart(){
        GameObject go = Instantiate(heart, spawnPt.position, Quaternion.identity);
        HeartCollectible heartCollectible = go.GetComponent<HeartCollectible>();
        heartCollectible.HeartSpawner = GetComponent<HeartSpawner>();
        empty = false;
    }

    IEnumerator SpawnHearts(float period){
        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time >= period && GameManager.instance.PlayerHealth < 3 && empty){
                int random = (int) Random.Range(0, randomChance);
                time = 0;
                if(random == 0){
                    SpawnHeart();
                }                
            }
            else{
                yield return null;
            }
            
        }


    }
}
