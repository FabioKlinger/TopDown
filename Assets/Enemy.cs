using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform startTarget;

    private Transform target;
    private Transform player;
    private NavMeshAgent agent;
    public int followPauseTimer = 2;
    

    private void OnEnable()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = startTarget;
    }

    private void FixedUpdate()
    {
        if (target == null) return;
        agent.SetDestination(target.position);
    }

    public void SetPlayerTarget(bool aggro)
    {
        if (aggro)
        {
            target = player;
        }
        else
        {
            StartCoroutine(FollowPauseThenReturn());
        }
    }

    private IEnumerator FollowPauseThenReturn()
    {
        yield return new WaitForSeconds(followPauseTimer);
        target = startTarget;
        
    }
}