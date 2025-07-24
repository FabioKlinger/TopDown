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
    public EnemyHealth eh;
    
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
    
    
    #region AttackRegion

    [Header("Attack Region")]
    [SerializeField] private int damagePerHit = 1;
    [SerializeField] private float attackInterval = 2f;

    private Coroutine attackCoroutine;
    private bool playerInRegion = false;
    private PlayerInformation playerInfo;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&&eh.isDead == false)
        {
            playerInRegion = true;

            playerInfo = other.GetComponent<PlayerInformation>();

            if (playerInfo != null)
            {
                attackCoroutine = StartCoroutine(AttackPlayer());
            }
            else
            {
                Debug.LogWarning("Player hat kein PlayerInformation Script");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRegion = false;

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator AttackPlayer()
    {
        while (playerInRegion)
        {
            if (!eh.isDead)
            {
                anim.SetTrigger("attack");
                playerInfo.GetDamage(damagePerHit);
                yield return new WaitForSeconds(attackInterval);
            }
            else
            {
                
                eh.DisableColliders();
                anim.SetTrigger("die");
                eh.npcContainer.GetComponent<NavMeshAgent>().enabled = false;
                StartCoroutine(eh.RemoveAfterDeath());
                yield break; 
            }
        }
    }

    #endregion
}