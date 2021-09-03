using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(_navMeshAgent.enabled == true && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.SetDestination(_player.transform.position);
        }
    }

    //Lets the enemy roll around without control
    public void TurnOnRagdoll()
    {
        StartCoroutine(WaitToGetUp());
        _rb.isKinematic = false;
        _navMeshAgent.enabled = false;
    }

    private void TurnOffRagdoll()
    {
        _rb.isKinematic = true;
        _navMeshAgent.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hole")
        {
            _gameManager.AddToScoreTotal(100);
            StopAllCoroutines();
            Destroy(gameObject, 5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _gameManager.PlayerLoseHealth();
        }
    }

    IEnumerator WaitToGetUp()
    {
        yield return new WaitForSeconds(5f);
        TurnOffRagdoll();
    }
}
