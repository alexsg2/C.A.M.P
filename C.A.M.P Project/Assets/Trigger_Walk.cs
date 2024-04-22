using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Walk : MonoBehaviour
{
    private bool walkdone = false;
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            walkdone = true;
            triggered = true;
            Debug.Log("Player triggered the walk!");
        }
    }

    public bool checkWalk()
    {
        return walkdone;
    }
}
