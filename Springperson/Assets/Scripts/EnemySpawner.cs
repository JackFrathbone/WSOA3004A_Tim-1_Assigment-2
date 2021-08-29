using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform spawnPoint;

    [SerializeField] float randomMin = 3f;
    [SerializeField] float randomMax = 10f;

    private GameObject currentSpawn;

    private void Start()
    {
        StartCoroutine(WaitAndSpawn());
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(Random.Range(randomMin, randomMax));
        if (currentSpawn == null)
        {
            currentSpawn = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
        StartCoroutine(WaitAndSpawn());
    }
}
