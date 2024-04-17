using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarpAppear1 : MonoBehaviour
{
    public GameObject tarp1; // reference to standing pole1

    public TentTriggerZone tentTriggerZone; // reference to TentTriggerZone script

    private bool triggered = false; // flag to track if trigger has been activated

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tarp") && !triggered)
        {
            Destroy(other.gameObject);
            tarp1.SetActive(true);
            triggered = true; // set the flag to true

            // Call the TarpExecuted method of TentTriggerZone script
            tentTriggerZone.Tarp1Executed();
        }
    }
}
