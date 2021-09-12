using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefabBasic;
    [SerializeField] GameObject enemyPrefabRanged;
    [SerializeField] Transform spawnPoint;

    [SerializeField] float randomMin = 3f;
    [SerializeField] float randomMax = 10f;

    private void Start()
    {
        StartCoroutine(WaitAndSpawn());
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(Random.Range(randomMin, randomMax));
        if (GameManager.instance.currentEnemy < GameManager.instance.totalEnemy)
        {
            int ranNum = Random.Range(0, 101);
            if(ranNum >= 50)
            {
                Instantiate(enemyPrefabBasic, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Instantiate(enemyPrefabRanged, spawnPoint.position, Quaternion.identity);
            }

            GameManager.instance.currentEnemy++;
        }
        StartCoroutine(WaitAndSpawn());
    }
}
