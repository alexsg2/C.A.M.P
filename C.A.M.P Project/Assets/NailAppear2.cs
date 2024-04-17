using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailAppear2 : MonoBehaviour
{
    public GameObject nail2; // reference to standing pole1

    public TentTriggerZone tentTriggerZone; // reference to TentTriggerZone script

    private bool triggered = false; // flag to track if trigger has been activated

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nail") && !triggered)
        {
            Destroy(other.gameObject);
            nail2.SetActive(true);
            triggered = true; // set the flag to true

            // Call the NailExecuted method of TentTriggerZone script
            tentTriggerZone.Nail2Executed();
        }
    }
}
