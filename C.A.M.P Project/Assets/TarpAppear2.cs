using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarpAppear2 : MonoBehaviour
{
    public GameObject tarp2; // reference to standing pole1

    public TentTriggerZone tentTriggerZone; // reference to TentTriggerZone script

    private bool triggered = false; // flag to track if trigger has been activated

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tarp") && !triggered)
        {
            Destroy(other.gameObject);
            tarp2.SetActive(true);
            triggered = true; // set the flag to true

            // Call the TarpExecuted method of TentTriggerZone script
            tentTriggerZone.Tarp2Executed();
        }
    }
}
