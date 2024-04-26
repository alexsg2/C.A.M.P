using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Box : MonoBehaviour
{
    private bool grabdone = false;
    private bool triggered = false;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box") && !triggered)
        {
            grabdone = true;
            triggered = true;
        }
    }

    public bool checkGrab()
    {
        return grabdone;
    }
}
