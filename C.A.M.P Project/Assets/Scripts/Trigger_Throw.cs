using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_ThrowTo : MonoBehaviour
{
    private bool throwto = false;
    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Made it");
        if (other.CompareTag("Box") && !triggered)
        {
            throwto = true;
            triggered = true;
            Debug.Log("Player got box in!");
        }
    }

    public bool checkThrowTo()
    {
        return throwto;
    }
}
