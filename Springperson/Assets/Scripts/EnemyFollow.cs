using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;

    [SerializeField] float projectileForce = 10f;
    [SerializeField] Transform _orientation;
    bool _ragDoll;

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
        //transform.rotation = Quaternion.LookRotation(_player.transform.position);

    }

    //Lets the enemy roll around without control
    public void TurnOnRagdoll()
    {
        StartCoroutine(WaitToGetUp());
        _rb.isKinematic = false;
        _navMeshAgent.enabled = false;
        _ragDoll = true;
    }

    private void TurnOffRagdoll()
    {
        _rb.isKinematic = true;
        _navMeshAgent.enabled = true;
        _ragDoll = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                if (other.gameObject.name != "Spring End" && !GameManager.instance.IsPoweredUp)
                {

                    if (other.gameObject.name != "Spring End" && !_ragDoll)
                    {

                        GameManager.instance.PlayerLoseHealth();
                    }
                }
                break;
            case "Hole":
                GameManager.instance.currentEnemy--;
                GameManager.instance.AddToScoreTotal(100);
                StopAllCoroutines();
                Destroy(gameObject);
                break;

            case "Projectile":
                Rigidbody projRb = other.gameObject.GetComponent<Rigidbody>();
                Projectile proj = other.gameObject.GetComponent<Projectile>();
                if (proj.Reversed)
                {
                    TurnOnRagdoll();
                    SoundBoard.instance.EnemyHitSound();
                    _rb.AddForce(projRb.velocity.normalized * projectileForce, ForceMode.Impulse);
                    Destroy(other.gameObject, 0.5f);
                }

                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !_ragDoll)
        {

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
}
