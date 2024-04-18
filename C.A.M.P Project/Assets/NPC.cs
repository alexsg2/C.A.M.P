using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NPCState
{
    Idle,
    Move
}

[RequireComponent(typeof(NavMeshAgent))]
public class NPC : MonoBehaviour
{
    [Header("Move")]
    public float moveDist = 10f;
    public float moveSpeed = 5f; 
    public float maxMoveTime = 4f;

    [Header("Idle")]
    public float idleTime = 5f;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected NPCState currState = NPCState.Idle;

    private void Start() {
        InitalizeNPC();
    }

    protected virtual void InitalizeNPC()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        currState = NPCState.Idle;
        UpdateState();
    }

    protected virtual void UpdateState()
    {
        switch (currState)
        {
            case NPCState.Idle:
                HandleIdleState();
                break;
            case NPCState.Move:
                HandleMoveState();
                break;
        }
    }

    protected Vector3 GetRandomNavMeshPosition(Vector3 origin, float dist)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, dist, NavMesh.AllAreas)) {
            return hit.position;
        }

        return GetRandomNavMeshPosition(origin, dist);
    }

    protected virtual void HandleIdleState()
    {
        StartCoroutine(WaitToMove());
    }

    private IEnumerator WaitToMove()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);

        Vector3 randomDest = GetRandomNavMeshPosition(transform.position, moveDist);
        agent.SetDestination(randomDest);
        SetState(NPCState.Move);
    }

    protected virtual void HandleMoveState()
    {
        StartCoroutine(WaitToReachDestination());
    }

    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            if (Time.time - startTime >= maxMoveTime)
            {
                agent.ResetPath();
                SetState(NPCState.Idle);
                yield break;
            }
            yield return null;
        }

        // Destination reached
        SetState(NPCState.Idle);
    }

    protected void SetState(NPCState newState)
    {
        if (currState == newState) {
            return;
        }
        currState = newState;
        OnStateChanged(newState);
    }

    protected virtual void OnStateChanged(NPCState newState)
    {
        animator?.CrossFadeInFixedTime(newState.ToString(), 0.25f);

        UpdateState();
    }
}

