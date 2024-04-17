using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailAppear1 : MonoBehaviour
{
    public GameObject nail1; // reference to standing pole1

    public TentTriggerZone tentTriggerZone; // reference to TentTriggerZone script

    private bool triggered = false; // flag to track if trigger has been activated

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nail") && !triggered && tentTriggerZone.checkTarp())
        {
            Destroy(other.gameObject);
            nail1.SetActive(true);
            triggered = true; // set the flag to true

            // Call the NailExecuted method of TentTriggerZone script
            tentTriggerZone.Nail1Executed();
        }
    }
}