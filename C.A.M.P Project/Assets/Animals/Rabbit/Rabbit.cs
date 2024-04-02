using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum RabbitState
{
    Idle,
    Run
}

[RequireComponent(typeof(NavMeshAgent))]
public class Rabbit : MonoBehaviour
{
    [Header("Run")]
    public float runDist = 10f;
    public float runSpeed = 5f; 
    public float maxRunTime = 4f;

    [Header("Idle")]
    public float idleTime = 5f;

    [Header("Attributes")]
    [SerializeField] private List<VisualRandomization> visualRandomization = new List<VisualRandomization>();

    protected NavMeshAgent agent;
    protected Animator animator;
    protected RabbitState currState = RabbitState.Idle;

    private void Start() {
        InitalizeRabbit();
    }

    protected virtual void InitalizeRabbit()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = runSpeed;

        currState = RabbitState.Idle;
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
            case RabbitState.Idle:
                HandleIdleState();
                break;
            case RabbitState.Run:
                HandleRunState();
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
        StartCoroutine(WaitToRun());
    }

    private IEnumerator WaitToRun()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);

        Vector3 randomDest = GetRandomNavMeshPosition(transform.position, runDist);
        agent.SetDestination(randomDest);
        SetState(RabbitState.Run);
    }

    protected virtual void HandleRunState()
    {
        StartCoroutine(WaitToReachDestination());
    }

    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            if (Time.time - startTime >= maxRunTime)
            {
                agent.ResetPath();
                SetState(RabbitState.Idle);
                yield break;
            }
            yield return null;
        }

        // Destination reached
        SetState(RabbitState.Idle);
    }

    protected void SetState(RabbitState newState)
    {
        if (currState == newState) {
            return;
        }
        currState = newState;
        OnStateChanged(newState);
    }

    protected virtual void OnStateChanged(RabbitState newState)
    {
        animator?.CrossFadeInFixedTime(newState.ToString(), 0.25f);

        UpdateState();
    }
}


[System.Serializable]
public class VisualRandomization
{
    public List<Material> materials = new List<Material>(); //Determine what color our animal (rabbit) will be
}
