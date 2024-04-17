using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalState
{
    Idle,
    Move
}

[RequireComponent(typeof(NavMeshAgent))]
public class Animal : MonoBehaviour
{
    [Header("Move")]
    public float moveDist = 10f;
    public float moveSpeed = 5f; 
    public float maxMoveTime = 4f;

    [Header("Idle")]
    public float idleTime = 5f;

    [Header("Attributes")]
    [SerializeField] private List<VisualRandomization> visualRandomization = new List<VisualRandomization>();

    protected NavMeshAgent agent;
    protected Animator animator;
    protected AnimalState currState = AnimalState.Idle;

    private void Start() {
        InitalizeAnimal();
    }

    protected virtual void InitalizeAnimal()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        currState = AnimalState.Idle;
        UpdateState();

        GenereateRandomVisual();
    }

    protected virtual void GenereateRandomVisual()
    {
        foreach(VisualRandomization randomization in visualRandomization)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material randomMaterial = randomization.materials[Random.Range(0, randomization.materials.Count)];
                renderer.material = randomMaterial;
            }
        }
    }

    protected virtual void UpdateState()
    {
        switch (currState)
        {
            case AnimalState.Idle:
                HandleIdleState();
                break;
            case AnimalState.Move:
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
        SetState(AnimalState.Move);
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
                SetState(AnimalState.Idle);
                yield break;
            }
            yield return null;
        }

        // Destination reached
        SetState(AnimalState.Idle);
    }

    protected void SetState(AnimalState newState)
    {
        if (currState == newState) {
            return;
        }
        currState = newState;
        OnStateChanged(newState);
    }

    protected virtual void OnStateChanged(AnimalState newState)
    {
        animator?.CrossFadeInFixedTime(newState.ToString(), 0.25f);

        UpdateState();
    }
}


[System.Serializable]
public class VisualRandomization
{
    public List<Material> materials = new List<Material>(); //Determine what color our animal will be
}
