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
public class Moving : MonoBehaviour
{

    [Header("Move")]
    public float moveDist = 10f;
    public float moveSpeed = 5f; 
    public float maxMoveTime = 4f;

    [Header("Idle")]
    public float idleTime = 5f;

    protected NavMeshAgent agent;
    public Animator animator;
    protected NPCState currState = NPCState.Idle;
    private bool allowMovement = true; // Flag to control movement
    private bool shouldFacePlayer = false;
    private Vector3 targetPosition;

    private void Start() {
        InitalizeNPC();
    }

    protected virtual void InitalizeNPC()
    {
        // animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        currState = NPCState.Idle;
        UpdateState();
    }

    private void Update()
    {
        if (shouldFacePlayer)
        {
            FaceTarget(targetPosition);
        }
    }

    protected virtual void UpdateState()
    {
        if (!allowMovement)
        {
            // Optionally, update animator to reflect no movement
            animator.SetFloat("MoveSpeed", 0f);
            return;
        }

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
    public void StopAndFacePlayer(Vector3 playerPosition)
    {
        Debug.Log("STOP Moving");
        allowMovement = false;
        StopAllCoroutines();  // Stop all coroutines to ensure no movement restarts
        SetState(NPCState.Idle);
        agent.ResetPath();
        // FaceTarget(playerPosition);
        targetPosition = playerPosition;
        shouldFacePlayer = true;
    }


    // private void FaceTarget(Vector3 targetPosition)
    // {
    //     Vector3 direction = (targetPosition - transform.position).normalized;
    //     if (direction.magnitude > 0.1f)  // Check if the player is not too close
    //     {
    //         Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    //         transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    //     }
    // }
    private void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            if (Quaternion.Angle(transform.rotation, lookRotation) < 1f)
            {
                transform.rotation = lookRotation; // Snap to final rotation
                shouldFacePlayer = false; // Stop rotating
            }
        }
    }

    public void ResumeNormalBehavior()
    {
        allowMovement = true; // Re-enable movement transitions
        shouldFacePlayer = false;
        UpdateState();
    }
    protected virtual void OnStateChanged(NPCState newState)
    {
        if (newState == NPCState.Idle)
        {
            animator.SetFloat("MoveSpeed", 0f); // Set Animator moveSpeed to 0 when idle
        }
        else if (newState == NPCState.Move)
        {
            animator.SetFloat("MoveSpeed", 0.33f); // Set Animator moveSpeed to NPC's moveSpeed when moving
        }

        UpdateState();
    }
}

