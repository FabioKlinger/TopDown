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
    public Animator anim;
    
    public static readonly int Hash_dirX = Animator.StringToHash("dirX");

    public static readonly int Hash_dirY = Animator.StringToHash("dirY");

    public static readonly int Hash_MovementValue = Animator.StringToHash("MovementValue");

    

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
        
            UpdateAnimator();
        
            if (agent.desiredVelocity.x < 1)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (agent.velocity.x > 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            
            }
    }

    public void UpdateAnimator()
    {
        if (agent.desiredVelocity != Vector3.zero)
        {
            anim.SetFloat(Hash_dirX, agent.desiredVelocity.x);
            anim.SetFloat(Hash_dirY, agent.desiredVelocity.y);
        }
        anim.SetFloat(Hash_MovementValue, agent.desiredVelocity != Vector3.zero ? 1: 0);
        
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