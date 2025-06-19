using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMesh : MonoBehaviour
{
    private PlayerController playerController;
    private Animator anim;
    private Transform target;
    public NavMeshAgent agent;
    private bool isMoving;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = true;
        agent.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        Vector3 direction = agent.desiredVelocity.normalized;
        playerController.UpdateAutoMoveAnimator(direction);
        
        if (agent.remainingDistance <= .1f)
        {
            StopAgent();
        }
        
    }

    private void StopAgent()
    {
        agent.isStopped = true;
        isMoving = false;
        agent.enabled = false;
        
        target = null;
        
        playerController.AutoMovement(false);
    }

    public void MovePlayerToTarget(Transform newTarget)
    {
        target = newTarget;
        isMoving = true;
        agent.enabled = true;
        agent.isStopped = false;
        
        playerController.AutoMovement(true);
        
        agent.SetDestination(target.position);
    }
    
    
}