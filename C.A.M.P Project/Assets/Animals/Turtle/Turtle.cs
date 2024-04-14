using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TurtleState
{
    Idle,
    Walk,
    Hide,
    Unhide
}

[RequireComponent(typeof(NavMeshAgent))]
public class Turtle : MonoBehaviour
{
    [Header("Jump")]
    public float jumpDist = 10f;
    public float jumpSpeed = 5f; 
    public float maxJumpTime = 4f;

    [Header("Idle")]
    public float idleTime = 5f;

    // [Header("Attributes")]
    // [SerializeField] private List<VisualRandomization> visualRandomization = new List<VisualRandomization>();

    protected NavMeshAgent agent;
    protected Animator animator;
    protected FrogState currState = FrogState.Idle;

    private void Start() {
        InitalizeFrog();
    }

    protected virtual void InitalizeFrog()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = jumpSpeed;

        currState = FrogState.Idle;
        UpdateState();
    
        // GenereateRandomVisual();
    }

    // protected virtual void GenereateRandomVisual()
    // {
    //     foreach(VisualRandomization randomization in visualRandomization)
    //     {
    //         Renderer[] renderers = GetComponentsInChildren<Renderer>();
    //         foreach (Renderer renderer in renderers)
    //         {
    //             Material randomMaterial = randomization.materials[Random.Range(0, randomization.materials.Count)];
    //             renderer.material = randomMaterial;
    //         }
    //     }
    // }

    protected virtual void UpdateState()
    {
        switch (currState)
        {
            case FrogState.Idle:
                HandleIdleState();
                break;
            case FrogState.Jump:
                HandleJumpState();
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
        StartCoroutine(WaitToJump());
    }

    private IEnumerator WaitToJump()
    {
        float waitTime = Random.Range(idleTime / 2, idleTime * 2);
        yield return new WaitForSeconds(waitTime);

        Vector3 randomDest = GetRandomNavMeshPosition(transform.position, jumpDist);
        agent.SetDestination(randomDest);
        SetState(FrogState.Jump);
    }

    protected virtual void HandleJumpState()
    {
        StartCoroutine(WaitToReachDestination());
    }

    private IEnumerator WaitToReachDestination()
    {
        float startTime = Time.time;
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            if (Time.time - startTime >= maxJumpTime)
            {
                agent.ResetPath();
                SetState(FrogState.Idle);
                yield break;
            }
            yield return null;
        }

        // Destination reached
        SetState(FrogState.Idle);
    }

    protected void SetState(FrogState newState)
    {
        if (currState == newState) {
            return;
        }
        currState = newState;
        OnStateChanged(newState);
    }

    protected virtual void OnStateChanged(FrogState newState)
    {
        animator?.CrossFadeInFixedTime(newState.ToString(), 0.25f);

        UpdateState();
    }
}


// [System.Serializable]
// public class VisualRandomization
// {
//     public List<Material> materials = new List<Material>(); //Determine what color our animal (rabbit) will be
// }

