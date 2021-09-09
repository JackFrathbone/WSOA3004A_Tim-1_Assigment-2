using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_navMeshAgent.enabled == true && _navMeshAgent.isOnNavMesh)
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
        if (other.tag == "Hole")
        {
            GameManager.instance.AddToScoreTotal(100);
            StopAllCoroutines();
            Destroy(gameObject, 2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.PlayerLoseHealth();
        }
    }

    IEnumerator WaitToGetUp()
    {
        yield return new WaitForSeconds(3f);

        if(_rb.velocity.magnitude < 0.5f)
        {
            TurnOffRagdoll();
        }
        else
        {
            StartCoroutine(WaitToGetUp());
        }
    }
}
