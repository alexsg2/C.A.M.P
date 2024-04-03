using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BirdState
{
    Flying
}

[RequireComponent(typeof(NavMeshAgent))]
public class Bird : MonoBehaviour
{
    [Header("Flying")]
    public float flyDist = 40f;
    public float flySpeed = 5f; 
    public float maxFlyTime = 10f;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected BirdState currState = BirdState.Flying;

    private void Start() {
        InitalizeBird();
    }

    protected virtual void InitalizeBird()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = flySpeed;

        currState = BirdState.Flying;
        HandleFlyState();
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


    protected virtual void HandleFlyState()
    {
        StartCoroutine(WaitToReachDestination());
    }

    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            if (Time.time - startTime >= maxFlyTime)
            {
                agent.ResetPath();
                yield break;
            }
            yield return null;
        }

        // Destination reached
        currState = BirdState.Flying;
        OnStateChanged(BirdState.Flying);
    }

    protected virtual void OnStateChanged(BirdState newState)
    {
        animator?.CrossFadeInFixedTime(newState.ToString(), 0.25f);

        HandleFlyState();
    }
}

