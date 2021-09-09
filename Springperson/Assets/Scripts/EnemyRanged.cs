using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;

    private bool _chasingPlayer;

    private bool _canFire = true;

    [SerializeField] Transform projectileParent;
    [SerializeField] GameObject projectilePrefab;

    //In seconds
    [SerializeField] float firingSpeed;
    //How fast the projectile moves
    [SerializeField] float projectileSpeed;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();

        _chasingPlayer = true;
    }

    private void Update()
    {
        if (_chasingPlayer && _navMeshAgent.enabled == true && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_player.transform.position);
        }
        else if (!_chasingPlayer && _navMeshAgent.enabled == true && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity = Vector3.zero;
            _navMeshAgent.SetDestination(transform.position);
        }

        if (Physics.Linecast(transform.position, _player.transform.position, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.tag == "Player")
            {
                _chasingPlayer = false;
            }
            else
            {
                _chasingPlayer = true;
            }
        }

        if (_canFire && !_chasingPlayer)
        {
            StartCoroutine(WaitToFire());
        }
    }

    //Lets the enemy roll around without control
    public void TurnOnRagdoll()
    {
        StopAllCoroutines();
        StartCoroutine(WaitToGetUp());
        _rb.isKinematic = false;
        _navMeshAgent.enabled = false;
        _canFire = false;
    }

    private void TurnOffRagdoll()
    {
        _rb.isKinematic = true;
        _navMeshAgent.enabled = true;
        _canFire = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hole")
        {
            GameManager.instance.AddToScoreTotal(200);
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

        if (_rb.velocity.magnitude < 0.5f)
        {
            TurnOffRagdoll();
        }
        else
        {
            StartCoroutine(WaitToGetUp());
        }
    }

    IEnumerator WaitToFire()
    {
        _canFire = false;
        yield return new WaitForSeconds(firingSpeed);

        GameObject newProjectile = Instantiate(projectilePrefab, projectileParent.position, projectileParent.rotation);
        newProjectile.GetComponent<Rigidbody>().velocity = (_player.transform.position - newProjectile.transform.position).normalized * projectileSpeed;
        newProjectile.transform.LookAt(_player.transform);
        Destroy(newProjectile, 5f);

        _canFire = true;
    }
}
