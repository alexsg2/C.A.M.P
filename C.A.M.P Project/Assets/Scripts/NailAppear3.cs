using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailAppear3 : MonoBehaviour
{
    public GameObject nail3;

    public TentTriggerZone tentTriggerZone; // reference to TentTriggerZone script

    private bool triggered = false; // flag to track if trigger has been activated
    private int currNailHits = 0; // flag to track number of nail hits
    private int neededNailHits = 2; // constant to track number of needed nail hits

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nail") && !triggered && tentTriggerZone.checkTarp())
        {
            Destroy(other.gameObject);
            nail3.SetActive(true);
            triggered = true; // set the flag to true
        }

        if (other.CompareTag("Hammer") && (currNailHits != neededNailHits))
        {
            // Move the nail down slightly
            nail3.transform.position -= Vector3.up * 0.08f;

            // Increment hit counter
            currNailHits += 1;

            if (currNailHits == neededNailHits)
            {
                // Call the NailExecuted method of TentTriggerZone script
                tentTriggerZone.Nail3Executed();
            }
        }
    }
}